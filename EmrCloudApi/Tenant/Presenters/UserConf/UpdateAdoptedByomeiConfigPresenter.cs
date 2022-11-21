using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UserConf;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;

namespace EmrCloudApi.Tenant.Presenters.UserConf;

public class UpdateAdoptedByomeiConfigPresenter : IUpdateAdoptedByomeiConfigOutputPort
{
    public Response<UpdateAdoptedByomeiConfigResponse> Result { get; private set; } = new Response<UpdateAdoptedByomeiConfigResponse>();

    public void Complete(UpdateAdoptedByomeiConfigOutputData output)
    {
        Result.Data = new UpdateAdoptedByomeiConfigResponse(output.Status == UpdateAdoptedByomeiConfigStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateAdoptedByomeiConfigStatus status) => status switch
    {
        UpdateAdoptedByomeiConfigStatus.Successed => ResponseMessage.Success,
        UpdateAdoptedByomeiConfigStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        UpdateAdoptedByomeiConfigStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        UpdateAdoptedByomeiConfigStatus.InvalidAdoptedValue => ResponseMessage.InvalidAdoptedValue,
        UpdateAdoptedByomeiConfigStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
