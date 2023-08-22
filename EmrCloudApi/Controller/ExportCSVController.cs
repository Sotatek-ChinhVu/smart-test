using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportCsv;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Services;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Reporting.ReportServices;
using System.Text;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ExportCSVController : AuthorizeControllerBase
{
    private readonly IReportService _reportService;

    private readonly UseCaseBus _bus;
    public ExportCSVController(UseCaseBus bus, IUserService userService, IReportService reportService, ITenantProvider tenantProvider) : base(userService)
    {
        _bus = bus;
        _reportService = reportService;
    }

    [HttpPost(ApiPath.ExportPeriodReceipt)]
    public IActionResult GenerateKarteCsvReport([FromBody] ExportCsvRequest request)
    {
        var data = _reportService.OutputExcelForPeriodReceipt(HpId, request.StartDate, request.EndDate,
                                                              request.PtConditions.Select(p => new Tuple<long, int>(p.PtId, p.HokenId)).ToList(),
                                                              request.GrpConditions.Select(p => new Tuple<int, string>(p.GrpId, p.GrpCd)).ToList(),
                                                              request.Sort, request.MiseisanKbn, request.SaiKbn, request.MisyuKbn, request.SeikyuKbn, request.HokenKbn);
        if (!data.Any())
        {
            return Ok("出力データがありません。");
        }

        return RenderCsv(data, "期間指定請求書リスト.csv");
    }

    [HttpGet(ApiPath.ExportStatics)]
    public IActionResult GenerateExportStatics([FromQuery] StatisticExportRequest request)
    {
        var data = _reportService.ExportCsv(HpId, request.FormName, request.MenuId, request.MonthFrom, request.MonthTo, request.DateFrom, request.DateTo, request.TimeFrom, request.TimeTo,
                                            request.CoFileType, request.IsPutTotalRow, request.TenkiDateFrom, request.TenkiDateTo, request.EnableRangeFrom, request.EnableRangeTo, request.PtNumFrom, request.PtNumTo);
        if (!data.Any())
        {
            return Ok("出力データがありません。");
        }

        return RenderCsv(data, "期間指定請求書リスト.csv");
    }

    private IActionResult RenderCsv(List<string> dataList, string fileName)
    {
        if (!dataList.Any())
        {
            return Content(@"");
        }
        var csv = new StringBuilder();

        string contentType = "text/csv";

        foreach (var row in dataList)
        {
            csv.AppendLine(row);
        }
        var content = Encoding.UTF8.GetBytes(csv.ToString());
        var result = Encoding.UTF8.GetPreamble().Concat(content).ToArray();
        return File(result, contentType, fileName);
    }
}
