using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;
using UseCase.Insurance.ValidMainInsurance;

namespace UseCase.Insurance.ValidHokenInfAllType
{
    public class ValidHokenInfAllTypeOutputData : IOutputData
    {
        public bool Result { get => !ValidateDetails.Any(); }

        public List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> ValidateDetails { get; private set; } = new List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>>();

        public ValidHokenInfAllTypeOutputData(List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> details)
        {
            ValidateDetails = details;
        }
    }
}
