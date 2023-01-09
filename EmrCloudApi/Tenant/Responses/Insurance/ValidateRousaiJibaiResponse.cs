using Domain.Models.Insurance;
using UseCase.Insurance.ValidateRousaiJibai;

namespace EmrCloudApi.Responses.Insurance
{
    public class ValidateRousaiJibaiResponse
    {
        public ValidateRousaiJibaiResponse(bool resultCheck, List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> details)
        {
            ResultCheck = resultCheck;
            Details = details;
        }

        public bool ResultCheck { get; private set; }

        public List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> Details { get; private set; }
    }
}