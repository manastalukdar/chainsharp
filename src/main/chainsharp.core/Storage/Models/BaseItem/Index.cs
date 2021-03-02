using System.Data.HashFunction.SpookyHash;
using System.IO;
using System.Text;

namespace chainsharp.core.Storage.Models.BaseItem
{
    public class Index : IBaseItem
    {
        public Index(string index)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(index);
            MemoryStream stream = new MemoryStream(byteArray);

            ISpookyHashV2 spookyHash = SpookyHashV2Factory.Instance.Create();
            var hashValue = spookyHash.ComputeHash(stream);

            Id = hashValue.AsBase64String();
        }

        public string Id { get; }

        public byte[] Data { get; set; }

        public int Size { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
}
