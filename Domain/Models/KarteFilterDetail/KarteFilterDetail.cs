namespace Domain.Models.KarteFilterDetail;

public class KarteFilterDetail
{
    public KarteFilterDetail(int hpId, int userId, long filterId, int filterItemCd, int filterEdaNo, int val, string param)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterItemCd = filterItemCd;
        FilterEdaNo = filterEdaNo;
        Val = val;
        Param = param;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public int FilterItemCd { get; private set; }

    public int FilterEdaNo { get; private set; }

    public int Val { get; private set; }

    public string Param { get; private set; }

}
