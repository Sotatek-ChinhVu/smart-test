using Domain.Models.KarteFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class KarteFilterMstModelOutputItem : IOutputData
{
    public KarteFilterMstModelOutputItem(List<KarteFilterDetailOutputItem> karteFilterDetailModels, int hpId, int userId, long filterId, string filterName, string filterNameRevert, int autoApply)
    {
        KarteFilterDetailModels = karteFilterDetailModels;
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        FilterNameRevert = filterNameRevert;
        AutoApply = autoApply;
    }

    public List<KarteFilterDetailOutputItem> KarteFilterDetailModels { get; private set; }
    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public long FilterId { get; private set; }
    public string FilterName { get; private set; }
    public string FilterNameRevert { get; private set; } = string.Empty;
    public int AutoApply { get; private set; }
}
