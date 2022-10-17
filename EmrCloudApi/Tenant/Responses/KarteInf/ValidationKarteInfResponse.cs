using Helper.Constants;

namespace EmrCloudApi.Tenant.Responses.KarteInf
{
    public class ValidationKarteInfResponse
    {
        public ValidationKarteInfResponse(KarteConst.KarteValidationStatus status, string validationMessage)
        {
            Status = status;
            ValidationMessage = validationMessage;
        }

        public KarteConst.KarteValidationStatus Status { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
