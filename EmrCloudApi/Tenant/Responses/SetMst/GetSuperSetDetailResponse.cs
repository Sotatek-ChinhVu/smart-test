using Domain.Models.SuperSetDetail;

namespace EmrCloudApi.Tenant.Responses.SetMst;

public class GetSuperSetDetailResponse
{
    public GetSuperSetDetailResponse(SuperSetDetailModel data)
    {
        Data = data;
    }

    public SuperSetDetailModel Data { get; private set; }
}
