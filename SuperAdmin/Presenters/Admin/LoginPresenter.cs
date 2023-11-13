using Helper.Constants;
using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdmin.Responses.Admin;
using SuperAdminAPI.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UseCase.SuperAdmin.Login;

namespace SuperAdmin.Presenters.Admin;

public class LoginPresenter
{
    public Response<LoginResponse> Result { get; private set; } = new();

    public void Complete(LoginOutputData output)
    {
        if (output.Status == LoginStatus.Successed)
        {
            var claims = new Claim[]
                {
            new(ParamConstant.UserId, output.User.Id.ToString()),
            new(ParamConstant.Role, output.User.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var token = AuthProvider.GenerateAccessToken(claims);
            Result.Data = new LoginResponse(token, output.User.Id);
        }
        else
        {
            Result.Data = new LoginResponse(string.Empty, 0);
        }

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
