using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiMst
{
    public class GetRenkeiMstInputData : IInputData<GetRenkeiMstOutputData>
    {
        public GetRenkeiMstInputData(int hpId, int renkeiId)
        {
            HpId = hpId;
            RenkeiId = renkeiId;
        }

        public int HpId { get; private set; }

        public int RenkeiId { get; private set; }
    }
}
