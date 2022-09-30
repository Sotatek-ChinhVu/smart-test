using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchPostCodeRespone
    {
        public SearchPostCodeRespone(List<PostCodeMstModel> postCodeMstModels)
        {
            PostCodeMstModels = postCodeMstModels;
        }

        public List<PostCodeMstModel> PostCodeMstModels { get; private set; }
    }
}
