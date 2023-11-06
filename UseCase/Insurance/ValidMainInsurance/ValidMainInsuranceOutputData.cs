using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidMainInsurance
{
    public class ValidMainInsuranceOutputData : IOutputData
    {
        public bool Result { get => !ValidateDetails.Any(); }

        public List<ResultValidateInsurance<ValidMainInsuranceStatus>> ValidateDetails { get; private set; } = new List<ResultValidateInsurance<ValidMainInsuranceStatus>>();

        public ValidMainInsuranceOutputData(List<ResultValidateInsurance<ValidMainInsuranceStatus>> details)
        {
            ValidateDetails = details;
        }
    }
}
