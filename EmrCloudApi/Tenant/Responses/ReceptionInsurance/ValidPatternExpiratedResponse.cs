namespace EmrCloudApi.Tenant.Responses.ReceptionInsurance
{
    public class ValidPatternExpiratedResponse
    {
        public ValidPatternExpiratedResponse(bool resultCheck, string message)
        {
            ResultCheck = resultCheck;
            Message = message;
        }

        public bool ResultCheck { get; private set; }
        public string Message { get; private set; }
    }
}
