﻿using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportCsv;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Services;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
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
    public IActionResult GenerateExportStatics([FromQuery] ExportCsvStaticsRequest request)
    {
        var data = _reportService.ExportCsv(HpId, request.MenuName, request.MenuId, request.TimeFrom, request.TimeTo, request.MonthFrom, request.MonthTo, request.DateFrom, request.DateTo, 
                                            request.IsPutTotalRow, request.TenkiDateFrom, request.TenkiDateTo, request.EnableRangeFrom, request.EnableRangeTo, request.PtNumFrom, request.PtNumTo, request.IsPutColName, request.CoFileType);
        if (!data.Data.Any())
        {
            return Ok("EndNoData");
        }
        return RenderCsvStatics(data);
    }

    [HttpPost(ApiPath.ExportSta9000Csv)]
    public IActionResult ExportSta9000Csv([FromBody] ExportCsvSta9000Request request)
    {
        var data = _reportService.OutPutFileSta900(HpId, request.OutputColumns, request.IsPutColName);
        if (data.code == CoPrintExitCode.EndNoData && !string.IsNullOrEmpty(data.message))
        {
            return Ok("EndNoData");
        }
        else if (data.code == CoPrintExitCode.EndError)
        {
            return Ok("EndError");
        }
        else if (data.code == CoPrintExitCode.EndFormFileNotFound)
        {
            return Ok("EndFormFileNotFound");
        }
        else if (data.code == CoPrintExitCode.EndTemplateNotFound)
        {
            return Ok("EndTemplateNotFound");

        }
        else if (data.code == CoPrintExitCode.EndInvalidArg)
        {
            return Ok("EndInvalidArg");
        }

        return RenderCsv(data.data, request.OutputFileName);
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

    private IActionResult RenderCsvStatics(CommonExcelReportingModel dataModel)
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
