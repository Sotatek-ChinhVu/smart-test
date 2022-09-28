using Domain.Models.PostCodeMst;

namespace EmrCloudApi.Tenant.Responses.PostCodeMst
{
    public class SearchPostCodeRespone
    {
        public SearchPostCodeRespone(List<PostCodeMstModel> postCodeMstModels)
        {
            this.postCodeMstModels = postCodeMstModels;
        }

        public List<PostCodeMstModel> postCodeMstModels { get; private set; }
    }
}
