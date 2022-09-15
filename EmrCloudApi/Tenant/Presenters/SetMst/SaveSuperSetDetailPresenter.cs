using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SuperSetDetail.SaveSuperSetDetail;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class SaveSuperSetDetailPresenter : ISaveSuperSetDetailOutputPort
{
    public Response<SaveSuperSetDetailResponse> Result { get; private set; } = new();

    public void Complete(SaveSuperSetDetailOutputData output)
    {
        Result.Data = new SaveSuperSetDetailResponse(output.Status == SaveSuperSetDetailStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveSuperSetDetailStatus status) => status switch
    {
        SaveSuperSetDetailStatus.Successed => ResponseMessage.Success,
        SaveSuperSetDetailStatus.Failed => ResponseMessage.Failed,
        SaveSuperSetDetailStatus.SaveSetByomeiFailed => ResponseMessage.SaveSetByomeiFailed,
        SaveSuperSetDetailStatus.SaveSetKarteInfFailed => ResponseMessage.SaveSetKarteInfFailed,
        SaveSuperSetDetailStatus.SaveSetOrderInfFailed => ResponseMessage.SaveSetOrderInfFailed,
        _ => string.Empty
    };
}
