using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Infrastructure.Common;
namespace EmrCloudApi.Controller;

[ApiController]
public class CookieController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public int HpId { get; private set; }

    public CookieController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var cookieModel = GetCookieModel();
        HpId = cookieModel.HpId;
    }

    private CookieModel GetCookieModel()
    {
        string cookieValue = _httpContextAccessor.HttpContext?.Request?.Cookies.FirstOrDefault().Value ?? string.Empty;
        if (!string.IsNullOrEmpty(cookieValue))
        {
            return JsonSerializer.Deserialize<CookieModel>(cookieValue) ?? new();
        }
        return new();
    }
}
