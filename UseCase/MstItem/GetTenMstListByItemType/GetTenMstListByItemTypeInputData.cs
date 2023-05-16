using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstListByItemType
{
    public class GetTenMstListByItemTypeInputData : IInputData<GetTenMstListByItemTypeOutputData>
    {
        public GetTenMstListByItemTypeInputData(int hpId, ItemTypeEnums itemType, int sinDate)
        {
            HpId = hpId;
            ItemType = itemType;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public ItemTypeEnums ItemType { get; private set; }

        public int SinDate { get; private set; }
    }
}
