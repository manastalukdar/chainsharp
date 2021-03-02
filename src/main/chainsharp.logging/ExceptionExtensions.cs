using System;

namespace chainsharp.logging
{
    public static class ExceptionExtensions
    {
        public static string GetFullExceptionMessage(this Exception ex)
        {
#if DEBUG
            return ex.ToString();
#else
            var message = ex.Message;
            while (ex.InnerException != null)
            {
                message += "||InnerException: " + ex.InnerException.Message;
                ex = ex.InnerException;
            }

            return message;
#endif
        }
    }
}
