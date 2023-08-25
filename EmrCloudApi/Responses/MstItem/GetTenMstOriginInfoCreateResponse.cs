using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenMstOriginInfoCreateResponse
    {
        public GetTenMstOriginInfoCreateResponse(string itemCd, int jihiSbt, TenMstOriginModel tenMstOriginModel)
        {
            ItemCd = itemCd;
            JihiSbt = jihiSbt;
            TenMstOriginModel = tenMstOriginModel;
        }

        public string ItemCd { get; private set; }

        public int JihiSbt { get; private set; }

        public TenMstOriginModel TenMstOriginModel { get; private set; }
    }
}
