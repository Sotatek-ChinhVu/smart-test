using UseCase.SetMst.GetList;

namespace EmrCloudApi.Tenant.Responses.SetMst
{
    public class GetSetMstListResponse
    {
        public GetSetMstListResponse(List<GetSetMstListOutputItem>? data)
        {
            Data = data;
        }

        public List<GetSetMstListOutputItem>? Data { get; private set; }
    }
}