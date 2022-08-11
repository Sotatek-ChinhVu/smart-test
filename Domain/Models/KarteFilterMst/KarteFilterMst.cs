namespace Domain.Models.KarteFilterMst;

public class KarteFilterMst
{
    public KarteFilterMst(int hpId, int userId, long filterId, string filterName, int sortNo, int autoApply)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        SortNo = sortNo;
        AutoApply = autoApply;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }
}
