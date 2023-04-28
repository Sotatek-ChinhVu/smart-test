using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveSetDataTenMst
{
    public class SaveSetDataTenMstInputData : IInputData<SaveSetDataTenMstOutputData>
    {
        public SaveSetDataTenMstInputData(int hpId, int userId, string itemCd, List<TenMstOriginModel> tenOrigins, SetDataTenMstOriginModel setData)
        {
            HpId = hpId;
            UserId = userId;
            ItemCd = itemCd;
            TenOrigins = tenOrigins;
            SetData = setData;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string ItemCd { get; private set; }

        public List<TenMstOriginModel> TenOrigins { get; private set; }

        public SetDataTenMstOriginModel SetData { get; private set; }
    }
}
