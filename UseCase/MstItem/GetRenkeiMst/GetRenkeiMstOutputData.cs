using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiMst
{
    public class GetRenkeiMstOutputData : IOutputData
    {
        public GetRenkeiMstOutputData(GetRenkeiMstStatus status, RenkeiMstModel renkeiMst)
        {
            Status = status;
            RenkeiMst = renkeiMst;
        }

        public GetRenkeiMstStatus Status { get; private set; }

        public RenkeiMstModel RenkeiMst { get; private set; }
    }
}
