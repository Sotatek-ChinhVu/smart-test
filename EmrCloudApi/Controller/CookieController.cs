using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Infrastructure.Common;
using Helper.Constants;
using Domain.Models.UserToken;

namespace EmrCloudApi.Controller;

[ApiController]
public class CookieController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserTokenRepository _userTokenRepository;

    public int HpId { get; private set; }

    public CookieController(IHttpContextAccessor httpContextAccessor, IUserTokenRepository userTokenRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userTokenRepository = userTokenRepository;
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
            // check RefreshToken is valid
            if (_userTokenRepository.RefreshTokenIsValid(cookie.UserId, cookie.RefreshToken))
            {
                return cookie.HpId;
            }
        }
        return -1;
    }
}
