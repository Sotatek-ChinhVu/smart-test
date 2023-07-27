using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Receipt;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Requests.ReceiptCheck;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Services;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using UseCase.Core.Sync;
using UseCase.Receipt.Recalculation;
using UseCase.ReceiptCheck.Recalculation;
using UseCase.ReceiptCheck.ReceiptInfEdit;
using Castle.Core.Internal;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class RecalculationController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private CancellationToken? _cancellationToken;
    private Socket server;
    public RecalculationController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
        
    }

    [HttpPost]
    public void Recalculation([FromBody] RecalculationRequest request, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        try
        {
            Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
            Messenger.Instance.Register<StopCalcStatus>(this, StopCalculation);

            HttpContext.Response.ContentType = "application/json";
            //HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            HttpResponse response = HttpContext.Response;
            //response.StatusCode = 202;

            var input = new RecalculationInputData(HpId, UserId, request.SinYm, request.PtIdList, request.IsRecalculationCheckBox, request.IsReceiptAggregationCheckBox, request.IsCheckErrorCheckBox);
            _bus.Handle(input);
        }
        catch
        {
            var resultForFrontEnd = Encoding.UTF8.GetBytes("Error");
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
        finally
        {
            Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
            Messenger.Instance.Deregister<StopCalcStatus>(this, StopCalculation);
            HttpContext.Response.Body.Close();
            server.Dispose();
        }
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
        if ((status.Type == 1 && status.Message.Equals("StartCalculateMonth"))
            || (status.Type == 2 && status.Message.Equals("StartFutanCalculateMain")))
        {
            var hostName = Dns.GetHostName();
            Task.Run(() =>
            {
                CreateSocketServer(hostName);
            });
        }
        else
        {
            AddMessageCheckErrorInMonth(status);
        }
    }

    private void AddMessageCheckErrorInMonth(RecalculationStatus status)
    {
        string result = "\n" + JsonSerializer.Serialize(status);
        var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
        HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.FlushAsync();
    }

    [HttpPost(ApiPath.ReceiptCheck)]
    public void ReceiptCheckRecalculation([FromBody] ReceiptCheckRecalculationRequest request)
    {
        try
        {
            Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);
            Messenger.Instance.Register<StopCalcStatus>(this, StopCalculation);

            HttpContext.Response.ContentType = "application/json";
            //HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            HttpResponse response = HttpContext.Response;
            //response.StatusCode = 202;

            var input = new ReceiptCheckRecalculationInputData(HpId, UserId, request.PtIds, request.SeikyuYm, request.ReceStatus);
            _bus.Handle(input);
        }
        catch
        {
            var resultForFrontEnd = Encoding.UTF8.GetBytes("\n Error");
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
        finally
        {
            Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
            Messenger.Instance.Deregister<StopCalcStatus>(this, StopCalculation);
            HttpContext.Response.Body.Close();
        }
    }

    [HttpGet(ApiPath.DeleteReceiptInfEdit)]
    public ActionResult<Response<DeleteReceiptInfResponse>> DeleteReceiptInfEdit([FromQuery] DeleteReceiptInfEditRequest request)
    {
        var input = new DeleteReceiptInfEditInputData(HpId, UserId, request.PtId, request.SeikyuYm, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new DeleteReceiptInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<DeleteReceiptInfResponse>>(presenter.Result);
    }

    private void CreateSocketServer(string hostName)
    {
        var ipEntryAwait = Dns.GetHostEntryAsync(hostName);
        ipEntryAwait.Wait();
        IPHostEntry ipEntry = ipEntryAwait.Result;

        // we will axtract the local host ip 
        IPAddress ip = ipEntry.AddressList[0];

        // connect the server socket to client socket
        IPEndPoint iPEndPoint = new IPEndPoint(ip, 22222);

        server = new Socket(
            iPEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        server.Bind(iPEndPoint);
        server.Listen();

        var handlerAwait = server.AcceptAsync();
        handlerAwait.Wait();
        var handler = handlerAwait.Result;

        while (true)
        {
            var buffer = new byte[1024];

            // receive the message from client but as bytes
            var receviedAwait = handler.ReceiveAsync(buffer, SocketFlags.None);
            receviedAwait.Wait();
            var recevied = receviedAwait.Result;

            // convert bytes to string message
            var messageString = Encoding.UTF8.GetString(buffer, 0, recevied);
            if (!messageString.IsNullOrEmpty())
            {
                Console.WriteLine(messageString);
            }
        }
    }
}
