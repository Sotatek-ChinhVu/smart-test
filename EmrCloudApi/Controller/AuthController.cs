using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Auth;
using EmrCloudApi.Requests.UserToken;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Auth;
using EmrCloudApi.Responses.UserToken;
using EmrCloudApi.Security;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IHttpContextAccessor httpContextAccessor, UseCaseBus bus)
    {
        _bus = bus;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("ExchangeToken"), Produces("application/json")]
    public ActionResult<Response<ExchangeTokenResponse>> ExchangeToken([FromBody] ExchangeTokenRequest req)
    {
        var getUserInput = new GetUserByLoginIdInputData(req.LoginId, req.Password);
        var getUserOutput = _bus.Handle(getUserInput);
        var user = getUserOutput.User;
        if (user is null)
        {
            var errorResult = GetErrorResult("The loginId is invalid.");
            return BadRequest(errorResult);
        }

        //if (req.Password != user.LoginPass)
        //{
        //    var errorResult = GetErrorResult("The password is invalid.");
        //    return BadRequest(errorResult);
        //}

        // The claims that will be persisted in the tokens.
        var claims = new Claim[]
        {
            new(ParamConstant.UserId, user.UserId.ToString()),
            new(ParamConstant.HpId, user.HpId.ToString()),
            new(ParamConstant.DepartmentId, user.KaId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        string token = AuthProvider.GenerateAccessToken(claims);
        var resultRefreshToken = SigInRefreshToken(user.UserId);

        if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(resultRefreshToken.refreshToken))
        {
            // set cookie
            SetCookie(token);

            var successResult = GetSuccessResult(token, user.UserId, user.LoginId, user.Name, user.KanaName, user.KaId, user.JobCd == 1, user.ManagerKbn, user.Sname, user.HpId, resultRefreshToken.refreshToken, resultRefreshToken.refreshTokenExpiryTime);
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
                Data = new ExchangeTokenResponse(string.Empty, 0, string.Empty, string.Empty, string.Empty, 0, false, 0, string.Empty, 0, string.Empty, DateTime.MinValue),
                Status = 0,
                Message = errorMessage
            };
        }

        Response<ExchangeTokenResponse> GetSuccessResult(string token, int userId, string loginId, string name, string kanaName, int kaId, bool isDoctor, int managerKbn, string sName, int hpId, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(token, userId, loginId, name, kanaName, kaId, isDoctor, managerKbn, sName, hpId, refreshToken, refreshTokenExpiryTime),
                Status = 1,
                Message = ResponseMessage.Success
            };
        }

        #endregion Helper methods
    }

    [HttpPost("RefreshToken")]
    public ActionResult<Response<RefreshTokenResponse>> RefreshAccessToken([FromBody] RefreshTokenRequest request)
    {
        ClaimsPrincipal? principal = AuthProvider.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null)
            return BadRequest("Invalid access token");

        int.TryParse(principal.FindFirstValue(ParamConstant.UserId), out int userId);
        var input = new RefreshTokenByUserInputData(userId, request.RefreshToken, AuthProvider.GeneratorRefreshToken());
        ///var input = new RefreshTokenByUserInputData(userId, request.RefreshToken, AuthProvider.GeneratorRefreshToken(), DateTime.UtcNow.AddMinutes(3));
        var output = _bus.Handle(input);
        if (output.Status == RefreshTokenByUserStatus.Successful)
        {
            string newToken = AuthProvider.GenerateAccessToken(new Claim[]
            {
                new(ParamConstant.UserId, userId.ToString()),
                new(ParamConstant.HpId, principal.FindFirstValue(ParamConstant.HpId)),
                new(ParamConstant.DepartmentId, principal.FindFirstValue(ParamConstant.DepartmentId)),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });

            // set cookie with new token
            SetCookie(newToken);

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

    [Authorize]
    [HttpGet("Authentication")]
    public ActionResult ValidateToken()
    {
        return Ok("Token is valid");
    }

    [HttpPost("AppToken"), Produces("application/json")]
    public ActionResult<Response<AppTokenResponse>> AppToken([FromBody] AppTokenRequest req)
    {
        var getUserInput = new GetUserByLoginIdInputData(req.LoginId, req.Password);
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
            new(ParamConstant.UserId, user.UserId.ToString()),
            new(ParamConstant.HpId, user.HpId.ToString()),
            new(ParamConstant.DepartmentId, user.KaId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        string token = AuthProvider.GenerateAppToken(claims).token;

        if (!string.IsNullOrEmpty(token))
        {
            var successResult = GetSuccessResult(token, user.UserId, user.LoginId);
            return Ok(successResult);
        }
        else
        {
            var errorResult = GetErrorResult("An error occurred while verification token");
            return BadRequest(errorResult);
        }

        #region Helper methods

        Response<AppTokenResponse> GetErrorResult(string errorMessage)
        {
            return new Response<AppTokenResponse>
            {
                Data = new AppTokenResponse(string.Empty, 0, string.Empty),
                Status = 0,
                Message = errorMessage
            };
        }

        Response<AppTokenResponse> GetSuccessResult(string token, int userId, string loginId)
        {
            return new Response<AppTokenResponse>
            {
                Data = new AppTokenResponse(token, userId, loginId),
                Status = 1,
                Message = ResponseMessage.Success
            };
        }

        #endregion Helper methods
    }

    private (string refreshToken, DateTime refreshTokenExpiryTime) SigInRefreshToken(int userId)
    {
        string refreshToken = AuthProvider.GeneratorRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddHours(AuthProvider.GetHoursRefreshTokenExpiryTime());
        var input = new SigninRefreshTokenInputData(userId, refreshToken, refreshTokenExpiryTime);
        var output = _bus.Handle(input);
        if (output.Status == SigninRefreshTokenStatus.Successful)
            return new(refreshToken, refreshTokenExpiryTime);
        else
            return new(string.Empty, DateTime.MinValue);
    }

    /// <summary>
    /// Set Cookie to report author
    /// </summary>
    /// <param name="token"></param>
    private void SetCookie(string token)
    {
        // get domain from headers
        var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
        string clientDomain = headers != null && headers.ContainsKey(ParamConstant.Domain) ? headers[ParamConstant.Domain].AsString() : string.Empty;

        // set cookie
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddDays(1);
        options.Path = "/";
        options.Secure = true;
        options.SameSite = SameSiteMode.None;

        var cookieObject = new CookieModel(clientDomain, token);
        string dataCookie = JsonSerializer.Serialize(cookieObject);
        HttpContext.Response.Cookies.Append(DomainCookie.CookieReportKey, dataCookie, options);
    }
}