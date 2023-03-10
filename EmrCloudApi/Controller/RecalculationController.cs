using Domain.Models.Receipt;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Services;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
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

    public RecalculationController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }


    [HttpPost]
    public void HistoryReceCmt([FromBody] RecalculationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);

            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            HttpResponse response = HttpContext.Response;
            response.StatusCode = 202;

            var input = new RecalculationInputData(HpId, UserId, request.SinYm, request.PtIdList);
            _bus.Handle(input);
            Messenger.Instance.Send(new StopCalcStatus(cancellationToken.IsCancellationRequested));
        }
        finally
        {
            Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);
            HttpContext.Response.Body.Close();
        }
    }

    //[HttpPost]
    //public void Recalculation([FromBody] RecalculationRequest request, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        Messenger.Instance.Register<RecalculationStatus>(this, UpdateRecalculationStatus);

    //        HttpContext.Response.ContentType = "application/json";
    //        HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
    //        HttpResponse response = HttpContext.Response;
    //        response.StatusCode = 202;
    //        List<ReceCheckErrModel> newReceCheckErrList = new();
    //        StringBuilder errorText = new();
    //        StringBuilder errorTextSinKouiCount = new();
    //        var dataForLoop = _recalculationService.GetDataForLoop(HpId, request.SinYm, request.PtIdList);

    //        int allCheckCount = dataForLoop.ReceRecalculationList.Count;
    //        HttpContext.Response.Headers.Add("AllCheckCount", allCheckCount.ToString());
    //        AddMessageCheckErrorInMonth(false, 3, allCheckCount, 0, string.Empty);

    //        int successCount = 1;
    //        foreach (var recalculationItem in dataForLoop.ReceRecalculationList)
    //        {
    //            if (cancellationToken.IsCancellationRequested)
    //            {
    //                break;
    //            }
    //            var dataInsideLoop = _recalculationService.GetDataInsideLoop(HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
    //            var resultCheckError = _recalculationService.CheckError(HpId, recalculationItem.SinYm, recalculationItem, dataForLoop.ReceCheckOptList, dataForLoop.ReceRecalculationList, dataForLoop.AllReceCheckErrList, dataForLoop.AllSystemConfigList, dataForLoop.AllSyobyoKeikaList, dataForLoop.AllIsKantokuCdValidList, dataInsideLoop.SinKouiCountList, dataInsideLoop.TenMstByItemCdList, dataInsideLoop.ItemCdList);

    //            newReceCheckErrList.AddRange(resultCheckError.Item1);
    //            errorText.Append(resultCheckError.Item2);
    //            errorTextSinKouiCount.Append(resultCheckError.Item3);

    //            // Send the chunk to the client
    //            if (allCheckCount == successCount)
    //            {
    //                break;
    //            }
    //            AddMessageCheckErrorInMonth(false, 3, allCheckCount, successCount, string.Empty);
    //            successCount++;
    //        }

    //        errorText.Append(errorTextSinKouiCount);
    //        errorText = _recalculationService.GetErrorTextAfterCheck(HpId, request.SinYm, ref errorText, request.PtIdList, dataForLoop.AllSystemConfigList, dataForLoop.ReceRecalculationList);

    //        if (cancellationToken.IsCancellationRequested || !_recalculationService.SaveReceCheckErrList(HpId, UserId, newReceCheckErrList))
    //        {
    //            AddMessageCheckErrorInMonth(false, 3, allCheckCount, successCount, string.Empty);
    //        }
    //        AddMessageCheckErrorInMonth(true, 3, allCheckCount, successCount, errorText.ToString());
    //    }
    //    finally
    //    {
    //        Messenger.Instance.Deregister<RecalculationStatus>(this, UpdateRecalculationStatus);

    //        _recalculationService.ReleaseResource();
    //        HttpContext.Response.Body.Close();
    //    }
    //}

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
