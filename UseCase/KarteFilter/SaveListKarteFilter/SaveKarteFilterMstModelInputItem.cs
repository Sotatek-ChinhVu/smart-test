namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterMstModelInputItem
{
    public SaveKarteFilterMstModelInputItem(int hpId, int userId, long filterId, string filterName, int sortNo, int autoApply, int isDeleted, SaveKarteFilterDetailModelInputItem karteFilterDetailModel)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        SortNo = sortNo;
        AutoApply = autoApply;
        IsDeleted = isDeleted;
        this.karteFilterDetailModel = karteFilterDetailModel;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }

    public int IsDeleted { get; private set; }

    public SaveKarteFilterDetailModelInputItem karteFilterDetailModel { get; private set; }
}
