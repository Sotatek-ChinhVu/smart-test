using EmrCloudApi.Requests.MstItem.RequestItem;

namespace EmrCloudApi.Requests.MstItem;

public class SaveRenkeiRequest
{
    public List<RenkeiTab> RenkeiTabList { get; set; } = new();
}

public class RenkeiTab
{
    public int RenkeiSbt { get; set; }

    public List<RenkeiConfRequestItem> RenkeiConfList { get; set; } = new();
}