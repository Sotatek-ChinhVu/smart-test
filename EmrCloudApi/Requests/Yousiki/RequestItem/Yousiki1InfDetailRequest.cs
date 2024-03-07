namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class Yousiki1InfDetailRequest
    {
        public long PtId { get; set; }

        public int SinYm { get; set; }

        public int DataType { get; set; }

        public int SeqNo { get; set; }

        public string CodeNo { get; set; } = string.Empty;

        public int RowNo { get; set; }

        public int Payload { get; set; }

        public string Value { get; set; } = string.Empty;

        public int IsDeleted { get; set; }
    }
}
