using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstOriginInfoCreate
{
    public class GetTenMstOriginInfoCreateInputData : IInputData<GetTenMstOriginInfoCreateOutputData>
    {
        public GetTenMstOriginInfoCreateInputData(ItemTypeEnums type, int hpId)
        {
            Type = type;
            HpId = hpId;
        }

        public ItemTypeEnums Type { get; private set; }

        public int HpId { get; private set; }
    }
}
