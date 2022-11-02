using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchPostCodeRespone
    {
        public SearchPostCodeRespone(int totalCount, List<PostCodeMstModel> postCodeMstModels)
        {
            TotalCount = totalCount;
            PostCodeMstModels = postCodeMstModels;
        }

        public int TotalCount { get; private set; }
        public List<PostCodeMstModel> PostCodeMstModels { get; private set; }
    }
}
