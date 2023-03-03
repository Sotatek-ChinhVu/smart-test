using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Accounting;
using EmrCloudApi.Requests.Accounting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.GetAccountingHeader;
using UseCase.Accounting.GetAccountingInf;
using UseCase.Accounting.GetHistoryOrder;
using UseCase.Accounting.GetPtByoMei;
using UseCase.Accounting.GetSinMei;
using UseCase.Accounting.PaymentMethod;
using UseCase.Accounting.SaveAccounting;
using UseCase.Accounting.WarningMemo;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class AccountingController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public AccountingController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetAccountingResponse>> GetList([FromQuery] GetAccountingRequest request)
        {
            var input = new GetAccountingInputData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.PaymentMethod)]
        public ActionResult<Response<GetPaymentMethodResponse>> GetList([FromQuery] GetPaymentMethodRequest request)
        {
            var input = new GetPaymentMethodInputData(request.HpId);
            var output = _bus.Handle(input);

            var presenter = new GetPaymentMethodPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPaymentMethodResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.WarningMemo)]
        public ActionResult<Response<GetWarningMemoResponse>> GetList([FromQuery] GetWarningMemoRequest request)
        {
            var input = new GetWarningMemoInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetWarningMemoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetWarningMemoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.PtByoMei)]
        public ActionResult<Response<GetPtByoMeiResponse>> GetList([FromQuery] GetPtByoMeiRequest request)
        {
            var input = new GetPtByoMeiInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetPtByoMeiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtByoMeiResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveAccounting)]
        public ActionResult<Response<SaveAccountingResponse>> SaveList([FromBody] SaveAccountingRequest request)
        {
            var input = new SaveAccountingInputData(HpId, request.PtId, UserId, request.SinDate, request.RaiinNo,
                request.SumAdjust, request.ThisWari, request.Credit, request.PayType, request.Comment, request.IsDisCharged);
            var output = _bus.Handle(input);

            var presenter = new SaveAccountingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveAccountingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.HistoryOrder)]
        public ActionResult<Response<GetAccountingHistoryOrderResponse>> GetList([FromQuery] GetAccountingHistoryOrderRequest request)
        {
            var input = new GetAccountingHistoryOrderInputData(request.PtId, HpId, UserId, request.SinDate, request.DeleteConditon, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingHistoryOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingHistoryOrderResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHeaderInf)]
        public ActionResult<Response<GetAccountingHeaderResponse>> GetList([FromQuery] GetAccountingHeaderRequest request)
        {
            var input = new GetAccountingHeaderInputData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetAccountingHeaderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAccountingHeaderResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckAccounting)]
        public ActionResult<Response<CheckAccountingStatusResponse>> ActionResult([FromBody] CheckAccountingStatusRequest request)
        {
            var input = new CheckAccountingStatusInputData(HpId, request.PtId, request.SinDate, request.RaiinNo, request.DebitBalance,
                request.SumAdjust, request.ThisCredit, request.Wari, request.IsDisCharge, request.IsDeletedSyuno, request.IsSaveAccounting,
                request.SyunoSeikyuDtos, request.AllSyunoSeikyuDtos);
            var output = _bus.Handle(input);
            var presenter = new CheckAccountingStatusPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckAccountingStatusResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSinMei)]
        public ActionResult<Response<GetSinMeiResponse>> GetList([FromQuery] GetSinMeiRequest request)
        {
            var input = new GetSinMeiInputData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetSinMeiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSinMeiResponse>>(presenter.Result);
        }
    }
}