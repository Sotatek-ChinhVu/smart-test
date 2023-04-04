using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveReceCheckOptRequest
{
    public List<SaveReceCheckOptRequestItem> ReceCheckOptList { get; set; } = new();
}
