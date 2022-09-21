using EmrCloudApi.Tenant.Responses.OrdInf;
using UseCase.MedicalExamination.UpsertTodayOrd;
using static Helper.Constants.RaiinInfConst;
using static Helper.Constants.TodayKarteConst;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class UpsertTodayOdrResponse
    {
        public UpsertTodayOdrResponse(UpsertTodayOrdStatus status, RaiinInfValidationStatus validationRaiinInf, List<ValidationOrdInfListItemResponse> validationOdrs, Dictionary<int, TodayKarteValidationStatus> validationKartes)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrs = validationOdrs;
            ValidationKartes = validationKartes;
        }

        public UpsertTodayOrdStatus Status { get; private set; }

        public RaiinInfValidationStatus ValidationRaiinInf { get; private set; }
        public List<ValidationOrdInfListItemResponse> ValidationOdrs { get; private set; }
        public Dictionary<int, TodayKarteValidationStatus> ValidationKartes { get; private set; }
    }
}
