using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chainsharp.helper;

namespace chainsharp.scale
{
    public class Engine : IDisposable
    {
        const int _taskPool = Constants.TaskPool;

        private List<string> _keys;
        private SemaphoreSlim _lock;

        private DatabaseClient _databaseClient;

        private readonly CancellationTokenSource _periodicWriteRateCalculatorCts;
        private readonly CancellationTokenSource _periodicReadRateCalculatorCts;
        private long _writeCount;
        private long _readCount;
        private Task _periodicWriteRateCalculator;
        private Task _periodicReadRateCalculator;

        public Engine()
        {
            _keys = new List<string>();
            _lock = new SemaphoreSlim(1, 1);
            _databaseClient = new DatabaseClient(Constants.EndpointUrl);

            _periodicWriteRateCalculatorCts = new CancellationTokenSource();
            _periodicReadRateCalculatorCts = new CancellationTokenSource();
        }

        public int WriteRate { get; private set; }

        public int ReadRate { get; private set; }


        public async Task WriteTaskAsync()
        {
            if (_periodicWriteRateCalculator == null)
            {
                _periodicWriteRateCalculator = PeriodicTaskFactory.Run(CalculateWriteRate, TimeSpan.FromSeconds(5), _periodicWriteRateCalculatorCts.Token);
            }

            Task[] tasks = new Task[_taskPool];

            var rand = new Random();
            for (int i = 0; i < _taskPool; i++)
            {
                var key = Guid.NewGuid().ToString();
                var value = rand.Next(Int32.MaxValue).ToString();
                tasks[i] = Task.Factory.StartNew(async () =>
                {
                    var response = await _databaseClient.SendWriteRequestAsync(key, value);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"{key} - {response.StatusCode}");
                    }
                    else
                    {
                        lock(_lock)
                        {
                            _keys.Add(key);
                        }

                        Interlocked.Increment(ref _writeCount);
                    }

                });
            }

            await Task.WhenAll(tasks);
        }

        public async Task ReadTaskAsync()
        {
            if (_periodicReadRateCalculator == null)
            {
                _periodicReadRateCalculator = PeriodicTaskFactory.Run(CalculateReadRate, TimeSpan.FromSeconds(5), _periodicReadRateCalculatorCts.Token);
            }

            var keyCount = 0;
            lock (_lock)
            {
                keyCount = _keys.Count;
            }

            Task[] tasks = new Task[keyCount];

            var keys = _keys.GetRange(0, _taskPool);
            while(keys.Count > 0)
            {
                keys = _keys.GetRange(0, Math.Min(_keys.Count, _taskPool));
                int i = 0;
                foreach(var key in keys)
                {
                    tasks[i++] = Task.Factory.StartNew(async () =>
                    {
                        var response = await _databaseClient.SendReadRequestAsync(key);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"{key} - {response.StatusCode}");
                        }
                        else
                        {
                            Interlocked.Increment(ref _readCount);
                        }

                    });
                }
            }

            await Task.WhenAll(tasks);
        }

        private void CalculateWriteRate()
        {
            var interval = TimeSpan.FromSeconds(5).TotalSeconds;
            var writeCount = Interlocked.Read(ref _writeCount);
            WriteRate = (int)(writeCount / interval);
            if (writeCount > 0)
            {
                Interlocked.Exchange(ref _writeCount, 0);
            }

            Console.WriteLine($"writeCount: {writeCount}; WriteRate: {WriteRate}");
        }

        private void CalculateReadRate()
        {
            var interval = TimeSpan.FromSeconds(5).TotalSeconds;
            var readCount = Interlocked.Read(ref _readCount);
            ReadRate = (int)(readCount / interval);
            if (readCount > 0)
            {
                Interlocked.Exchange(ref _readCount, 0);
            }

            Console.WriteLine($"readCount: {readCount}; ReadRate: {ReadRate}");
        }

        public void Dispose()
        {
            _periodicWriteRateCalculatorCts.Cancel();
            _periodicReadRateCalculatorCts.Cancel();
        }
    }
}
