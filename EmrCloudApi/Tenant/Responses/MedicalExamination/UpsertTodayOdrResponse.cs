using EmrCloudApi.Tenant.Responses.KarteInf;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class UpsertTodayOdrResponse
    {
        public UpsertTodayOdrResponse(UpsertTodayOrdStatus status, RaiinInfItemResponse validationRaiinInf, List<ValidationTodayOrdItemResponse> validationOdrs, ValidationKarteInfResponse validationKarte)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrs = validationOdrs;
            ValidationKarte = validationKarte;
        }
        public UpsertTodayOrdStatus Status { get; private set; }
        public RaiinInfItemResponse ValidationRaiinInf { get; private set; }
        public List<ValidationTodayOrdItemResponse> ValidationOdrs { get; private set; }
        public ValidationKarteInfResponse ValidationKarte { get; private set; }
    }
}
