using EmrCloudApi.Responses.KarteInf;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ValidationTodayOrdResponse
    {
        public ValidationTodayOrdResponse(RaiinInfItemResponse validationRaiinInf, List<ValidationTodayOrdItemResponse> validationOdrInfs, ValidationKarteInfResponse validationKarte)
        {
            ValidationRaiinInf = validationRaiinInf;
            ValidationOdrInfs = validationOdrInfs;
            ValidationKarte = validationKarte;
        }

        public RaiinInfItemResponse ValidationRaiinInf { get; private set; }
        public List<ValidationTodayOrdItemResponse> ValidationOdrInfs { get; private set; }
        public ValidationKarteInfResponse ValidationKarte { get; private set; }
    }
}
