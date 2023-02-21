using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Accounting;
using EmrCloudApi.Requests.Accounting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Accounting.GetAccountingInf;
using UseCase.Accounting.GetPtByoMei;
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

            var input = new GetWarningMemoInputData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetWarningMemoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetWarningMemoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.PtByoMei)]
        public ActionResult<Response<GetPtByoMeiResponse>> GetList([FromQuery] GetPtByoMeiRequest request)
        {

            var input = new GetPtByoMeiInputData(request.HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetPtByoMeiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtByoMeiResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveAccounting)]
        public ActionResult<Response<SaveAccountingResponse>> SaveList([FromBody] SaveAccountingRequest request)
        {
            var input = new SaveAccountingInputData(request.HpId, request.PtId, request.UserId, request.SinDate, request.RaiinNo,
                request.SumAdjust, request.ThisWari, request.Credit, request.PayType, request.Comment, request.isDisCharged);
            var output = _bus.Handle(input);

            var presenter = new SaveAccountingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveAccountingResponse>>(presenter.Result);
        }
    }
}
