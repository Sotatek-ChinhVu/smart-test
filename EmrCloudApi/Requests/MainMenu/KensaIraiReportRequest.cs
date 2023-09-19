using EmrCloudApi.Requests.MainMenu.RequestItem;

namespace EmrCloudApi.Requests.MainMenu;

public class KensaIraiReportRequest
{
    public string CenterCd { get; set; } = string.Empty;

    public int SystemDate { get; set; }

    public int FromDate { get; set; }

    public int ToDate { get; set; }

    public List<KensaIraiReportRequestItem> KensaIraiList { get; set; } = new();
}
