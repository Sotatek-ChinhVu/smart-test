using Domain.Models.ListSetMst;

namespace EmrCloudApi.Responses.ListSetMst
{
    public class GetTreeListSetMstResponse
    {
        public List<ListSetMstModel> Data { get; set; } = new List<ListSetMstModel>();
    }
}
