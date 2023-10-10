using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Lock;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Lock;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
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
        private CancellationToken? _cancellationToken;
        private readonly IWebSocketService _webSocketService;

        public LockController(UseCaseBus bus, IUserService userService, IWebSocketService webSocketService) : base(userService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpPost(ApiPath.AddLock)]
        public async Task<ActionResult<Response<LockResponse>>> AddLock([FromBody] LockRequest request, CancellationToken cancellationToken)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = new AddLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, request.TabKey, request.LoginKey);
            var output = _bus.Handle(input);
            AddLockPresenter presenter = new();
            presenter.Complete(output);
            var result = new ActionResult<Response<LockResponse>>(presenter.Result);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            var json = JsonSerializer.Serialize(input);

            Console.WriteLine("Addlock Time: " + DateTime.Now);
            Console.WriteLine("Addlock RunTime: " + elapsedTime);
            Console.WriteLine("Addlock Input: " + json);

            _cancellationToken = cancellationToken;
            if (_cancellationToken!.Value.IsCancellationRequested)
            {
                Console.WriteLine("Come in cancelation Addlock");
                if (output.Status == AddLockStatus.Successed)
                {
                    var inputDelete = new RemoveLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, false, false, request.TabKey);
                    _bus.Handle(inputDelete);
                }
                output = new AddLockOutputData(AddLockStatus.Failed, new(), new());
                presenter.Complete(output);
                Console.WriteLine("End cancelation Addlock ");
                return new ActionResult<Response<LockResponse>>(presenter.Result);
            }
            else
            {
                if (output.Status == AddLockStatus.Successed)
                {
                    await _webSocketService.SendMessageAsync(FunctionCodes.LockChanged, output.ResponseLockModel);
                }
            }

            return result;
        }

        [HttpPost(ApiPath.CheckLock)]
        public ActionResult<Response<LockResponse>> CheckLock([FromBody] CheckLockRequest request)
        {
            var input = new CheckLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, request.TabKey);
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

        [HttpPost(ApiPath.RemoveLock)]
        public async Task<ActionResult<Response<UpdateVisitingLockResponse>>> RemoveLock([FromBody] LockRequest request)
        {
            var input = new RemoveLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, false, false, request.TabKey);
            var output = _bus.Handle(input);

            if (output.Status == RemoveLockStatus.Successed)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.LockChanged, output.ResponseLockList);
            }

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateVisitingLockResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.RemoveAllLock)]
        public ActionResult<Response<UpdateVisitingLockResponse>> RemoveAllLock()
        {
            var input = new RemoveLockInputData(HpId, 0, "", 0, 0, UserId, true, false);
            var output = _bus.Handle(input);

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateVisitingLockResponse>>(presenter.Result);
        }


        [HttpGet(ApiPath.RemoveAllLockPtId)]
        public async Task<ActionResult<Response<UpdateVisitingLockResponse>>> RemoveAllLockPtId([FromQuery] RemoveAllLockPtIdRequest request)
        {
            var input = new RemoveLockInputData(HpId, request.PtId, request.FunctionCd, request.SinDate, 0, UserId, false, true, request.TabKey);
            var output = _bus.Handle(input);

            if (output.Status == RemoveLockStatus.Successed)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.LockChanged, output.ResponseLockList);
            }

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateVisitingLockResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.RemoveLockWhenLogOut)]
        public async Task<ActionResult<Response<UpdateVisitingLockResponse>>> RemoveLockWhenLogOut([FromBody] RemoveLockWhenLogOutRequest request)
        {
            var input = new RemoveLockInputData(HpId, UserId, true, request.LoginKey);
            var output = _bus.Handle(input);

            if (output.Status == RemoveLockStatus.Successed)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.LockChanged, output.ResponseLockList);
            }

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateVisitingLockResponse>>(presenter.Result);
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
            var input = new CheckLockVisitingInputData(HpId, UserId, request.PtId, request.SinDate, request.FunctionCode, request.TabKey);
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
