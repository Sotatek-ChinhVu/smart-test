namespace EmrCloudApi.Requests.MainMenu.RequestItem;

public class KensaIraiByListRequestItem
{
    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public string CenterCd { get; set; } = string.Empty;

    public int PrimaryKbn { get; set; }

    public long IraiCd { get; set; }
}
