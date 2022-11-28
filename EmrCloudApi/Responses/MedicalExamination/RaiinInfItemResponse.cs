using Helper.Constants;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class RaiinInfItemResponse
    {
        public RaiinInfItemResponse(RaiinInfConst.RaiinInfTodayOdrValidationStatus status, string validationMessage)
        {
            Status = status;
            ValidationMessage = validationMessage;
        }

        public RaiinInfConst.RaiinInfTodayOdrValidationStatus Status { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
