using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chainsharp.core.Storage.Models.BaseItem
{
    public interface IBaseItem
    {
        byte[] Data { get; set; }

        int Size { get; set; }

        int StartPosition { get; set; }

        int EndPosition { get; set; }
    }
}
