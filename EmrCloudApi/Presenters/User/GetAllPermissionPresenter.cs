using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using UseCase.User.GetAllPermission;

namespace EmrCloudApi.Presenters.User;

public class GetAllPermissionPresenter : IGetAllPermissionOutputPort
{
    public Response<GetAllPermissionResponse> Result { get; private set; } = new Response<GetAllPermissionResponse>();

    public void Complete(GetAllPermissionOutputData output)
    {
        Result.Data = new GetAllPermissionResponse(output.UserPermissions);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetAllPermissionStatus status) => status switch
    {
        GetAllPermissionStatus.Success => ResponseMessage.Success,
        GetAllPermissionStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetAllPermissionStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        _ => string.Empty
    };
}
