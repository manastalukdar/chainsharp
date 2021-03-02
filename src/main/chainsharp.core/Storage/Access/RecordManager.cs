using System.Collections.Generic;
using chainsharp.core.Storage.Models;
using chainsharp.core.Storage.Models.BaseItem;

namespace chainsharp.core.Storage.Access
{
    public class RecordManager
    {
        public RecordManager()
        {
            Pages = new List<Page>
            {
                PageFactory.GetPage()
            };
        }

        public List<Page> Pages { get; }

        public bool AddRecord(Record record)
        {
            var activePage = Pages[Pages.Count-1];
            if (CanInsertIntoActivePage(record, activePage))
            {
                PageClient.InsertIntoPage(activePage, record.Data);
            }

            return true;
        }

        private bool CanInsertIntoActivePage(Record record, Page activePage)
        {
            if (record.Size > activePage.FreeSpace)
            {
                return false;
            }

            return true;
        }
    }
}
