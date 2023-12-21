using DocumentFormat.OpenXml.Spreadsheet;
using Helper.Constants;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Constants;
using SuperAdmin.Presenters.Admin;
using SuperAdmin.Requests.Admin;
using SuperAdmin.Responses;
using SuperAdmin.Responses.Admin;
using SuperAdminAPI.Requests.UserToken;
using SuperAdminAPI.Responses.UserToken;
using SuperAdminAPI.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.Login;
using UseCase.UserToken.GetInfoRefresh;
using UseCase.UserToken.SiginRefresh;

namespace SuperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public AdminController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("Login")]
        public ActionResult<Response<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var input = new LoginInputData(request.LoginId, request.Password);
            var output = _bus.Handle(input);

            // The claims that will be persisted in the tokens.
            var claims = new Claim[]
            {
            new(ParamConstant.UserId, output.User.Id.ToString()),
            new(ParamConstant.Name, output.User.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            Response<LoginResponse> result = new();
            result.Message = GetMessage(output.Status);
            result.Status = (int)output.Status;
            if (output.User.Id > 0)
            {
                string token = AuthProvider.GenerateAccessToken(claims);
                var resultRefreshToken = SigInRefreshToken(output.User.Id);
                result.Data = new LoginResponse(token, output.User.Id, output.User.Name, output.User.FullName, output.User.Role, resultRefreshToken.refreshToken, resultRefreshToken.refreshTokenExpiryTime);
    
                return new ActionResult<Response<LoginResponse>>(result);
            }
            else
            {
                return new ActionResult<Response<LoginResponse>>(result);
            }
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
                new(ParamConstant.Name, principal.FindFirstValue(ParamConstant.Name)),
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
                return new(refreshToken, refreshTokenExpiryTime);
            else
                return new(string.Empty, DateTime.MinValue);
        }

        private string GetMessage(LoginStatus status) => status switch
        {
            LoginStatus.Successed => ResponseMessage.Success,
            LoginStatus.Failed => ResponseMessage.Failed,
            LoginStatus.InvalidLoginId => ResponseMessage.InvalidLoginId,
            LoginStatus.InvalidPassWord => ResponseMessage.InvalidPassword,
            _ => string.Empty
        };
    }
}
