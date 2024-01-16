using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Infrastructure.Common;
using Helper.Constants;
using System.IdentityModel.Tokens.Jwt;
using Helper.Extension;
using Helper.Common;

namespace EmrCloudApi.Controller;

[ApiController]
public class CookieController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public int HpId { get; private set; }

    public CookieController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        HpId = GetHpId();
    }

    /// <summary>
    ///  Get HpId from cookie to report api
    /// </summary>
    /// <returns></returns>
    private int GetHpId()
    {
        string cookieValue = _httpContextAccessor.HttpContext?.Request?.Cookies[DomainCookie.CookieReportKey] ?? string.Empty;
        if (!string.IsNullOrEmpty(cookieValue))
        {
            var cookie = JsonSerializer.Deserialize<CookieModel>(cookieValue);
            if (cookie == null)
            {
                return -1;
            }
            var jwtToken = new JwtSecurityToken(cookie.Token);
            if (jwtToken.ValidFrom < DateTime.UtcNow && jwtToken.ValidTo > DateTime.UtcNow)
            {
                int result = jwtToken.Payload[ParamConstant.HpId].AsInteger();
                if (result > 0)
                {
                    return result;
                }
            }
        }
        return -1;
    }
}
