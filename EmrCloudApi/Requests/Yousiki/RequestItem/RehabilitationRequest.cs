namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class RehabilitationRequest
    {
        public List<Yousiki1InfDetailRequest> Yousiki1InfDetails { get; set; } = new();

        public List<OutpatientConsultationRequest> OutpatientConsultations { get; set; } = new();

        public List<CommonForm1Request> ByomeiRehabilitations { get; set; } = new();

        public List<PatientStatusRequest> BarthelIndexs { get; set; } = new();

        public List<PatientStatusRequest> Fims { get; set; } = new();
    }
}
