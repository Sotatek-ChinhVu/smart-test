using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SuperSetDetail.SuperSetDetail;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class GetSuperSetDetailPresenter : IGetSuperSetOutputPort
{
    public Response<GetSuperSetDetailResponse> Result { get; private set; } = new();

    public void Complete(GetSuperSetDetailOutputData output)
    {
        Result.Data = new GetSuperSetDetailResponse(output.SuperSetDetailModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSuperSetDetailListStatus status) => status switch
    {
        GetSuperSetDetailListStatus.Successed => ResponseMessage.Success,
        GetSuperSetDetailListStatus.Failed => ResponseMessage.Failed,
        GetSuperSetDetailListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetSuperSetDetailListStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        _ => string.Empty
    };
}
