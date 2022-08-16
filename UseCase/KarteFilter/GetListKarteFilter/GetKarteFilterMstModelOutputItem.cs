using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterMstModelOutputItem : IOutputData
{
    public GetKarteFilterMstModelOutputItem(GetKarteFilterDetailOutputItem karteFilterDetailModel, int hpId, int userId, long filterId, string filterName, int autoApply, int isDeleted)
    {
        KarteFilterDetailModel = karteFilterDetailModel;
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        AutoApply = autoApply;
        IsDeleted = isDeleted;
    }
    
    public GetKarteFilterDetailOutputItem KarteFilterDetailModel { get; private set; }
    
    public int HpId { get; private set; }
    
    public int UserId { get; private set; }
    
    public long FilterId { get; private set; }
    
    public string FilterName { get; private set; }
    
    public int AutoApply { get; private set; }
    
    public int IsDeleted { get; private set; }
}
