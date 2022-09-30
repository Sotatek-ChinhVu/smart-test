using Domain.Models.PostCodeMst;

namespace EmrCloudApi.Tenant.Responses.PostCodeMst
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
