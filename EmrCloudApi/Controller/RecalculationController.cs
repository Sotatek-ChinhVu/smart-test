using Domain.Models.Receipt;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Services;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class RecalculationController : AuthorizeControllerBase
{
    private readonly IRecalculationService _recalculationService;

    public RecalculationController(IRecalculationService recalculationService, IUserService userService) : base(userService)
    {
        _recalculationService = recalculationService;
    }

    [HttpPost]
    public void Recalculation([FromBody] RecalculationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.Headers.Add("Transfer-Encoding", "chunked");
            List<ReceCheckErrModel> newReceCheckErrList = new();
            StringBuilder errorText = new();
            StringBuilder errorTextSinKouiCount = new();
            var dataForLoop = _recalculationService.GetDataForLoop(HpId, request.SinYm, request.PtIdList);

            int allCheckCount = dataForLoop.ReceRecalculationList.Count;
            HttpContext.Response.Headers.Add("AllCheckCount", allCheckCount.ToString());
            HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(""), 0, Encoding.UTF8.GetBytes("").Length);
            HttpContext.Response.Body.FlushAsync();

            foreach (var recalculationItem in dataForLoop.ReceRecalculationList)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var dataInsideLoop = _recalculationService.GetDataInsideLoop(HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);

                var resultCheckError = _recalculationService.CheckError(HpId, recalculationItem.SinYm, recalculationItem, dataForLoop.ReceCheckOptList, dataForLoop.ReceRecalculationList, dataForLoop.AllReceCheckErrList, dataForLoop.AllSystemConfigList, dataForLoop.AllSyobyoKeikaList, dataForLoop.AllIsKantokuCdValidList, dataInsideLoop.SinKouiCountList, dataInsideLoop.TenMstByItemCdList, dataInsideLoop.ItemCdList);

                newReceCheckErrList.AddRange(resultCheckError.Item1);
                errorText.Append(resultCheckError.Item2);
                errorTextSinKouiCount.Append(resultCheckError.Item3);
                // Send the chunk to the client
                var resultForFrontEnd = Encoding.UTF8.GetBytes(recalculationItem.PtId + " done!\n");

                HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
                HttpContext.Response.Body.FlushAsync();
            }
            var count = newReceCheckErrList.Count;

            errorText.Append(errorTextSinKouiCount);
            errorText = _recalculationService.GetErrorTextAfterCheck(HpId, request.SinYm, ref errorText, request.PtIdList, dataForLoop.AllSystemConfigList, dataForLoop.ReceRecalculationList);
            var bytes = Encoding.UTF8.GetBytes(count + " ReceCheckErr, save action error!");
            if (!_recalculationService.SaveReceCheckErrList(HpId, UserId, newReceCheckErrList))
            {
                HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
            bytes = Encoding.UTF8.GetBytes("\n\"" + errorText + "\"\n" + count + " ReceCheckErr, save action success!");
            HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
        finally
        {
            _recalculationService.ReleaseResource();
            HttpContext.Response.Body.Close();
        }
    }
}
