namespace EmrCloudApi.Requests.MstItem.RequestItem;

public class RenkeiTimingRequestItem
{
    public long Id { get; set; }

    public string EventCd { get; set; } = string.Empty;

    public int IsInvalid { get; set; }

    public bool IsDeleted { get; set; }
}
