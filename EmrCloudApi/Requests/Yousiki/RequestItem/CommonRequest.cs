namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class CommonRequest
    {
        public List<Yousiki1InfDetailRequest> Yousiki1InfDetails { get; set; } = new();

        public List<CommonForm1Request> DiagnosticInjurys { get; set; } = new();

        public InputByomeiCommonRequest HospitalizationStatusInf { get; set; } = new();

        public InputByomeiCommonRequest FinalExaminationInf { get; set; } = new();
    }
}
