using EmrCloudApi.Configs.Options;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Auth;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UseCase.Core.Sync;
using UseCase.User.GetByLoginId;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly JwtOptions _jwtOptions;

    public AuthController(UseCaseBus bus, IOptions<JwtOptions> jwtOptionsAccessor)
    {
        _bus = bus;
        _jwtOptions = jwtOptionsAccessor.Value;
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
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = CreateToken(claims);
        var successResult = GetSuccessResult(token, user.UserId);
        return Ok(successResult);

        #region Helper methods

        Response<ExchangeTokenResponse> GetErrorResult(string errorMessage)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(string.Empty, 0),
                Status = 0,
                Message = errorMessage
            };
        }

        Response<ExchangeTokenResponse> GetSuccessResult(string token, int userId)
        {
            return new Response<ExchangeTokenResponse>
            {
                Data = new ExchangeTokenResponse(token, userId),
                Status = 1,
                Message = ResponseMessage.Success
            };
        }

        #endregion
    }

    private string CreateToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
        var signingKey = new SymmetricSecurityKey(key);
        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(_jwtOptions.TokenLifetime),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
