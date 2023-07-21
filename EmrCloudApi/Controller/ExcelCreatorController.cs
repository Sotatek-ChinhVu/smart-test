using ClosedXML.Excel;
using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using Entity.Tenant;
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
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        using (var workbook = new XLWorkbook())
        {
            IXLWorksheet worksheet =
            workbook.Worksheets.Add("shift_jis");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "FirstName";
            worksheet.Cell(1, 3).Value = "LastName";
            for (int index = 1; index <= dataList.Count; index++)
            {
                worksheet.Cell(index + 1, 1).Value = dataList[index - 1];
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