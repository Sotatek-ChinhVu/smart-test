using ClosedXML.Excel;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Services;
using Helper.Enum;
using Helper.Extension;
using Microsoft.AspNetCore.Mvc;
using Reporting.Mappers.Common;
using Reporting.ReportServices;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ImportCSVController : AuthorizeControllerBase
{
    private readonly IReportService _reportService;

    private readonly UseCaseBus _bus;
    public ImportCSVController(UseCaseBus bus, IUserService userService, IReportService reportService) : base(userService)
    {
        _bus = bus;
        _reportService = reportService;
    }

    [HttpPost(ApiPath.ReceListCsv)]
    public IActionResult GenerateKarteCsvReport([FromBody] ReceiptListExcelRequest request)
    {
        var data = _reportService.GetReceiptListExcel(HpId, request.SeikyuYm, ConvertToReceiptListAdvancedSearchInputData(HpId, request), request.IsIsExportTitle);
        return RenderExcel(data);
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

    private IActionResult RenderExcel(CommonExcelReportingModel dataModel)
    {
        var dataList = dataModel.Data;
        if (!dataList.Any())
        {
            return Content(@"
            <meta charset=""utf-8"">
            <title>印刷対象が見つかりません。</title>
            <p style='text-align: center;font-size: 25px;font-weight: 300'>印刷対象が見つかりません。</p>
            ", "text/html");
        }
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        using (var workbook = new XLWorkbook())
        {
            IXLWorksheet worksheet =
            workbook.Worksheets.Add(dataModel.SheetName);
            int rowIndex = 1;
            foreach (var row in dataList)
            {
                List<string> colDataList = row.Split(',').ToList();
                int colIndex = 1;
                foreach (var cellData in colDataList)
                {
                    worksheet.Cell(rowIndex, colIndex).Value = cellData;
                    colIndex++;
                }
                rowIndex++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, dataModel.FileName);
            }
        }
    }
}
