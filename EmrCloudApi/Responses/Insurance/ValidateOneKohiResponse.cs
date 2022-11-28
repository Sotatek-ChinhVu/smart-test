using Domain.Models.Insurance;
using UseCase.Insurance.ValidateOneKohi;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class ValidateOneKohiResponse
    {
        public ValidateOneKohiResponse(bool resultCheck, List<ResultValidateInsurance<ValidOneKohiStatus>> details)
        {
            ResultCheck = resultCheck;
            Details = details;
        }

        public bool ResultCheck { get; private set; }

        public List<ResultValidateInsurance<ValidOneKohiStatus>> Details { get; private set; }
    }
}
