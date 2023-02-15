using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveListReceCmtRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveListReceCmtRequestItem> ReceCmtList { get; set; } = new();
}
