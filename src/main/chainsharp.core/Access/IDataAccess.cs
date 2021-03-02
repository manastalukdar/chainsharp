using System.Collections.Generic;

namespace chainsharp.core.Access
{
    public interface IDataAccess<T>
    {
        #region Public Methods

        IEnumerable<string> GetKeys();

        bool KeyExists(string key);

        bool TryAdd(string key, T value);

        bool TryAddOrUpdate(string key, T value);

        (bool, T) TryDelete(string key);

        bool TryGet(string key, out T value);

        bool TryUpdate(string key, T value);

        #endregion Public Methods
    }
}
