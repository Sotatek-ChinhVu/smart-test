using UseCase.Core.Sync.Core;

namespace UseCase.HokenMst.GetHokenMst
{
    public class GetHokenMstInputData : IInputData<GetHokenMstOutputData>
    {
        public GetHokenMstInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
