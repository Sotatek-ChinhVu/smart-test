namespace EmrCloudApi.Requests.MainMenu.RequestItem;

public class KensaIraiRequestItem
{
    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public long IraiCd { get; set; }

    public long PtId { get; set; }

    public long PtNum { get; set; }

    public string Name { get; set; } = string.Empty;

    public string KanaName { get; set; } = string.Empty;

    public int Sex { get; set; }

    public int Birthday { get; set; }

    public int TosekiKbn { get; set; }

    public int SikyuKbn { get; set; }

    public List<KensaIraiDetailRequestItem> KensaIraiDetails { get; set; } = new();
}
