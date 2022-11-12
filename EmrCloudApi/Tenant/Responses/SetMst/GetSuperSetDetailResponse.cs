using UseCase.SuperSetDetail.GetSuperSetDetail;

namespace EmrCloudApi.Tenant.Responses.SetMst;

public class GetSuperSetDetailResponse
{
    public GetSuperSetDetailResponse(SuperSetDetailItem data)
    {
        Data = data;
    }

    public SuperSetDetailItem Data { get; private set; }
}
