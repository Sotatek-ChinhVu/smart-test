using EmrCloudApi.Tenant.Responses.KarteInf;
using EmrCloudApi.Tenant.Responses.OrdInf;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class UpsertTodayOdrResponse
    {
        public UpsertTodayOdrResponse(UpsertTodayOrdStatus status, RaiinInfItemResponse validationRaiinInf, List<ValidationOrdInfListItemResponse> validationOdrInfs, List<ValidationKarteInfListItemResponse> validationKartes)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrInfs = validationOdrInfs;
            ValidationKartes = validationKartes;
        }
        public UpsertTodayOrdStatus Status { get; private set; }
        public RaiinInfItemResponse ValidationRaiinInf { get; private set; }
        public List<ValidationOrdInfListItemResponse> ValidationOdrInfs { get; private set; }
        public List<ValidationKarteInfListItemResponse> ValidationKartes { get; private set; }
    }
}
