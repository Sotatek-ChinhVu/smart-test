namespace EmrCloudApi.Requests.MainMenu.RequestItem;

public class DeleteKensaInfRequestItem
{
    public long IraiCd { get; set; }

    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public List<DeleteKensaInfDetailRequestItem> KensaInfDetailList { get; set; } = new();
}
