namespace EmrCloudApi.Responses.Insurance
{
    public class ValidateListInsuranceResponse
    {
        public ValidateListInsuranceResponse(List<ValidateInsuranceResponse> listResult)
        {
            ListResult = listResult;
        }

        public List<ValidateInsuranceResponse> ListResult { get; private set; }
    }
}
