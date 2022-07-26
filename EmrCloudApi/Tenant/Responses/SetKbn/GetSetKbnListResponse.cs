using UseCase.SetKbn.GetList;

namespace EmrCloudApi.Tenant.Responses.SetKbn
{
    public class GetSetKbnListResponse
    {
        public GetSetKbnListResponse(List<GetSetKbnListOutputItem>? data)
        {
            Data = data;
        }

        public List<GetSetKbnListOutputItem>? Data { get; set; }
    }
}