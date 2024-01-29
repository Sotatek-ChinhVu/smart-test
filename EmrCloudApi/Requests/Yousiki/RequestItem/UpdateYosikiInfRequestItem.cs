namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class UpdateYosikiInfRequestItem
    {
        public long PtId { get; set; }

        public string SinYmDisplay { get; set; } = string.Empty;

        public int DataType { get; set; }

        public int Status { get; set; }

        public int SeqNo { get; set; }

        public int IsDeleted { get; set; }
    }
}
