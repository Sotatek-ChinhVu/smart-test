namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class CommonForm1ModelRequest
    {
        public List<HospitalizationStatusListRequestItem> DiagnosticInjuryListRequestItems { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> HospitalizationStatusInf { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> FinalExaminationInf { get; set; } = new();

        public List<UpdateYosiki1InfDetailRequestItem> CommonForm1ModelRequestItems { get; set; } = new();

        public List<PatientSitutationListRequestItem> PatientSitutationListRequestItems { get; set; } = new();

        public List<BarthelIndexListRequest> BarthelIndexListRequestIItems { get; set; } = new();

        public List<StatusNurtritionListRequest> StatusNurtritionListRequestIItems { get; set; } = new();
    }
}
