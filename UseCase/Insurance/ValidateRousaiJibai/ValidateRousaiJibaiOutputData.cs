using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidateRousaiJibai
{
    public class ValidateRousaiJibaiOutputData : IOutputData
    {
        public bool Result { get => !ValidateDetails.Any(); }

        public List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> ValidateDetails { get; private set; }

        public ValidateRousaiJibaiOutputData(List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> details)
        {
            ValidateDetails = details;
        }
    }
}