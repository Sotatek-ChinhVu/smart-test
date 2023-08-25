using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class GetTenMstOriginInfoCreateRequest
    {
        public ItemTypeEnums Type { get; set; }
        public string ItemCd { get; set; } = string.Empty;
    }
}
