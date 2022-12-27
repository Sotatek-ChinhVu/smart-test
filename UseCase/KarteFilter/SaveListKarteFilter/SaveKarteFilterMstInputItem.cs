using System.ComponentModel.DataAnnotations;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterMstInputItem
{
    public SaveKarteFilterMstInputItem(long filterId, string filterName, int sortNo, int autoApply, int isDeleted, SaveKarteFilterDetailInputItem karteFilterDetailModel)
    {
        FilterId = filterId;
        FilterName = filterName;
        SortNo = sortNo;
        AutoApply = autoApply;
        IsDeleted = isDeleted;
        KarteFilterDetailModel = karteFilterDetailModel;
    }
    public long FilterId { get; private set; }

    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }

    public int IsDeleted { get; private set; }

    public SaveKarteFilterDetailInputItem KarteFilterDetailModel { get; private set; }
}
