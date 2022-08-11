using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter;

public class KarteFilterDetailOutputItem : IOutputData
{
    public KarteFilterDetailOutputItem(int hpId, int userId, long filterId, int filterItemCd, int filterEdaNo, int val, bool isModifiedData, string? param, int kaId, string kaName, string kaSName, int userKaId, string sname, int userSortNo)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterItemCd = filterItemCd;
        FilterEdaNo = filterEdaNo;
        Val = val;
        IsModifiedData = isModifiedData;
        Param = param;
        KaId = kaId;
        KaName = kaName;
        KaSName = kaSName;
        UserKaId = userKaId;
        Sname = sname;
        UserSortNo = userSortNo;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public int FilterItemCd { get; private set; }

    public int FilterEdaNo { get; private set; }

    public int Val { get; private set; }

    public bool IsModifiedData { get; private set; }

    public string? Param { get; private set; }

    public int KaId { get; private set; }

    public string KaName { get; private set; }

    public string KaSName { get; private set; }

    public int UserKaId { get; private set; }

    public string Sname { get; private set; }

    public int UserSortNo { get; private set; }
}
