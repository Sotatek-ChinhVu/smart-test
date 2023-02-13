using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Accounting;
using EmrCloudApi.Requests.Accounting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Accounting.GetAccountingInf;
using UseCase.Accounting.PaymentMethod;
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
    }
}
