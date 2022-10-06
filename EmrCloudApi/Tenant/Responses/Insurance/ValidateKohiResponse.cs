namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class ValidateKohiResponse
    {
        public ValidateKohiResponse(bool resultCheck, string message)
        {
            ResultCheck = resultCheck;
            Message = message;
        }

        public bool ResultCheck { get; private set; }
        public string Message { get; private set; }
    }
}
