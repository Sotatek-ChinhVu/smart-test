using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.GetToolTip
{
    public class GetSetMstToolTipInputData : IInputData<GetSetMstToolTipOutputData>
    {
        public GetSetMstToolTipInputData(int hpId, int setCd)
        {
            HpId = hpId;
            SetCd = setCd;
        }

        public int HpId { get; private set; }
        public int SetCd { get; private set; }
    }
}