using UseCase.SetMst.GetList;

namespace EmrCloudApi.Responses.SetMst
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