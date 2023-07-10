using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.CheckNextOrdHaveOdr
{
    public class CheckNextOrdHaveOdrOutputData : IOutputData
    {
        public CheckNextOrdHaveOdrOutputData(CheckNextOrdHaveOdrStatus status, bool result)
        {
            Status = status;
            Result = result;
        }

        public CheckNextOrdHaveOdrStatus Status { get; private set; }

        public bool Result { get; private set; }
    }
}
