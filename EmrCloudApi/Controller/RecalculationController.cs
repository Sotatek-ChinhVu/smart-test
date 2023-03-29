using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Services;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using UseCase.Core.Sync;
using UseCase.Receipt.Recalculation;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class RecalculationController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private CancellationToken? _cancellationToken;
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
            HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            HttpResponse response = HttpContext.Response;
            response.StatusCode = 202;

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
        AddMessageCheckErrorInMonth(status.Done, status.Type, status.Length, status.SuccessCount, status.Message);
    }

    private void AddMessageCheckErrorInMonth(bool done, int type, int length, int successCount, string messager)
    {
        StringBuilder titleProgressbar = new();
        titleProgressbar.Append("\n{ status: \"");
        titleProgressbar.Append(done ? "done" : "inprogess");
        titleProgressbar.Append("\", type: ");
        titleProgressbar.Append(type);
        titleProgressbar.Append(", length: ");
        titleProgressbar.Append(length);
        titleProgressbar.Append(", successCount: ");
        titleProgressbar.Append(successCount);
        titleProgressbar.Append(", message: \"");
        titleProgressbar.Append(messager);
        titleProgressbar.Append("\" }");

        var resultForFrontEnd = Encoding.UTF8.GetBytes(titleProgressbar.ToString());
        HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.FlushAsync();
    }
}
