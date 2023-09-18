using Domain.Models.ListSetMst;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetTreeListSetMstResponse
    {
        public GetTreeListSetMstResponse(List<ListSetMstModel> data)
        {
            Data = data;
        }

        public List<ListSetMstModel> Data { get; private set; } = new List<ListSetMstModel>();
    }
}
