using Reporting.ReceiptPrint.Service;
using System.Collections.Generic;

namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptPrintRequest
{
    public int PrefNo { get; set; }

    public int ReportId { get; set; }

    public int ReportEdaNo { get; set; }

    public int DataKbn { get; set; }

    public long PtId { get; set; }

    public int SeikyuYm { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public int DiskKind { get; set; }

    public int DiskCnt { get; set; }

    public int WelfareType { get; set; }

    public string FormName { get; set; } = string.Empty;

    public List<string> PrintHokensyaNos { get; set; } = new();

    public int HokenKbn { get; set; }

    public ReseputoShubetsuModel SelectedReseputoShubeusu { get; set; } = new();

    public int DepartmentId { get; set; }

    public int DoctorId { get; set; }

    public int PrintNoFrom { get; set; }

    public int PrintNoTo { get; set; }

    public bool IsIncludeOutDrug { get; set; } = false;

    public bool IncludeTester { get; set; } = false;

    public int Sort { get; set; }

    public List<long> PrintPtIds { get; set; } = new();

    public int PrintType { get; set; }
}
