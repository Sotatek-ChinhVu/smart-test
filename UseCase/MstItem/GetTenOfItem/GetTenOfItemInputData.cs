using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfItem
{
    public class GetTenOfItemInputData : IInputData<GetTenOfItemOutputData>
    {
        public GetTenOfItemInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
