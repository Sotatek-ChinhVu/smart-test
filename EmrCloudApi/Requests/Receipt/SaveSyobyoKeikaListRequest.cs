using EmrCloudApi.Requests.Receipt.RequestItem;

namespace EmrCloudApi.Requests.Receipt;

public class SaveSyobyoKeikaListRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public List<SaveSyobyoKeikaRequestItem> SyobyoKeikaList { get; set; } = new();
}
