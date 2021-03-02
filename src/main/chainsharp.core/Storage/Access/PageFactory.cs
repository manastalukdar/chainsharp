using chainsharp.common;
using chainsharp.core.Storage.Models;

namespace chainsharp.core.Storage.Access
{
    public static class PageFactory
    {
        public static Page GetPage()
        {
            return new Page(Constants.PageSize);
        }
    }
}
