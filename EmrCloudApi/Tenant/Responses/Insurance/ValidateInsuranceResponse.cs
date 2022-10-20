namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class ValidateInsuranceResponse
    {
        public ValidateInsuranceResponse(bool resultCheck, string message, int indexItemError)
        {
            ResultCheck = resultCheck;
            Message = message;
            IndexItemError = indexItemError;
        }

        public bool ResultCheck { get; private set; }

        public string Message { get; private set; }

        public int IndexItemError { get; private set; }
    }
}
