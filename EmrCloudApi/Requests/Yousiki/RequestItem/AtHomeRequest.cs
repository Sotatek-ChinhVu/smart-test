namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class AtHomeRequest
    {
        public List<Yousiki1InfDetailRequest>? Yousiki1InfDetails { get; set; }

        public List<StatusVisitRequest> StatusVisits { get; set; } = new();

        public List<StatusVisitRequest> StatusVisitNursingList { get; set; } = new();

        public List<StatusEmergencyConsultationRequest> StatusEmergencyConsultations { get; set; } = new();

        public List<StatusShortTermAdmissionRequest> StatusShortTermAdmissions { get; set; } = new();

        public List<PatientSitutationRequest> PatientSitutations { get; set; } = new();

        public List<BarthelIndexRequest> BarthelIndexs { get; set; } = new();

        public List<StatusNurtritionRequest> StatusNurtritions { get; set; } = new();

        public List<CommonForm1Request> HospitalizationStatus { get; set; } = new();

        public List<CommonForm1Request> StatusHomeVisits { get; set; } = new();

        public CommonForm1Request FinalExaminationInf { get; set; } = new();

        public CommonForm1Request FinalExaminationInf2 { get; set; } = new();
    }
}
