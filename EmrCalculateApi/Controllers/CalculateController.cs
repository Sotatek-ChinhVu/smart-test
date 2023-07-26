using Domain.Models.Futan;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Responses;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Realtime;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        private readonly IIkaCalculateViewModel _ikaCalculate;
        private readonly IWebSocketService _webSocketService;
        private CancellationToken? _cancellationToken;

        public CalculateController(IIkaCalculateViewModel ikaCalculate, IWebSocketService webSocketService)
        {
            _ikaCalculate = ikaCalculate;
            _webSocketService = webSocketService;
        }

        [HttpPost("RunCalculateOne")]
        public ActionResult RunCalculateOne([FromBody] CalculateOneRequest calculateOneRequest)
        {
            _ikaCalculate.RunCalculateOne(
                calculateOneRequest.HpId,
                calculateOneRequest.PtId,
                calculateOneRequest.SinDate,
                calculateOneRequest.SeikyuUp,
                calculateOneRequest.Prefix);
            return Ok();
        }

        [HttpPost("RunCalculate")]
        public ActionResult RunCalculate([FromBody] CalculateRequest calculateRequest)
        {
            _ikaCalculate.RunCalculate(
                calculateRequest.HpId,
                calculateRequest.PtId,
                calculateRequest.SinDate,
                calculateRequest.SeikyuUp,
                calculateRequest.Prefix);
            return Ok();
        }

        [HttpPost("RunTrialCalculate")]
        public ActionResult RunTrialCalculate([FromBody] RunTraialCalculateRequest calculateRequest)
        {
            var data = _ikaCalculate.RunTraialCalculate(
                calculateRequest.OrderInfoList,
                calculateRequest.Reception,
                calculateRequest.CalcFutan);
            var result = new RunTraialCalculateResponse(data.sinMeis, data.kaikeis.Select(k => new KaikeiInfItemResponse(k)).ToList(), data.calcLogs);
            return Ok(result);
        }

        [HttpPost("RunCalculateMonth")]
        public async Task<ActionResult> RunCalculateMonth([FromBody] RunCalculateMonthRequest monthRequest, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
                Messenger.Instance.Register<StopCalcStatus>(this, StopCalculation);

                HttpContext.Response.ContentType = "application/json";
                await _webSocketService.SendMessageAsync(FunctionCodes.RunCalculateMonth, "CHECK");
                _ikaCalculate.RunCalculateMonth(
                             monthRequest.HpId,
                             monthRequest.SeikyuYm,
                             monthRequest.PtIds,
                             monthRequest.PreFix);
            }
            catch
            {
                var resultForFrontEnd = Encoding.UTF8.GetBytes("Error");
                await _webSocketService.SendMessageAsync(FunctionCodes.RunCalculateMonth, resultForFrontEnd);
            }
            finally
            {
                Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
                Messenger.Instance.Deregister<StopCalcStatus>(this, StopCalculation);
                HttpContext.Response.Body.Close();
            }
            return Ok();
        }

        private void StopCalculation(StopCalcStatus stopCalcStatus)
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

        private void UpdateRecalculationStatus(RecalculationStatus status)
        {
            var result = _webSocketService.SendMessageAsync(FunctionCodes.RunCalculateMonth, status);
            result.Wait();
        }

        private void AddMessageCheckErrorInMonth(RecalculationStatus status)
        {
            string result = "\n" + JsonSerializer.Serialize(status);
            var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
    }
}
