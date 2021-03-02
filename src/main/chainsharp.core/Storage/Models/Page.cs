namespace chainsharp.core.Storage.Models
{
    public class Page
    {
        public Page(int size)
        {
            Size = size;
            Contents = new byte[size];
        }

        public int Size { get; set; }

        public byte[] Contents { get; set; }

        public int FreeSpace { get; set; }
    }
}
