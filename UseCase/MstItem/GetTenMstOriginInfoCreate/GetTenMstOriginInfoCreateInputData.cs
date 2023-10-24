using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstOriginInfoCreate
{
    public class GetTenMstOriginInfoCreateInputData : IInputData<GetTenMstOriginInfoCreateOutputData>
    {
        public GetTenMstOriginInfoCreateInputData(ItemTypeEnums type, int hpId, int userId, string itemCd)
        {
            Type = type;
            HpId = hpId;
            UserId = userId;
            ItemCd = itemCd;
        }

        public ItemTypeEnums Type { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string ItemCd { get; private set; }
    }
}
