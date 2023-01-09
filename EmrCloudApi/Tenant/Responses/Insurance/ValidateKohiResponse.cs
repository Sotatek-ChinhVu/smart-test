using Domain.Models.Insurance;
using UseCase.Insurance.ValidKohi;

namespace EmrCloudApi.Responses.Insurance
{
    public class ValidateKohiResponse
    {
        public ValidateKohiResponse(bool resultCheck, List<ResultValidateInsurance<ValidKohiStatus>> details)
        {
            ResultCheck = resultCheck;
            Details = details;
        }

        public bool ResultCheck { get; private set; }

        public List<ResultValidateInsurance<ValidKohiStatus>> Details { get; private set; }
    }
}
