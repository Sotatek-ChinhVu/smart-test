using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidateOneKohi
{
    public class ValidOneKohiOutputData : IOutputData
    {
        public bool Result { get => !ValidateDetails.Any(); }

        public List<ResultValidateInsurance<ValidOneKohiStatus>> ValidateDetails { get; private set; } = new List<ResultValidateInsurance<ValidOneKohiStatus>>();

        public ValidOneKohiOutputData(List<ResultValidateInsurance<ValidOneKohiStatus>> details)
        { 
            ValidateDetails = details;
        }
    }
}