namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class OutpatientConsultationInfRequest
    {
        public Yousiki1InfDetailRequest ConsultationDate { get; set; } = new();

        public Yousiki1InfDetailRequest FirstVisit { get; set; } = new();

        public Yousiki1InfDetailRequest AppearanceReferral { get; set; } = new();

        public Yousiki1InfDetailRequest DepartmentCode { get; set; } = new();

        public bool IsDeleted { get; set; }

        public bool IsEnableReferral { get => FirstVisit.Value == "1"; }
    }
}
