﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Requests.ExportPDF;
using Interactor.ExportPDF;
using Interactor.ExportPDF.Karte2;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ExportReportController : ControllerBase
{
    private readonly IReporting _reporting;

    public ExportReportController(IReporting reporting)
    {
        _reporting = reporting;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public IActionResult GetListKarte1([FromQuery] Karte1ExportRequest request)
    {
        var output = _reporting.PrintKarte1(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei);
        return File(output.ToArray(), "application/pdf");
    }


    [HttpGet(ApiPath.ExportKarte2)]
    public IActionResult GetListKarte2([FromQuery] Karte2ExportRequest request)
    {
        var output = _reporting.PrintKarte2(ConvertToKarte2ExportInput(request));
        return File(output.ToArray(), "application/pdf");
    }

    private Karte2ExportInput ConvertToKarte2ExportInput(Karte2ExportRequest request)
    {
        return new Karte2ExportInput(
                request.PtId,
                request.HpId,
                request.UserId,
                request.SinDate,
                request.StartDate,
                request.EndDate,
                request.IsCheckedHoken,
                request.IsCheckedJihi,
                request.IsCheckedHokenJihi,
                request.IsCheckedJihiRece,
                request.IsCheckedHokenRousai,
                request.IsCheckedHokenJibai,
                request.IsCheckedDoctor,
                request.IsCheckedStartTime,
                request.IsCheckedVisitingTime,
                request.IsCheckedEndTime,
                request.IsUketsukeNameChecked,
                request.IsCheckedSyosai,
                request.IsIncludeTempSave,
                request.IsCheckedApproved,
                request.IsCheckedInputDate,
                request.IsCheckedSetName,
                request.DeletedOdrVisibilitySetting,
                request.IsIppanNameChecked,
                request.IsCheckedHideOrder
            );
    }
}