using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfKNItem
{
    public class GetTenOfKNItemInputData : IInputData<GetTenOfKNItemOutputData>
    {
        public GetTenOfKNItemInputData(int hpId, string itemCd)
        {
            HpId = hpId;
            ItemCd = itemCd;
        }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }
    }
}
