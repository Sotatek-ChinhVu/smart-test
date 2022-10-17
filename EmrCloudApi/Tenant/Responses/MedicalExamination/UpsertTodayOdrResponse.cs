using EmrCloudApi.Tenant.Responses.KarteInf;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class UpsertTodayOdrResponse
    {
        public UpsertTodayOdrResponse(UpsertTodayOrdStatus status, RaiinInfItemResponse validationRaiinInf, List<ValidationOrdInfListItemResponse> validationOdrInfs, ValidationKarteInfResponse validationKarte)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrInfs = validationOdrInfs;
            ValidationKarte = validationKarte;
        }
        public UpsertTodayOrdStatus Status { get; private set; }
        public RaiinInfItemResponse ValidationRaiinInf { get; private set; }
        public List<ValidationOrdInfListItemResponse> ValidationOdrInfs { get; private set; }
        public ValidationKarteInfResponse ValidationKarte { get; private set; }
    }
}
