using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ReceSeikyu;
using EmrCloudApi.Requests.ReceSeikyu;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.ReceSeikyu.GetList;
using UseCase.ReceSeikyu.SearchReceInf;
using UseCase.ReceSeikyu.Save;
using System.Text;
using Helper.Messaging.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using Helper.Messaging;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceSeikyuController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private CancellationToken? _cancellationToken;

        public ReceSeikyuController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetListReceSeikyu)]
        public ActionResult<Response<GetListReceSeikyuResponse>> GetListReceSeikyu([FromQuery] GetListReceSeikyuRequest request)
        {
            var input = new GetListReceSeikyuInputData(HpId, 
                                                       request.SinDate,
                                                       request.SinYm,
                                                       request.IsIncludingUnConfirmed,
                                                       request.PtNumSearch,
                                                       request.NoFilter,
                                                       request.IsFilterMonthlyDelay,
                                                       request.IsFilterReturn,
                                                       request.IsFilterOnlineReturn);
            var output = _bus.Handle(input);
            var presenter = new GetListReceSeikyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListReceSeikyuResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.SearchReceInf)]
        public ActionResult<Response<SearchReceInfResponse>> SearchReceInf([FromQuery] SearchReceInfRequest request)
        {
            var input = new SearchReceInfInputData(HpId, request.PtNum, request.SinYm);
            var output = _bus.Handle(input);
            var presenter = new SearchReceInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchReceInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveReceSeikyu)]
        public ActionResult<Response<SaveReceSeiKyuResponse>> SaveReceSeikyu([FromBody] SaveReceSeiKyuRequest request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                Messenger.Instance.Register<RecalculateInSeikyuPendingStatus>(this, UpdateRecalculationSaveReceSeikyu);
                Messenger.Instance.Register<RecalculateInSeikyuPendingStop>(this, StopCalculation);

                HttpContext.Response.ContentType = "application/json";
                HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
                HttpResponse response = HttpContext.Response;
                response.StatusCode = 202;

                var input = new SaveReceSeiKyuInputData(request.Data, request.SinYm, HpId, UserId);
                var output = _bus.Handle(input);
                var presenter = new SaveReceSeiKyuPresenter();
                presenter.Complete(output);
                return new ActionResult<Response<SaveReceSeiKyuResponse>>(presenter.Result);
            }
            finally
            {
                Messenger.Instance.Deregister<RecalculateInSeikyuPendingStatus>(this, UpdateRecalculationSaveReceSeikyu);
                Messenger.Instance.Deregister<RecalculateInSeikyuPendingStop>(this, StopCalculation);
                HttpContext.Response.Body.Close();
            }
        }

        private void StopCalculation(RecalculateInSeikyuPendingStop stopCalcStatus)
        {
            if (!_cancellationToken.HasValue)
            {
                stopCalcStatus.CallFailCallback(false);
            }
            else
            {
                stopCalcStatus.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
            }
        }

        private void UpdateRecalculationSaveReceSeikyu(RecalculateInSeikyuPendingStatus status)
        {
            StringBuilder titleProgressbar = new();
            titleProgressbar.Append("\n{ displayText: \"");
            titleProgressbar.Append(status.DisplayText);
            titleProgressbar.Append("\", percent: ");
            titleProgressbar.Append(status.Percent);
            titleProgressbar.Append("\" }");

            var resultForFrontEnd = Encoding.UTF8.GetBytes(titleProgressbar.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
    }
}
