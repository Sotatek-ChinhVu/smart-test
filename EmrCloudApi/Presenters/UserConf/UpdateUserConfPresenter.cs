using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.User.UpdateUserConf;

namespace EmrCloudApi.Presenters.UserConf;

public class UpdateUserConfPresenter : IUpdateUserConfOutputPort
{
    public Response<UpdateUserConfResponse> Result { get; private set; } = new Response<UpdateUserConfResponse>();

    public void Complete(UpdateUserConfOutputData output)
    {
        Result.Data = new UpdateUserConfResponse(output.Status == UpdateUserConfStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateUserConfStatus status) => status switch
    {
        UpdateUserConfStatus.Successed => ResponseMessage.Success,
        UpdateUserConfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        UpdateUserConfStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        UpdateUserConfStatus.InvalidGrpCd => ResponseMessage.InvalidGrpCd,
        UpdateUserConfStatus.InvalidValue => ResponseMessage.InvalidValue,
        UpdateUserConfStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
