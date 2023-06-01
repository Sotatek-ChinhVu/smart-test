using UseCase.Core.Sync.Core;
using static Helper.Constants.KarteConst;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RsvkrtByomeiConst;

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
