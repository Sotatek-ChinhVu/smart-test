using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetJihiSbtMstList
{
    public class GetJihiSbtMstListInputData : IInputData<GetJihiSbtMstListOutputData>
    {
        public GetJihiSbtMstListInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
