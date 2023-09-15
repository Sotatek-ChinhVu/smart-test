using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfHRTItem
{
    public class GetTenOfHRTItemInputData : IInputData<GetTenOfHRTItemOutputData>
    {
        public GetTenOfHRTItemInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
