using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveListSyobyoKeikaRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveListSyobyoKeikaRequestItem> SyobyoKeikaList { get; set; } = new();
}
