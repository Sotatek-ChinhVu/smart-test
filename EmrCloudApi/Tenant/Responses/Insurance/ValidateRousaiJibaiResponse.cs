namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class ValidateRousaiJibaiResponse
    {
        public ValidateRousaiJibaiResponse(bool resultCheck, string message)
        {
            ResultCheck = resultCheck;
            Message = message;
        }

        public bool ResultCheck { get; private set; }
        public string Message { get; private set; }
    }
}
