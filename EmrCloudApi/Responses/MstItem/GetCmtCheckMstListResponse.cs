using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetCmtCheckMstListResponse
    {
        public GetCmtCheckMstListResponse(List<ItemCmtModel> itemCmtModels)
        {
            ItemCmtModels = itemCmtModels;
        }

        public List<ItemCmtModel> ItemCmtModels { get; private set; }
    }
}
