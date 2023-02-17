using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveReceCheckCmtListRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveReceCheckCmtListRequestItem> ReceCheckCmtList { get; set; } = new();
}
