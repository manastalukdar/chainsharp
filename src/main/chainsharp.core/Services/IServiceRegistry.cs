namespace chainsharp.core.Services
{
    public interface IServiceRegistry
    {
        #region Public Methods

        T GetService<T>();

        void SetService<T>(T service);

        #endregion Public Methods
    }
}
