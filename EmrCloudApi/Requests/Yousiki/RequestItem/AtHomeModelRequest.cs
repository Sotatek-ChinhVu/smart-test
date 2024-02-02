namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class AtHomeModelRequest
    {
        public List<StatusVisitModelRequest> StatusVisitList { get; set; } = new();

        public List<StatusVisitModelRequest> StatusVisitNursingList { get; set; } = new();

        public List<StatusEmergencyConsultationModelRequest> StatusEmergencyConsultationList { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> HospitalizationStatusList { get; set; } = new();

        public List<StatusShortTermAdmissionModelRequest> StatusShortTermAdmissionList { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> StatusHomeVisitList { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> FinalExaminationInf { get; set; } = new();

        public List<HospitalizationStatusListRequestItem> FinalExaminationInf2 { get; set; } = new();

        public List<PatientSitutationListRequestItem> PatientSitutationListRequestItems { get; set; } = new();

        public List<BarthelIndexListRequest> BarthelIndexListRequestItems { get; set; } = new();

        public List<StatusNurtritionListRequest> StatusNurtritionListRequestIItems { get; set; } = new();
    }
}
