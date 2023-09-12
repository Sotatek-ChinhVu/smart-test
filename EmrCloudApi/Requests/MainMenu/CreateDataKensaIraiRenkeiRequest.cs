using EmrCloudApi.Requests.MainMenu.RequestItem;

namespace EmrCloudApi.Requests.MainMenu;

public class CreateDataKensaIraiRenkeiRequest
{
    public List<KensaIraiRequestItem> KensaIraiList { get; set; } = new();

    public string CenterCd { get; set; } = string.Empty;

    public int SystemDate { get; set; }
}
