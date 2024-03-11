using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using Helper.Enum;
using Helper.Extension;
using Microsoft.AspNetCore.Mvc;
using Reporting.Mappers.Common;
using Reporting.ReportServices;
using System.Text;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ImportCSVController : BaseParamControllerBase
{
    private readonly IReportService _reportService;

    public ImportCSVController(IReportService reportService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _reportService = reportService;
    }

    [HttpPost(ApiPath.ReceListCsv)]
    public IActionResult GenerateKarteCsvReport([FromBody] ReceiptListExcelRequest request)
    {
        _reportService.Instance(31);
        var data = _reportService.GetReceiptListExcel(HpId, request.SeikyuYm, ConvertToReceiptListAdvancedSearchInputData(HpId, request), request.IsIsExportTitle);
        _reportService.ReleaseResource();

        return RenderCsv(data);
    }

    private ReceiptListAdvancedSearchInput ConvertToReceiptListAdvancedSearchInputData(int hpId, ReceiptListExcelRequest request)
    {
        var itemList = request.ItemList.Select(item => new ItemSearchModel(
                                                            item.ItemCd,
                                                            item.InputName,
                                                            item.RangeSeach,
                                                            item.Amount,
                                                            item.OrderStatus,
                                                            item.IsComment
                                        )).ToList();

        var byomeiCdList = request.ByomeiCdList.Select(item => new SearchByoMstModel(
                                                                    item.ByomeiCd,
                                                                    item.InputName,
                                                                    item.IsComment
                                               )).ToList();

        return new ReceiptListAdvancedSearchInput(
                   hpId,
                   request.SeikyuYm,
                   request.Tokki,
                   request.IsAdvanceSearch,
                   request.HokenSbts,
                   request.IsAll,
                   request.IsNoSetting,
                   request.IsSystemSave,
                   request.IsSave1,
                   request.IsSave2,
                   request.IsSave3,
                   request.IsTempSave,
                   request.IsDone,
                   request.ReceSbtCenter,
                   request.ReceSbtRight,
                   request.HokenHoubetu,
                   request.Kohi1Houbetu,
                   request.Kohi2Houbetu,
                   request.Kohi3Houbetu,
                   request.Kohi4Houbetu,
                   request.IsIncludeSingle,
                   request.HokensyaNoFrom,
                   request.HokensyaNoTo,
                   request.HokensyaNoFrom.AsLong(),
                   request.HokensyaNoTo.AsLong(),
                   request.PtId,
                   request.PtIdFrom,
                   request.PtIdTo,
                   request.PtSearchOption,
                   request.TensuFrom,
                   request.TensuTo,
                   request.LastRaiinDateFrom,
                   request.LastRaiinDateTo,
                   request.BirthDayFrom,
                   request.BirthDayTo,
                   itemList,
                   (QuerySearchEnum)request.ItemQuery,
                   request.IsOnlySuspectedDisease,
                   (QuerySearchEnum)request.ByomeiQuery,
                   byomeiCdList,
                   request.IsFutanIncludeSingle,
                   request.FutansyaNoFromLong,
                   request.FutansyaNoToLong,
                   request.KaId,
                   request.DoctorId,
                   request.Name,
                   request.IsTestPatientSearch,
                   request.IsNotDisplayPrinted,
                   request.GroupSearchModels,
                   request.SeikyuKbnAll,
                   request.SeikyuKbnDenshi,
                   request.SeikyuKbnPaper);
    }

    private IActionResult RenderCsv(CommonExcelReportingModel dataModel)
    {
        var dataList = dataModel.Data;
        if (!dataList.Any())
        {
            return Content(@"", "text/html");
        }
        var csv = new StringBuilder();

        string contentType = "text/csv";

        foreach (var row in dataList)
        {
            csv.AppendLine(row);
        }
        var content = Encoding.UTF8.GetBytes(csv.ToString());
        var result = Encoding.UTF8.GetPreamble().Concat(content).ToArray();
        return File(result, contentType, dataModel.FileName);
    }
}
