using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveReceCmtListRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveReceCmtRequestItem> ReceCmtList { get; set; } = new();
}
