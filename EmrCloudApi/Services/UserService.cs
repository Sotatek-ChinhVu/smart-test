﻿using EmrCloudApi.Constants;
using System.Security.Claims;

namespace EmrCloudApi.Services;

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
            int.TryParse(user.FindFirstValue(LoginUserConstant.DepartmentId), out int departmentId);
            result.UserId = userId;
            result.HpId = hpId;
            result.DepartmentId = departmentId;
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString() ?? string.Empty;
            result.Token = token.Replace("Bearer ", "");
        }
        return result;
    }
}
