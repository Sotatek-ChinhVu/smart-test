using ClosedXML.Excel;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using Microsoft.AspNetCore.Mvc;
using Reporting.Mappers.Common;
using Reporting.ReportServices;
using Reporting.Structs;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ExcelCreatorController : ControllerBase
{
    private readonly IReportService _reportService;

    public ExcelCreatorController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet(ApiPath.P24WelfareDisk)]
    public IActionResult GenerateKarte1Report([FromQuery] P24WelfareDiskRequest request)
    {
        SeikyuType seikyuType = new SeikyuType(request.IsNormal, request.IsPaper, request.IsDelay, request.IsHenrei, request.IsOnline);
        var data = _reportService.GetDataP24WelfareDisk(request.HpId, request.SeikyuYm, seikyuType);
        return RenderExcel(data);
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