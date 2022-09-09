using Domain.Models.SuperSetDetail;

namespace EmrCloudApi.Tenant.Responses.SetMst;

public class GetSetByomeiListResponse
{
    public GetSetByomeiListResponse(List<SetByomeiModel> data)
    {
        Data = data;
    }

    public List<SetByomeiModel> Data { get; private set; }
}
