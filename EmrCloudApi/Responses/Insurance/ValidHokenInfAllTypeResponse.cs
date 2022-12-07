using Domain.Models.Insurance;
using UseCase.Insurance.ValidHokenInfAllType;

namespace EmrCloudApi.Responses.Insurance
{
    public class ValidHokenInfAllTypeResponse
    {
        public ValidHokenInfAllTypeResponse(bool resultCheck, List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> details)
        {
            ResultCheck = resultCheck;
            Details = details;
        }

        public bool ResultCheck { get; private set; }

        public List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> Details { get; private set; }
    }
}
