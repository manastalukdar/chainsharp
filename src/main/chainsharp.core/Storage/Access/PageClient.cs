using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chainsharp.core.Storage.Models;

namespace chainsharp.core.Storage.Access
{
    public static class PageClient
    {
        public static Page InsertIntoPage(Page page, byte[] content)
        {
            var startIndex = page.Size - page.FreeSpace - 1;
            content.CopyTo(page.Contents, startIndex);
            return page;
        }

        public static Page DeleteFromPage(Page page, int startIndex, int endIndex)
        {
            var contentsList = page.Contents.ToList();
            var count = (endIndex - startIndex) + 1; // todo: Bug?
            contentsList.RemoveRange(startIndex, count);
            page.Contents = contentsList.ToArray();
            return page;
        }
    }
}
