using Domain.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class DeleteOrRecoverTenMstRequest
    {
        public DeleteOrRecoverTenMstRequest(string itemCd, string selectedTenMstModelName, DeleteOrRecoverTenMstMode mode, List<TenMstOriginModelDto> tenMsts, bool confirmDeleteIfModeIsDeleted)
        {
            ItemCd = itemCd;
            SelectedTenMstModelName = selectedTenMstModelName;
            Mode = mode;
            TenMsts = tenMsts;
            ConfirmDeleteIfModeIsDeleted = confirmDeleteIfModeIsDeleted;
        }

        public string ItemCd { get; private set; }

        public string SelectedTenMstModelName { get; private set; }

        public DeleteOrRecoverTenMstMode Mode { get; private set; }

        /// <summary>
        /// Only pass IsStartDateKeyUpdated = true;
        /// </summary>
        public List<TenMstOriginModelDto> TenMsts { get; private set; }

        public bool ConfirmDeleteIfModeIsDeleted { get; private set; }
    }
}
