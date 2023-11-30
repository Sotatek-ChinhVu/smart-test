using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.Accounting;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Accounting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.CheckOpenAccounting;
using UseCase.Accounting.GetAccountingFormMst;
using UseCase.Accounting.GetAccountingHeader;
using UseCase.Accounting.GetAccountingInf;
using UseCase.Accounting.GetAccountingSystemConf;
using UseCase.Accounting.GetListHokenSelect;
using UseCase.Accounting.GetPtByoMei;
using UseCase.Accounting.GetSinMei;
using UseCase.Accounting.PaymentMethod;
using UseCase.Accounting.Recaculate;
using UseCase.Accounting.SaveAccounting;
using UseCase.Accounting.UpdateAccountingFormMst;
using UseCase.Accounting.WarningMemo;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class AccountingController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;

        public AccountingController(UseCaseBus bus, IUserService userService, IWebSocketService webSocketService) : base(userService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetAccountingResponse>> GetList([FromQuery] GetAccountingRequest request)
        {
            var input = new GetAccountingInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.PaymentMethod)]
        public ActionResult<Response<GetPaymentMethodResponse>> PaymentMethod()
        {
            var input = new GetPaymentMethodInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetPaymentMethodPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPaymentMethodResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.WarningMemo)]
        public ActionResult<Response<GetWarningMemoResponse>> WarningMemo([FromQuery] GetWarningMemoRequest request)
        {
            var input = new GetWarningMemoInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetWarningMemoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetWarningMemoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.PtByoMei)]
        public ActionResult<Response<GetPtByoMeiResponse>> PtByoMei([FromQuery] GetPtByoMeiRequest request)
        {
            var input = new GetPtByoMeiInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetPtByoMeiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtByoMeiResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveAccounting)]
        public async Task<ActionResult<Response<SaveAccountingResponse>>> SaveAccounting([FromBody] SaveAccountingRequest request)
        {
            var input = new SaveAccountingInputData(HpId, request.PtId, UserId, request.SinDate, request.RaiinNo,
                request.SumAdjust, request.ThisWari, request.Credit, request.PayType, request.Comment, request.IsDisCharged, request.KaikeiTime);
            var output = _bus.Handle(input);

            if (output.Status == SaveAccountingStatus.Success)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
            }

            var presenter = new SaveAccountingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveAccountingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHeaderInf)]
        public ActionResult<Response<GetAccountingHeaderResponse>> GetHeaderInf([FromQuery] GetAccountingHeaderRequest request)
        {
            var input = new GetAccountingHeaderInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingHeaderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingHeaderResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckAccounting)]
        public ActionResult<Response<CheckAccountingStatusResponse>> CheckAccounting([FromBody] CheckAccountingStatusRequest request)
        {
            var input = new CheckAccountingStatusInputData(HpId, request.PtId, request.SinDate, request.RaiinNo, request.DebitBalance,
                request.SumAdjust, request.Credit, request.Wari, request.IsDisCharge, request.IsSaveAccounting,
                request.SyunoSeikyuDtos, request.AllSyunoSeikyuDtos);
            var output = _bus.Handle(input);
            var presenter = new CheckAccountingStatusPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckAccountingStatusResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSystemConfig)]
        public ActionResult<Response<GetAccountingConfigResponse>> GetSystemConfig([FromQuery] GetAccountingConfigRequest request)
        {
            var input = new GetAccountingConfigInputData(HpId, request.PtId, request.RaiinNo, request.SumAdjust);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingConfigPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingConfigResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetMeiHoGai)]
        public ActionResult<Response<GetMeiHoGaiResponse>> GetMeiHoGai([FromQuery] GetMeiHoGaiRequest request)
        {
            var input = new GetMeiHoGaiInputData(HpId, request.PtId, request.SinDate, request.RaiinNos);
            var output = _bus.Handle(input);

            var presenter = new GetMeiHoGaiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMeiHoGaiResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.CheckOpenAccounting)]
        public ActionResult<Response<CheckOpenAccountingResponse>> CheckOpenAccounting([FromQuery] CheckOpenAccountingRequest request)
        {
            var input = new CheckOpenAccountingInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new CheckOpenAccountingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckOpenAccountingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Recaculation)]
        public ActionResult<Response<RecaculationResponse>> Recaculation([FromBody] RecaculationRequest request)
        {
            var input = new RecaculationInputData(HpId, request.RaiinNo, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new RecaculationPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<RecaculationResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAccountingFormMst)]
        public ActionResult<Response<UpdateAccountingFormMstResponse>> UpdateAccountingFormMst([FromBody] UpdateAccountingFormMstRequest request)
        {
            var input = new UpdateAccountingFormMstInputData(UserId, request.AccountingFormMstModels);
            var output = _bus.Handle(input);

            var presenter = new UpdateAccountingFormMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateAccountingFormMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetAccountingFormMst)]
        public ActionResult<Response<GetAccountingFormMstResponse>> GetAccountingFormMst()
        {
            var input = new GetAccountingFormMstInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingFormMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingFormMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListHokenSelect)]
        public ActionResult<Response<GetListHokenSelectResponse>> GetListHokenSelect([FromQuery] GetListHokenSelectRequest request)
        {
            var input = new GetListHokenSelectInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetListHokenSelectPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListHokenSelectResponse>>(presenter.Result);
        }
    }
}