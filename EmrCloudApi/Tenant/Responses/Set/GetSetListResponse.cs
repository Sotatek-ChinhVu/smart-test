using UseCase.Set.GetList;

namespace EmrCloudApi.Tenant.Responses.Set
{
    public class GetSetListResponse
    {
        public GetSetListResponse(List<GetSetListOutputItem>? data)
        {
            Data = data;
        }

        public List<GetSetListOutputItem>? Data { get; set; }
    }
}