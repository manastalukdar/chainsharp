namespace chainsharp.core.Storage.Models.BaseItem
{
    public class Record : IBaseItem
    {
        public byte[] Data { get; set; }

        public int Size { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
}
