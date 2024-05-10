using Helper.Constants;

namespace EmrCloudApi.Requests.Reception;

public class GetReceptionPagingListRequest
{
    public int SinDate { get; set; }

    public long RaiinNo { get; set; } = CommonConstants.InvalidId;

    public long PtId { get; set; } = CommonConstants.InvalidId;

    public bool IsGetFamily { get; set; } = false;

    public int IsDeleted { get; set; }

    public bool SearchSameVisit { get; set; } = false;

    public int Limit { get; set; }

    public int Offset { get; set; }
}
