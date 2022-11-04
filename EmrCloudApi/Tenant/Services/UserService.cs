using System.Security.Claims;

namespace EmrCloudApi.Tenant.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetUserId()
    {
        var result  = string.Empty;
        if (_httpContextAccessor.HttpContext !=null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        return result;
    }
}
