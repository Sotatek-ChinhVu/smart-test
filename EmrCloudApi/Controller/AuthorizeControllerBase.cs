using Helper.Constants;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller
{
    [ApiController]
    public class BaseParamControllerBase : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int _hpId;

        private int _userId;

        private int _departmentId;

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int DepartmentId { get; private set; }

        public BaseParamControllerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            HpId = GetHpId();
            UserId = GetUserId();
            DepartmentId = GetDepartmentId();
        }

        private int GetHpId()
        {
            string? hpId = string.Empty;
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.HpId))
            {
                return 0;
            }
            hpId = headers[ParamConstant.HpId];

            return int.TryParse(hpId, out _hpId) ? _hpId : 0;
        }

        private int GetUserId()
        {
            string? userId = string.Empty;
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.UserId))
            {
                return 0;
            }
            userId = headers[ParamConstant.UserId];

            return int.TryParse(userId, out _userId) ? _userId : 0;
        }

        private int GetDepartmentId()
        {
            string? departmentId = string.Empty;
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.DepartmentId))
            {
                return 0;
            }
            departmentId = headers[ParamConstant.DepartmentId];

            return int.TryParse(departmentId, out _departmentId) ? _departmentId : 0;
        }
    }
}
