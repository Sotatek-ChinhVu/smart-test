using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Lock;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.Lock;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Lock.Add;
using UseCase.Lock.Check;
using UseCase.Lock.Remove;

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

            var presenter = new AddLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<LockResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.CheckLock)]
        public ActionResult<Response<LockResponse>> CheckLock([FromQuery] LockRequest request)
        {
            var input = new CheckLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId);
            var output = _bus.Handle(input);

            var presenter = new CheckLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<LockResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.RemoveLock)]
        public ActionResult<Response> RemoveLock([FromQuery] LockRequest request)
        {
            var input = new RemoveLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, false);
            var output = _bus.Handle(input);

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response>(presenter.Result);
        }

        [HttpGet(ApiPath.RemoveAllLock)]
        public ActionResult<Response> RemoveAllLock()
        {
            var input = new RemoveLockInputData(HpId, 0, "", 0, 0, UserId, true);
            var output = _bus.Handle(input);

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response>(presenter.Result);
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
