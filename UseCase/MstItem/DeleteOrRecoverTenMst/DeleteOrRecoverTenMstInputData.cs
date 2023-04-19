using Domain.Enum;
using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.DeleteOrRecoverTenMst
{
    public class DeleteOrRecoverTenMstInputData : IInputData<DeleteOrRecoverTenMstOutputData>
    {
        public DeleteOrRecoverTenMstInputData(string itemCd, string selectedTenMstModelName, DeleteOrRecoverTenMstMode mode, List<TenMstOriginModel> tenMsts, int userId, int hpId, bool confirmDeleteIfModeIsDeleted)
        {
            ItemCd = itemCd;
            SelectedTenMstModelName = selectedTenMstModelName;
            Mode = mode;
            TenMsts = tenMsts;
            UserId = userId;
            HpId = hpId;
            ConfirmDeleteIfModeIsDeleted = confirmDeleteIfModeIsDeleted;
        }

        public string ItemCd { get; private set; }

        public string SelectedTenMstModelName { get; private set; }

        public DeleteOrRecoverTenMstMode Mode { get; private set; }

        public List<TenMstOriginModel> TenMsts { get; private set; }

        public int UserId { get; private set; }

        public int HpId { get; private set; }

        public bool ConfirmDeleteIfModeIsDeleted { get; private set; }
    }
}
