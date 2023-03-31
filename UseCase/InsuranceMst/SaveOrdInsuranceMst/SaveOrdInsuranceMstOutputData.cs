using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.SaveOrdInsuranceMst
{
    public class SaveOrdInsuranceMstOutputData : IOutputData
    {
        public SaveOrdInsuranceMstStatus Status { get; private set; }

        public SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus status)
        {
            Status = status;
        }
    }
}
