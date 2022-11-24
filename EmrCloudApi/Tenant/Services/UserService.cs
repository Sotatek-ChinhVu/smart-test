using EmrCloudApi.Tenant.Constants;
using System.Security.Claims;

namespace EmrCloudApi.Tenant.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public LoginTokenResponse GetLoginUser()
    {
        var result = new LoginTokenResponse();
        if (_httpContextAccessor.HttpContext != null)
        {
            var user = _httpContextAccessor.HttpContext.User;
            int.TryParse(user.FindFirstValue(LoginUserConstant.HpId), out int hpId);
            int.TryParse(user.FindFirstValue(LoginUserConstant.UserId), out int userId);
            result.UserId = userId;
            result.HpId = hpId;
        }

        return result;
    }
}
