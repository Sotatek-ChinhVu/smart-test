using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdmin.Responses.Admin;
using UseCase.SuperAdmin.Login;

namespace SuperAdmin.Presenters.Admin;

public class LoginPresenter
{
    public Response<LoginResponse> Result { get; private set; } = new();

    public void Complete(LoginOutputData output)
    {
        Result.Data = new LoginResponse(output.Status == LoginStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(LoginStatus status) => status switch
    {
        LoginStatus.Successed => ResponseMessage.Success,
        LoginStatus.Failed => ResponseMessage.Fail,
        LoginStatus.InvalidLoginId => ResponseMessage.InvalidLoginId,
        LoginStatus.InvalidPassWord => ResponseMessage.InvalidPassword,
        _ => string.Empty
    };
}
