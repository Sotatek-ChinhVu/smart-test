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
        var result  = new LoginTokenResponse();
        if (_httpContextAccessor.HttpContext !=null)
        {
            var user = _httpContextAccessor.HttpContext.User;
            result.UserId = user.FindFirstValue(LoginUserConstant.UserId);
            result.HpId = user.FindFirstValue(LoginUserConstant.HpId);
        }

        return result;
    }
}
