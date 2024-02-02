namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class CommonForm1ModelRequest
    {
        public List<UpdateYosiki1InfDetailRequestItem> CommonForm1ModelRequestItems { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> DiagnosticInjuryListRequestItems { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> HospitalizationStatusInf { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> FinalExaminationInf { get; set; } = new();

        public List<ValueSelectObjectRequest> ValueSelectObjectRequest { get; set; } = new();
    }
}
