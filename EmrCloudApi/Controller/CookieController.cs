using Domain.Models.UserToken;
using Helper.Constants;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller;

[ApiController]
public class CookieController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserTokenRepository _userTokenRepository;
    private int _hpId;

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
    //private int GetHpId()
    //{
    //    string cookieValue = _httpContextAccessor.HttpContext?.Request?.Cookies[DomainCookie.CookieReportKey] ?? string.Empty;
    //    if (!string.IsNullOrEmpty(cookieValue))
    //    {
    //        var cookie = JsonSerializer.Deserialize<CookieModel>(cookieValue);
    //        if (cookie == null)
    //        {
    //            return -1;
    //        }
    //        // check RefreshToken is valid
    //        if (_userTokenRepository.RefreshTokenIsValid(cookie.UserId, cookie.RefreshToken))
    //        {
    //            return cookie.HpId;
    //        }
    //    }
    //    return -1;
    //}

    private int GetHpId()
    {
        string? hpIdValue = _httpContextAccessor.HttpContext?.Request?.Query[ParamConstant.HpId] ?? string.Empty;

        if (!string.IsNullOrEmpty(hpIdValue))
        {
            return int.TryParse(hpIdValue, out _hpId) ? _hpId : -1;
        }
        return -1;
    }
}
