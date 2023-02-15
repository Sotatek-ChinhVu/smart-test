using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveListSyoukiInfRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveListSyoukiInfRequestItem> SyoukiInfList { get; set; } = new();
}
