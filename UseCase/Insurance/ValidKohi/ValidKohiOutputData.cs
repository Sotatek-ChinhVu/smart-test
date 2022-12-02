using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidKohi
{
    public class ValidKohiOutputData : IOutputData
    {
        public bool Result { get => !ValidateDetails.Any(); }

        public List<ResultValidateInsurance<ValidKohiStatus>> ValidateDetails { get; private set; } = new List<ResultValidateInsurance<ValidKohiStatus>>();

        public ValidKohiOutputData(List<ResultValidateInsurance<ValidKohiStatus>> details)
        {
            ValidateDetails = details;
        }
    }
}