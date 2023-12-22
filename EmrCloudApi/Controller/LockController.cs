using Domain.Models.Lock;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Lock;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Lock;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using EmrCloudApi.Services;
using Helper.Constants;
using Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using UseCase.Core.Sync;
using UseCase.Lock.Add;
using UseCase.Lock.Check;
using UseCase.Lock.CheckExistFunctionCode;
using UseCase.Lock.CheckIsExistedOQLockInfo;
using UseCase.Lock.Get;
using UseCase.Lock.GetLockInf;
using UseCase.Lock.Remove;
using UseCase.Lock.Unlock;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private CancellationToken? _cancellationToken;
        private readonly IWebSocketService _webSocketService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LockController(UseCaseBus bus, IUserService userService, IHttpContextAccessor httpContextAccessor, IWebSocketService webSocketService) : base(userService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost(ApiPath.AddLock)]
        public async Task<ActionResult<Response<LockResponse>>> AddLock([FromBody] LockRequest request, CancellationToken cancellationToken)
        {
            var input = new AddLockInputData(HpId, request.PtId, request.FunctionCod, request.SinDate, request.RaiinNo, UserId, request.TabKey, request.LoginKey);
            var output = _bus.Handle(input);
            AddLockPresenter presenter = new();
            presenter.Complete(output);
            var result = new ActionResult<Response<LockResponse>>(presenter.Result);

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

                // Reset cookie expire date
                if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.Request.Cookies[DomainCookie.CookieReportKey]))
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddDays(-1);
                    options.Path = "/";
                    options.Secure = true;
                    options.SameSite = SameSiteMode.None;
                    HttpContext.Response.Cookies.Append(DomainCookie.CookieReportKey, string.Empty, options);
                }
            }

            var presenter = new RemoveLockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateVisitingLockResponse>>(presenter.Result);
        }

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

        [HttpGet(ApiPath.GetLockInf)]
        public ActionResult<Response<GetLockInfResponse>> GetLockInf([FromQuery] GetLockInfRequest request)
        {
            var input = new GetLockInfInputData(HpId, UserId, request.ManagerKbn);
            var output = _bus.Handle(input);

            var presenter = new GetLockInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetLockInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckIsExistedOQLockInfo)]
        public ActionResult<Response<CheckIsExistedOQLockInfoResponse>> CheckIsExistedOQLockInfo(CheckIsExistedOQLockInfoRequest request)
        {
            var input = new CheckIsExistedOQLockInfoInputData(HpId, UserId, request.PtId, request.FunctionCd, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new CheckIsExistedOQLockInfoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckIsExistedOQLockInfoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Unlock)]
        public ActionResult<Response<UnlockResponse>> Unlock(UnlockRequest request)
        {
            var input = new UnlockInputData(HpId, UserId, request.LockInfModels.Select(x => LockInfInputItemRequestToModel(x)).ToList(), request.ManagerKbn);
            var output = _bus.Handle(input);

            var presenter = new UnlockPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UnlockResponse>>(presenter.Result);
        }

        private LockInfModel LockInfInputItemRequestToModel(LockInfInputItem lockInfInputItem)
        {
            return
                new LockInfModel
                (
                    new LockPtInfModel(lockInfInputItem.PatientInfoModels.PtId,
                                       lockInfInputItem.PatientInfoModels.FunctionName,
                                       lockInfInputItem.PatientInfoModels.PtNum,
                                       lockInfInputItem.PatientInfoModels.SinDate,
                                       lockInfInputItem.PatientInfoModels.LockDate,
                                       lockInfInputItem.PatientInfoModels.Machine,
                                       lockInfInputItem.PatientInfoModels.FunctionCd,
                                       lockInfInputItem.PatientInfoModels.RaiinNo,
                                       lockInfInputItem.PatientInfoModels.OyaRaiinNo,
                                       lockInfInputItem.PatientInfoModels.UserId),
                    new LockCalcStatusModel(lockInfInputItem.CalcStatusModels.CalcId,
                                            lockInfInputItem.CalcStatusModels.PtId,
                                            lockInfInputItem.CalcStatusModels.PtNum,
                                            lockInfInputItem.CalcStatusModels.SinDate,
                                            lockInfInputItem.CalcStatusModels.CreateDate,
                                            lockInfInputItem.CalcStatusModels.CreateMachine,
                                            lockInfInputItem.CalcStatusModels.CreateId),
                    new LockDocInfModel(lockInfInputItem.DocInfModels.PtId,
                                        lockInfInputItem.DocInfModels.PtNum,
                                        lockInfInputItem.DocInfModels.SinDate,
                                        lockInfInputItem.DocInfModels.RaiinNo,
                                        lockInfInputItem.DocInfModels.SeqNo,
                                        lockInfInputItem.DocInfModels.CategoryCd,
                                        lockInfInputItem.DocInfModels.FileName,
                                        lockInfInputItem.DocInfModels.DspFileName,
                                        lockInfInputItem.DocInfModels.IsLocked,
                                        lockInfInputItem.DocInfModels.LockDate,
                                        lockInfInputItem.DocInfModels.LockId,
                                        lockInfInputItem.DocInfModels.LockMachine,
                                        lockInfInputItem.DocInfModels.IsDeleted)
                );
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
