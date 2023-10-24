using EmrCloudApi.Requests.MainMenu.RequestItem;

namespace EmrCloudApi.Requests.MainMenu;

public class GetKensaIraiByListRequest
{
    public List<KensaIraiByListRequestItem> KensaIraiByListRequest { get; set; } = new();
}
