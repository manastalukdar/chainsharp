using chainsharp.logging;
using Microsoft.AspNetCore.Hosting;

namespace chainsharp.server
{
    public class GlobalData
    {
        #region Public Properties

        public string LocalStorageLocation { get; set; }

        public ILoggingService Logger { get; set; }

        public IWebHost WebHost { get; set; }

        #endregion Public Properties
    }
}
