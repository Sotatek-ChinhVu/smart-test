namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class OutpatientConsultationRequest
    {
        public Yousiki1InfDetailRequest ConsultationDate { get; set; } = new();

        public Yousiki1InfDetailRequest FirstVisit { get; set; } = new();

        public Yousiki1InfDetailRequest Referral { get; set; } = new();

        public Yousiki1InfDetailRequest DepartmentCode { get; set; } = new();

        public int RowNo { get; set; }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinYm { get; set; }

        public int DataType { get; set; }

        public int SeqNo { get; set; }

        public string CodeNo { get; set; } = string.Empty;

        public string DepartmentCodeDisplay { get; set; } = string.Empty;
    }
}
