namespace chainsharp.logging
{
    public interface ILogData
    {
        #region Public Properties

        string OperationId { get; set; }
        string TenantId { get; set; }

        #endregion Public Properties
    }
}
