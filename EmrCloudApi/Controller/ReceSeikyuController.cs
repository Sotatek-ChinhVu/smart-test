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
using Helper.Messaging;
using Domain.Models.ReceSeikyu;
using UseCase.ReceSeikyu.ImportFile;
using UseCase.ReceSeikyu.CancelSeikyu;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Dynamic.Core.Tokenizer;
using UseCase.ReceSeikyu.GetReceSeikyModelByPtNum;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceSeikyuController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private CancellationToken? _cancellationToken;
        private readonly IMessenger _messenger;

        public ReceSeikyuController(UseCaseBus bus, IUserService userService, IMessenger messenger) : base(userService)
        {
            _bus = bus;
            _messenger = messenger;
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
                                                       request.IsFilterOnlineReturn,
                                                       request.IsGetDataPending
                                                       );
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

        [HttpPost(ApiPath.CancelSeikyu)]
        public ActionResult<Response<CancelSeikyuResponse>> CancelSeikyu([FromBody] CancelSeikyuRequest request)
        {
            var input = new CancelSeikyuInputData(HpId, request.SeikyuYm, request.SeikyuKbn, request.PtId, request.SinYm, request.HokenId, UserId);
            var output = _bus.Handle(input);
            var presenter = new CancelSeikyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CancelSeikyuResponse>>(presenter.Result);
        }

        /// <summary>
        /// Only pass records have IsModified = true;
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost(ApiPath.SaveReceSeikyu)]
        public void SaveReceSeikyu([FromBody] SaveReceSeiKyuRequest request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                _messenger.Register<RecalculateInSeikyuPendingStatus>(this, UpdateRecalculationSaveReceSeikyu);
                _messenger.Register<RecalculateInSeikyuPendingStop>(this, StopCalculation);

                HttpContext.Response.ContentType = "application/json";

                var input = new SaveReceSeiKyuInputData(request.Data.Select(x => new ReceSeikyuModel(0,
                                                                                                HpId,
                                                                                                x.PtId,
                                                                                                string.Empty,
                                                                                                x.SinYm,
                                                                                                0,
                                                                                                x.HokenId,
                                                                                                string.Empty,
                                                                                                x.SeqNo,
                                                                                                x.SeikyuYm,
                                                                                                x.SeikyuKbn,
                                                                                                x.PreHokenId,
                                                                                                x.Cmt ?? string.Empty,
                                                                                                0,
                                                                                                0,
                                                                                                string.Empty,
                                                                                                0,
                                                                                                0,
                                                                                                x.IsModified,
                                                                                                x.OriginSeikyuYm,
                                                                                                x.OriginSinYm,
                                                                                                x.IsAddNew,
                                                                                                x.IsDeleted,
                                                                                                x.IsChecked,
                                                                                                new())).ToList(), request.SinYm, HpId, UserId, _messenger);

                var output = _bus.Handle(input);
                if (output.Status == SaveReceSeiKyuStatus.Successful)
                    UpdateRecalculationSaveReceSeikyu(new RecalculateInSeikyuPendingStatus(string.Empty, 100, true, true));
                else
                    UpdateRecalculationSaveReceSeikyu(new RecalculateInSeikyuPendingStatus(string.Empty, 100, true, false));
            }
            finally
            {
                _messenger.Deregister<RecalculateInSeikyuPendingStatus>(this, UpdateRecalculationSaveReceSeikyu);
                _messenger.Deregister<RecalculateInSeikyuPendingStop>(this, StopCalculation);
            }
        }

        [HttpPost(ApiPath.ImportFileReceSeikyu)]
        public ActionResult<Response<ImportFileReceSeikyuResponse>> ImportFileReceSeikyu(IFormFile fileImport)
        {
            var input = new ImportFileReceSeikyuInputData(HpId, UserId, fileImport);
            var output = _bus.Handle(input);
            var presenter = new ImportFileReceSeikyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ImportFileReceSeikyuResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetReceSeikyModelByPtNum)]
        public ActionResult<Response<GetReceSeikyModelByPtNumResponse>> GetReceSeikyModelByPtNum([FromQuery] GetReceSeikyModelByPtNumRequest request)
        {
            var input = new GetReceSeikyModelByPtNumInputData(HpId,
                                                       request.SinDate,
                                                       request.SinYm,
                                                       request.PtNum
                                                       );
            var output = _bus.Handle(input);
            var presenter = new GetReceSeikyModelByPtNumPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetReceSeikyModelByPtNumResponse>>(presenter.Result);
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
            titleProgressbar.Append(", complete: ");
            titleProgressbar.Append(status.Complete.ToString().ToLower());
            titleProgressbar.Append(", completeSuccess: ");
            titleProgressbar.Append(status.CompleteSuccess.ToString().ToLower());
            titleProgressbar.Append(" }");
            var resultForFrontEnd = Encoding.UTF8.GetBytes(titleProgressbar.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
    }
}
