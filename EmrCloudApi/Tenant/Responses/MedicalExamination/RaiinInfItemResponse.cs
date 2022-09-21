using Helper.Constants;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class RaiinInfItemResponse
    {
        public RaiinInfItemResponse(RaiinInfConst.RaiinInfValidationStatus status, string validationMessage)
        {
            Status = status;
            ValidationMessage = validationMessage;
        }

        public RaiinInfConst.RaiinInfValidationStatus Status { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
