using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.HashFunction.SpookyHash;
using System.IO;
using System.Text;

namespace chainsharp.core.Access
{
    public class ConcurrentDictAccess<T> : IDataAccess<T>
    {
        #region Private Fields

        private ConcurrentDictionary<string, T> _dictionary;

        #endregion Private Fields

        #region Public Constructors

        public ConcurrentDictAccess()
        {
            _dictionary = new ConcurrentDictionary<string, T>();
        }

        #endregion Public Constructors

        #region Public Methods

        public IEnumerable<string> GetKeys()
        {
            return _dictionary.Keys;
        }

        public bool KeyExists(string key)
        {
            return _dictionary.TryGetValue(key, out T value);
        }

        public bool TryAdd(string key, T value)
        {
            if (!_dictionary.TryGetValue(key, out T extantValue))
            {
                return _dictionary.TryAdd(key, value);
            }

            return false;
        }

        public bool TryAddOrUpdate(string key, T value)
        {
            _dictionary.AddOrUpdate(
                key,
                value,
                (k, oldValue) => value);
            return true;
        }

        public (bool, T) TryDelete(string key)
        {
            if (_dictionary.TryRemove(key, out T value))
            {
                return (true, value);
            }
            else
            {
                return (false, value);
            }
        }

        public bool TryGet(string key, out T value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool TryUpdate(string key, T value)
        {
            if (_dictionary.TryGetValue(key, out T extantValue))
            {
                return _dictionary.TryUpdate(key, value, extantValue);
            }

            return false;
        }

        #endregion Public Methods
    }
}
