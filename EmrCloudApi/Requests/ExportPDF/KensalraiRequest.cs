﻿namespace EmrCloudApi.Requests.ExportPDF;

public class KensalraiRequest : ReportRequestBase
{
    public int SystemDate { get; set; }

    public int FromDate { get; set; }

    public int ToDate { get; set; }

    public string CenterCd { get; set; } = string.Empty;
}
