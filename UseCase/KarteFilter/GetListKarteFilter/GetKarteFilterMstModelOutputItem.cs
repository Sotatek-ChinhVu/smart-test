using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterMstModelOutputItem : IOutputData
{
    public GetKarteFilterMstModelOutputItem(List<GetKarteFilterDetailOutputItem> karteFilterDetailModels, int hpId, int userId, long filterId, string filterName, string filterNameRevert, int autoApply, int isDeleted)
    {
        KarteFilterDetailModels = karteFilterDetailModels;
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        FilterNameRevert = filterNameRevert;
        AutoApply = autoApply;
        IsDeleted = isDeleted;
    }
    
    public List<GetKarteFilterDetailOutputItem> KarteFilterDetailModels { get; private set; }
    
    public int HpId { get; private set; }
    
    public int UserId { get; private set; }
    
    public long FilterId { get; private set; }
    
    public string FilterName { get; private set; }
    
    public string FilterNameRevert { get; private set; } = string.Empty;
    
    public int AutoApply { get; private set; }
    
    public int IsDeleted { get; private set; }
}
