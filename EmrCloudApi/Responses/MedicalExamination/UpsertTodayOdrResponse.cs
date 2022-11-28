using EmrCloudApi.Responses.KarteInf;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class UpsertTodayOdrResponse
    {
        public UpsertTodayOdrResponse(UpsertTodayOrdStatus status, RaiinInfItemResponse validationRaiinInf, List<ValidationTodayOrdItemResponse> validationOdrInfs, ValidationKarteInfResponse validationKarte)
        {
            Status = status;
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrInfs = validationOdrInfs;
            ValidationKarte = validationKarte;
        }
        public UpsertTodayOrdStatus Status { get; private set; }
        public RaiinInfItemResponse ValidationRaiinInf { get; private set; }
        public List<ValidationTodayOrdItemResponse> ValidationOdrInfs { get; private set; }
        public ValidationKarteInfResponse ValidationKarte { get; private set; }
    }
}
