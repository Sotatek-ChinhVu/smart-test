namespace EmrCloudApi.Requests.MainMenu.RequestItem;

public class KensaIraiDetailReportRequestItem
{
    public long RpNo { get; set; }

    public long RpEdaNo { get; set; }

    public int RowNo { get; set; }

    public int SeqNo { get; set; }

    public string KensaItemCd { get; set; } = string.Empty;

    public string CenterItemCd { get; set; } = string.Empty;

    public string KensaKana { get; set; } = string.Empty;

    public string KensaName { get; set; } = string.Empty;

    public int ContainerCd { get; set; }
}
