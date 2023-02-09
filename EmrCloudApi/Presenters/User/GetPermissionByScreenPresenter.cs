using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using UseCase.User.GetPermissionByScreenCode;

namespace EmrCloudApi.Presenters.User;

public class GetPermissionByScreenPresenter : IGetPermissionByScreenOutputPort
{
    public Response<GetPermissionByScreenResponse> Result { get; private set; } = new Response<GetPermissionByScreenResponse>();

    public void Complete(GetPermissionByScreenOutputData output)
    {
        Result.Data = new GetPermissionByScreenResponse(output.PermissionType);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetPermissionByScreenStatus status) => status switch
    {
        GetPermissionByScreenStatus.Successed => ResponseMessage.Success,
        GetPermissionByScreenStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetPermissionByScreenStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        _ => string.Empty
    };
}
