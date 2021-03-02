using System;
using System.Threading;
using System.Threading.Tasks;

namespace chainsharp.helper
{
    public static class PeriodicTaskFactory
    {
        public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    action();
                }

                await Task.Delay(period, cancellationToken);
            }
        }

        public static Task Run(Action action, TimeSpan period)
        {
            return Run(action, period, CancellationToken.None);
        }
    }
}
