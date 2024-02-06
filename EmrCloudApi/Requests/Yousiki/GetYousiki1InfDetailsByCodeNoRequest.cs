namespace EmrCloudApi.Requests.Yousiki
{
    public class GetYousiki1InfDetailsByCodeNoRequest
    {
        public int SinYm { get; set; }

        public long PtId { get; set; }

        public int DataType { get; set; }

        public int SeqNo { get; set; }

        public string CodeNo {  get; set; } = string.Empty;
    }
}
