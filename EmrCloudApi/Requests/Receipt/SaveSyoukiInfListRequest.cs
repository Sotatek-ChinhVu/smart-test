using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveSyoukiInfListRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveSyoukiInfRequestItem> SyoukiInfList { get; set; } = new();
}
