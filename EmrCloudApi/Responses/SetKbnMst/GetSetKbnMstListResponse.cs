using UseCase.SetKbnMst.GetList;

namespace EmrCloudApi.Responses.SetKbnMst
{
    public class GetSetKbnMstListResponse
    {
        public GetSetKbnMstListResponse(List<GetSetKbnMstListOutputItem>? data)
        {
            Data = data;
        }

        public List<GetSetKbnMstListOutputItem>? Data { get; private set; }
    }
}