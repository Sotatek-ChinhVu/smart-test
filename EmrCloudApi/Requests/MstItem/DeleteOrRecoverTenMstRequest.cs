using Domain.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class DeleteOrRecoverTenMstRequest
    {
        public DeleteOrRecoverTenMstRequest(string itemCd ,DeleteOrRecoverTenMstMode mode, List<TenMstOriginModelDto> tenMsts)
        {
            ItemCd = itemCd;
            Mode = mode;
            TenMsts = tenMsts;
        }

        public string ItemCd { get; private set; }


        public DeleteOrRecoverTenMstMode Mode { get; private set; }

        /// <summary>
        /// Only pass IsStartDateKeyUpdated = true;
        /// </summary>
        public List<TenMstOriginModelDto> TenMsts { get; private set; }
    }
}
