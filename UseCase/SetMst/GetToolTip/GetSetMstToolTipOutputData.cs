using Domain.Models.SetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.GetToolTip
{
    public class GetSetMstToolTipOutputData : IOutputData
    {
        public SetMstTooltipModel SetList { get; private set; }
        public GetSetMstToolTipStatus Status { get; private set; }
        public GetSetMstToolTipOutputData(SetMstTooltipModel setList, GetSetMstToolTipStatus status)
        {
            SetList = setList;
            Status = status;
        }
    }
}