using EmrCloudApi.Requests.MainMenu.RequestItem;

namespace EmrCloudApi.Requests.MainMenu;

public class DeleteKensaInfRequest
{
    public List<DeleteKensaInfRequestItem> KensaInfList { get; set; } = new();
}
