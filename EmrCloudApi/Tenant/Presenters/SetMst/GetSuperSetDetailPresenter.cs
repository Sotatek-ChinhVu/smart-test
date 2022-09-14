using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SupperSetDetail.SupperSetDetail;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class GetSuperSetDetailPresenter : IGetSuperSetDetailOutputPort
{
    public Response<GetSuperSetDetailResponse> Result { get; private set; } = new();

    public void Complete(GetSupperSetDetailOutputData output)
    {
        Result.Data = new GetSuperSetDetailResponse(output.SuperSetDetailModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSupperSetDetailListStatus status) => status switch
    {
        GetSupperSetDetailListStatus.Successed => ResponseMessage.Success,
        GetSupperSetDetailListStatus.Failed => ResponseMessage.Failed,
        GetSupperSetDetailListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetSupperSetDetailListStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        _ => string.Empty
    };
}
