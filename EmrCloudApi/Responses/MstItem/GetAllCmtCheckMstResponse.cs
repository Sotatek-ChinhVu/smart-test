using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetAllCmtCheckMstResponse
    {
        public GetAllCmtCheckMstResponse(List<CommentCheckMstModel> itemCmtModels)
        {
            ItemCmtModels = itemCmtModels;
        }

        public List<CommentCheckMstModel> ItemCmtModels { get; private set; }
    }
}
