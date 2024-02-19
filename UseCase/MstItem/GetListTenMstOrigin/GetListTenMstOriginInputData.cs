using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListTenMstOrigin
{
    public class GetListTenMstOriginInputData : IInputData<GetListTenMstOriginOutputData>
    {
        public GetListTenMstOriginInputData(int hpId, string itemCd)
        {
            HpId = hpId;
            ItemCd = itemCd;
        }

        public int HpId { get; private set; }
        public string ItemCd { get; private set; }
    }
}
