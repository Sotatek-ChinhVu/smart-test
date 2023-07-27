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
using System.Net.Sockets;
using System.Net;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        private readonly IIkaCalculateViewModel _ikaCalculate;
        private readonly IWebSocketService _webSocketService;
        private CancellationToken? _cancellationToken;
        private Socket client;

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
        public ActionResult RunCalculateMonth([FromBody] RunCalculateMonthRequest monthRequest, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;

            try
            {
                Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
                Messenger.Instance.Register<StopCalcStatus>(this, StopCalculation);

                // info about localhost
                var ipEntryAwait = Dns.GetHostEntryAsync(Dns.GetHostName());
                ipEntryAwait.Wait();
                IPHostEntry ipEntry = ipEntryAwait.Result;

                // localhost ip address

                IPAddress ip = ipEntry.AddressList[0];

                IPEndPoint iPEndPoint = new(ip, 22222);

                // client socket
                client = new Socket(
                    iPEndPoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );

                var connect = client.ConnectAsync(iPEndPoint);
                connect.Wait();

                var resultForFrontEnd = Encoding.UTF8.GetBytes("start socket");
                var sendMessage = client.SendAsync(resultForFrontEnd, socketFlags: SocketFlags.None);
                sendMessage.Wait();

                //await _webSocketService.SendMessageAsync(FunctionCodes.RunCalculateMonth, "CHECK");
                _ikaCalculate.RunCalculateMonth(
                             monthRequest.HpId,
                             monthRequest.SeikyuYm,
                             monthRequest.PtIds,
                             monthRequest.PreFix);
            }
            catch
            {
                var resultForFrontEnd = Encoding.UTF8.GetBytes("Error");
                var sendMessage = client.SendAsync(resultForFrontEnd, socketFlags: SocketFlags.None);
                sendMessage.Wait();
                //await _webSocketService.SendMessageAsync(FunctionCodes.RunCalculateMonth, resultForFrontEnd);
            }
            finally
            {
                Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
                Messenger.Instance.Deregister<StopCalcStatus>(this, StopCalculation);
                var resultForFrontEnd = Encoding.UTF8.GetBytes("end socket");
                var sendMessage = client.SendAsync(resultForFrontEnd, socketFlags: SocketFlags.None);
                sendMessage.Wait();
                //client.Disconnect(true);
                //HttpContext.Response.Body.Close();
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
            AddMessageCheckErrorInMonth(status);
            //var result = _webSocketService.SendMessageAsync(FunctionCodes.RunCalculateMonth, status);
            //result.Wait();
        }

        private void AddMessageCheckErrorInMonth(RecalculationStatus status)
        {
            string result = "\n" + JsonSerializer.Serialize(status);
            var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());

            // send message to the server
            var sendMessage = client.SendAsync(resultForFrontEnd, socketFlags: SocketFlags.None);
            sendMessage.Wait();
        }
    }
}
