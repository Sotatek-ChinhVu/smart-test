using Castle.Core.Internal;
using DocumentFormat.OpenXml.VariantTypes;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Auth;
using EmrCloudApi.Requests.UserToken;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Auth;
using EmrCloudApi.Responses.UserToken;
using EmrCloudApi.Security;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UseCase.Core.Sync;
using UseCase.User.GetByLoginId;
using UseCase.UserToken.GetInfoRefresh;
using UseCase.UserToken.SiginRefresh;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public AuthController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpPost("ExchangeToken"), Produces("application/json")]
    public ActionResult<Response<ExchangeTokenResponse>> ExchangeToken([FromBody] ExchangeTokenRequest req)
    {
        var getUserInput = new GetUserByLoginIdInputData(req.LoginId);
        var getUserOutput = _bus.Handle(getUserInput);
        var user = getUserOutput.User;
        if (user is null)
        {
            var errorResult = GetErrorResult("The loginId is invalid.");
            return BadRequest(errorResult);
        }

        if (req.Password != user.LoginPass)
        {
            var errorResult = GetErrorResult("The password is invalid.");
            return BadRequest(errorResult);
        }

        // The claims that will be persisted in the tokens.
        var claims = new Claim[]
        {
            new(LoginUserConstant.UserId, user.UserId.ToString()),
            new(LoginUserConstant.HpId, user.HpId.ToString()),
            new(LoginUserConstant.DepartmentId, user.KaId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        string token = AuthProvider.GenerateAccessToken(claims);
        var resultRefreshToken = SigInRefreshToken(user.UserId);

        if(!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(resultRefreshToken.refreshToken))
        {
            var successResult = GetSuccessResult(token, user.UserId, user.Name, user.KanaName, user.KaId, user.JobCd == 1, user.ManagerKbn, user.Sname, user.HpId, resultRefreshToken.refreshToken, resultRefreshToken.refreshTokenExpiryTime);
            return Ok(successResult);
        }
        else
        {
            var errorResult = GetErrorResult("An error occurred while verification token");
            return BadRequest(errorResult);
        }

        #region Helper methods
        Response<ExchangeTokenResponse> GetErrorResult(string errorMessage)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(string.Empty, 0, string.Empty, string.Empty, 0, false, 0, string.Empty, 0, string.Empty, DateTime.MinValue),
                Status = 0,
                Message = errorMessage
            };
        }

        Response<ExchangeTokenResponse> GetSuccessResult(string token, int userId, string name, string kanaName, int kaId, bool isDoctor, int managerKbn, string sName, int hpId, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(token, userId, name, kanaName, kaId, isDoctor, managerKbn, sName, hpId, refreshToken, refreshTokenExpiryTime),
                Status = 1,
                Message = ResponseMessage.Success
            };
        }
        #endregion
    }

    [HttpPost("RefreshToken")]
    public ActionResult<Response<RefreshTokenResponse>> RefreshAccessToken([FromBody] RefreshTokenRequest request)
    {
        ClaimsPrincipal? principal = AuthProvider.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null)
            return BadRequest("Invalid access token");

        int.TryParse(principal.FindFirstValue(LoginUserConstant.UserId), out int userId);
        var input = new RefreshTokenByUserInputData(userId, request.RefreshToken, AuthProvider.GeneratorRefreshToken());
        //var input = new RefreshTokenByUserInputData(userId, request.RefreshToken, AuthProvider.GeneratorRefreshToken(), DateTime.UtcNow.AddMinutes(3));
        var output = _bus.Handle(input);
        if(output.Status == RefreshTokenByUserStatus.Successful)
        {
            string newToken = AuthProvider.GenerateAccessToken(new Claim[]
            {
                new(LoginUserConstant.UserId, userId.ToString()),
                new(LoginUserConstant.HpId, principal.FindFirstValue(LoginUserConstant.HpId)),
                new(LoginUserConstant.DepartmentId, principal.FindFirstValue(LoginUserConstant.DepartmentId)),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });

            return new Response<RefreshTokenResponse>
            {
                Data = new RefreshTokenResponse(newToken, output.UserToken.RefreshToken, output.UserToken.RefreshTokenExpiryTime),
                Status = (int)output.Status
            };
        }
        else
        {
            return new Response<RefreshTokenResponse>
            {
                Data = new RefreshTokenResponse(string.Empty, string.Empty, DateTime.MinValue),
                Status = (int)output.Status,
                Message = "Invalid refresh token"
            };
        }
    }

    private (string refreshToken, DateTime refreshTokenExpiryTime) SigInRefreshToken(int userId)
    {
        string refreshToken = AuthProvider.GeneratorRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddHours(AuthProvider.GetHoursRefreshTokenExpiryTime());
        var input = new SigninRefreshTokenInputData(userId, refreshToken, refreshTokenExpiryTime);
        var output = _bus.Handle(input);
        if (output.Status == SigninRefreshTokenStatus.Successful)
            return new (refreshToken, refreshTokenExpiryTime);
        else
            return new(string.Empty, DateTime.MinValue);
    }
}
