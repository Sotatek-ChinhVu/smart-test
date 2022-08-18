using System.ComponentModel.DataAnnotations;

namespace UseCase.KarteFilter.SaveListKarteFilter;

public class SaveKarteFilterMstInputItem
{
    public SaveKarteFilterMstInputItem(int hpId, int userId, long filterId, string filterName, int sortNo, int autoApply, int isDeleted, SaveKarteFilterDetailInputItem karteFilterDetailModel)
    {
        HpId = hpId;
        UserId = userId;
        FilterId = filterId;
        FilterName = filterName;
        SortNo = sortNo;
        AutoApply = autoApply;
        IsDeleted = isDeleted;
        KarteFilterDetailModel = karteFilterDetailModel;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FilterId { get; private set; }

    [MaxLength(20)]
    public string FilterName { get; private set; }

    public int SortNo { get; private set; }

    public int AutoApply { get; private set; }

    public int IsDeleted { get; private set; }

    public SaveKarteFilterDetailInputItem KarteFilterDetailModel { get; private set; }
}
