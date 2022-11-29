using EmrCloudApi.Responses.MedicalExamination;

namespace EmrCloudApi.Responses.OrdInfs
{
    public class ValidationInputItemOrdInfListResponse
    {
        public ValidationInputItemOrdInfListResponse(List<ValidationTodayOrdItemResponse> validations)
        {
            ValidationOdrInfs = validations;
        }

        public List<ValidationTodayOrdItemResponse> ValidationOdrInfs { get; private set; }
    }
}
