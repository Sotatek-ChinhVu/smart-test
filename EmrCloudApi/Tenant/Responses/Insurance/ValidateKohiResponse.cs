namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class ValidateKohiResponse
    {
        public ValidateKohiResponse(bool resultCheck, string message, int typeMessage)
        {
            ResultCheck = resultCheck;
            Message = message;
            TypeMessage = typeMessage;
        }

        public bool ResultCheck { get; private set; }
        public string Message { get; private set; }
        public int TypeMessage { get; private set; }
    }
}
