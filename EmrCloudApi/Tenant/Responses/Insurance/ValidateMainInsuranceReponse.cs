using Domain.Models.Insurance;
using UseCase.Insurance.ValidMainInsurance;

namespace EmrCloudApi.Responses.Insurance
{
    public class ValidateMainInsuranceReponse
    {
        public ValidateMainInsuranceReponse(bool resultCheck, List<ResultValidateInsurance<ValidMainInsuranceStatus>> details)
        {
            ResultCheck = resultCheck;
            Details = details;
        }

        public bool ResultCheck { get; private set; }

        public List<ResultValidateInsurance<ValidMainInsuranceStatus>> Details { get; private set; }
    }
}
