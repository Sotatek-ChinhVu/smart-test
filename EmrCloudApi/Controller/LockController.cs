using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.Lock;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Lock;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Services;
using Helper.Constants;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Lock.Add;
using UseCase.Lock.Check;
using UseCase.Lock.CheckExistFunctionCode;
using UseCase.Lock.Get;
using UseCase.Lock.Remove;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;

        public LockController(UseCaseBus bus, IUserService userService, IWebSocketService webSocketService) : base(userService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpGet(ApiPath.AddLock)]
        public async Task<ActionResult<Response<LockResponse>>> AddLock([FromQuery] LockRequest request)
        {
            var input = new AddLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, Token);
            var output = _bus.Handle(input);

            if (output.Status == AddLockStatus.Successed)
            {
                string functionCode = request.FunctionCod == FunctionCode.SwitchOrderCode ? FunctionCode.MedicalExaminationCode : request.FunctionCod;
                await _webSocketService.SendMessageAsync(FunctionCodes.AddLockChanged,
                    new LockMessage { SinDate = request.SinDate, RaiinNo = request.RaiinNo, PtId = request.PtId, Type = 1, FunctionCod = functionCode });
            }

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

        [HttpGet(ApiPath.CheckExistFunctionCode)]
        public ActionResult<Response<CheckExistFunctionCodeResponse>> CheckOpenSpecialNote([FromQuery] CheckExistFunctionCodeRequest request)
        {
            var input = new CheckExistFunctionCodeInputData(HpId, request.FunctionCod, request.PtId);
            var output = _bus.Handle(input);

            var presenter = new CheckExistFunctionCodePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckExistFunctionCodeResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.RemoveLock)]
        public async Task<ActionResult<Response>> RemoveLock([FromQuery] LockRequest request)
        {
            var input = new RemoveLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, false, false);
            var output = _bus.Handle(input);

            if (output.Status == RemoveLockStatus.Successed)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.RemoveLockChanged,
                    new LockMessage { SinDate = request.SinDate, RaiinNo = request.RaiinNo, PtId = request.PtId, Type = 2, FunctionCod = request.FunctionCod });
            }

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response>(presenter.Result);
        }

        [HttpGet(ApiPath.RemoveAllLock)]
        public ActionResult<Response> RemoveAllLock()
        {
            var input = new RemoveLockInputData(HpId, 0, "", 0, 0, UserId, true, false);
            var output = _bus.Handle(input);

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response>(presenter.Result);
        }


        [HttpGet(ApiPath.RemoveAllLockPtId)]
        public ActionResult<Response> RemoveAllLockPtId([FromQuery] RemoveAllLockPtIdRequest request)
        {
            var input = new RemoveLockInputData(HpId, request.PtId, request.FunctionCd, request.SinDate, 0, UserId, false, true);
            var output = _bus.Handle(input);

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response>(presenter.Result);
        }

        //[HttpGet(ApiPath.ExtendTtl)]
        //public ActionResult<Response> ExtendTtl([FromQuery] LockRequest request)
        //{
        //    var input = new ExtendTtlLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId);
        //    var output = _bus.Handle(input);

        //    var presenter = new ExtentTtlPresenter();
        //    presenter.Complete(output);

        //    return new ActionResult<Response>(presenter.Result);
        //}


        [HttpPost(ApiPath.GetLockInfo)]
        public ActionResult<Response<GetLockInfoResponse>> GetList([FromBody] GetLockInfoRequest request)
        {
            var input = new GetLockInfoInputData(HpId, request.PtId, request.ListFunctionCdB, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetLockInfoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetLockInfoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.CheckLockVisiting)]
        public ActionResult<Response<CheckLockVisitingResponse>> CheckLockVisiting([FromQuery] CheckLockVisitingRequest request)
        {
            var input = new CheckLockVisitingInputData(HpId, UserId, request.PtId, request.SinDate, request.FunctionCode, Token);
            var output = _bus.Handle(input);

            var presenter = new CheckLockVisitingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckLockVisitingResponse>>(presenter.Result);
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
