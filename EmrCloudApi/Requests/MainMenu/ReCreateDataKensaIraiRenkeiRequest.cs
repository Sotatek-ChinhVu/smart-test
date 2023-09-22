using EmrCloudApi.Requests.MainMenu.RequestItem;

namespace EmrCloudApi.Requests.MainMenu;

public class ReCreateDataKensaIraiRenkeiRequest
{
    public List<KensaIraiRequestItem> KensaIraiList { get; set; } = new();

    public int SystemDate { get; set; }
}
