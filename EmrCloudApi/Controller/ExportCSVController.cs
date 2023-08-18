using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportCsv;
using EmrCloudApi.Services;
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
    public ExportCSVController(UseCaseBus bus, IUserService userService, IReportService reportService) : base(userService)
    {
        _bus = bus;
        _reportService = reportService;
    }

    [HttpPost(ApiPath.ReceListCsv)]
    public IActionResult GenerateKarteCsvReport([FromBody] ExportCsvRequest request)
    {
        var data = _reportService.OutputExcelForPeriodReceipt(HpId, request.StartDate, request.EndDate,
                                                              request.PtConditions.Select(p => new Tuple<long, int>(p.PtId, p.HokenId)).ToList(),
                                                              request.GrpConditions.Select(p => new Tuple<int, string>(p.GrpId, p.GrpCd)).ToList(),
                                                              request.Sort, request.MiseisanKbn, request.SaiKbn, request.MisyuKbn, request.SeikyuKbn, request.HokenKbn);
        return RenderCsv(data, "期間指定請求書リスト.csv");
    }

    private IActionResult RenderCsv(List<string> dataList, string fileName)
    {
        if (!dataList.Any())
        {
            return Content(@"
            <meta charset=""utf-8"">
            <title>印刷対象が見つかりません。</title>
            <p style='text-align: center;font-size: 25px;font-weight: 300'>印刷対象が見つかりません。</p>
            ", "text/html");
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
