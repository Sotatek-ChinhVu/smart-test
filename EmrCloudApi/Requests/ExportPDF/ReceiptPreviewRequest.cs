﻿using System.Security.Principal;

namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptPreviewRequest : ReportRequestBase
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int SeiKyuYm { get; set; }

    public int HokenId { get; set; }

    public int HokenKbn { get; set; }

    public bool IsIncludeOutDrug { get; set; }

    public bool isOpenedFromAccounting { get; set; } = false;
}
