using chainsharp.core.Access;

namespace chainsharp.server
{
    public class DataStore
    {
        #region Public Constructors

        public DataStore()
        {
            Store = new ConcurrentDictAccess<byte[]>();
        }

        #endregion Public Constructors

        #region Public Properties

        public IDataAccess<byte[]> Store
        {
            get;
        }

        #endregion Public Properties
    }
}
