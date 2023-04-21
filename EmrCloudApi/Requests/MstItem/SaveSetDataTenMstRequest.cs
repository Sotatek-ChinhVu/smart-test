using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem
{
    public class SaveSetDataTenMstRequest
    {
        public SaveSetDataTenMstRequest(string itemCd ,List<TenMstOriginModelDto> tenOrigins, SetDataTenMstOriginModel setData)
        {
            ItemCd = itemCd;
            TenOrigins = tenOrigins;
            SetData = setData;
        }
        public string ItemCd { get; private set; }

        public List<TenMstOriginModelDto> TenOrigins { get; private set; }

        public SetDataTenMstOriginModel SetData { get; private set; }
    }
}
