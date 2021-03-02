namespace chainsharp.logging
{
    public interface ILoggingService
    {
        #region Public Methods

        void Log(ILogData data);

        #endregion Public Methods
    }
}
