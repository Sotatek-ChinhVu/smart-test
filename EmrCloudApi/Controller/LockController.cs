using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Lock;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Services;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Lock.Add;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public LockController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.AddLock)]
        public ActionResult<Response<LockResponse>> AddLock([FromQuery] LockRequest request)
        {
            var input = new AddLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId);
            var output = _bus.Handle(input);

            string messsage = output.Status == AddLockStatus.Successed ? "Successed" : "The lock is existed!!!";
            return new ActionResult<Response<LockResponse>>(new Response<LockResponse>()
            {
                Message = messsage,
                Status = (int)output.Status,
                Data = new LockResponse()
                {
                    LockLevel = output.LockInf.LockLevel,
                    ScreenName = output.LockInf.FunctionName,
                    UserName = output.LockInf.UserName
                }
            });
        }

        [HttpGet("CheckLock")]
        public IActionResult CheckLock([FromQuery] LockRequest request)
        {

            return Ok();
        }
    }

    public class Locker
    {
        private readonly List<LockInfo> _lockList = new List<LockInfo>();
        private static readonly object locker = new object();
        public object RegisterLock(string tenantId, long ptId)
        {
            lock (locker)
            {
                var lockInfo = _lockList.FirstOrDefault(l => l.TenantId == tenantId && l.PtId == ptId);
                if (lockInfo == null)
                {
                    lockInfo = new LockInfo()
                    {
                        TenantId = tenantId,
                        PtId = ptId,
                        Count = 1
                    };
                    _lockList.Add(lockInfo);
                }
                else
                {
                    lockInfo.Count++;
                }
                return lockInfo.LockObject;
            }
        }

        public void DeregisterLock(string tenantId, long ptId)
        {
            lock (locker)
            {
                var lockInfo = _lockList.FirstOrDefault(l => l.TenantId == tenantId && l.PtId == ptId);
                if (lockInfo == null)
                {
                    return;
                }
                lockInfo.Count--;
                if (lockInfo.Count == 0)
                {
                    _lockList.Remove(lockInfo);
                }
            }
        }
    }

    public class LockInfo
    {
        public string TenantId { get; set; } = string.Empty;

        public long PtId { get; set; }

        public object LockObject { get; set; } = new object();

        public int Count { get; set; }
    }
}
