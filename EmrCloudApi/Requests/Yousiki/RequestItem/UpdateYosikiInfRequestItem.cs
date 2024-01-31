namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class UpdateYosikiInfRequestItem
    {
        public long PtId { get; set; }

        public int SinYm{ get; set; } 

        public int DataType { get; set; }

        public int Status { get; set; }

        public int SeqNo { get; set; }

        public int IsDeleted { get; set; }
    }
}
