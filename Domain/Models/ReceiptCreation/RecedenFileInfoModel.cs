namespace Domain.Models.ReceiptCreation
{
    public class RecedenFileInfoModel
    {
        public string Prefix { get; private set; }

        public string FileName { get; private set; }

        public int CreateDate { get; private set; }

        public int CreateTime { get; private set; }

        public RecedenFileInfoModel(string filename, int createDate, int createTime, string prefix = "")
        {
            FileName = filename;
            CreateDate = createDate;
            CreateTime = createTime;
            Prefix = prefix;
        }
    }
}
