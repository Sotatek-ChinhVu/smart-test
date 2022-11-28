using UseCase.Insurance.ValidateInsurance;

namespace EmrCloudApi.Responses.Insurance
{
    public class ValidateInsuranceResponse
    {
        public ValidateInsuranceResponse(bool resultCheck, string message, ValidateInsuranceListItem itemValidate)
        {
            ResultCheck = resultCheck;
            Message = message;
            ItemValidate = itemValidate;
        }

        public bool ResultCheck { get; private set; }

        public string Message { get; private set; }

        public ValidateInsuranceListItem ItemValidate { get; private set; }
    }
}
