using UseCase.Core.Sync.Core;
using UseCase.HokenMst.GetDetail;

namespace UseCase.HokenMst.GetHokenMst
{
    public class GetHokenMstInputData : IInputData<GetHokenMstOutputData>
    {
        public GetHokenMstInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; set; }
    }
}
