using EmrCloudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller
{
    [ApiController]
    [Authorize]
    public class AuthorizeControllerBase : ControllerBase
    {
        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public AuthorizeControllerBase(IUserService userService)
        {
            var userInfo = userService.GetLoginUser();

            HpId = userInfo.HpId;
            UserId = userInfo.UserId;
        }
    }
}
