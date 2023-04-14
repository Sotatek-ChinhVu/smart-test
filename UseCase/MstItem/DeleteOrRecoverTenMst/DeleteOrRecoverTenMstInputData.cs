using Domain.Enum;
using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.DeleteOrRecoverTenMst
{
    public class DeleteOrRecoverTenMstInputData : IInputData<DeleteOrRecoverTenMstOutputData>
    {
        public DeleteOrRecoverTenMstInputData(string itemCd , DeleteOrRecoverTenMstMode mode, List<TenMstOriginModel> tenMsts, int userId)
        {
            ItemCd = itemCd;
            Mode = mode;
            TenMsts = tenMsts;
            UserId = userId;
        }
        public string ItemCd { get; private set; }

        public DeleteOrRecoverTenMstMode Mode { get; private set; }

        public List<TenMstOriginModel> TenMsts { get; private set; }

        public int UserId { get; private set; }
    }
}
