namespace Domain.Models.Document;

public class DocCategoryModel
{
    public DocCategoryModel(int hpId, int categoryCd, string categoryName, int sortNo)
    {
        HpId = hpId;
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        SortNo = sortNo;
    }
    public DocCategoryModel()
    {
        HpId = 0;
        CategoryCd = 0;
        CategoryName = string.Empty;
        SortNo = 0;
    }

    public int HpId { get; private set; }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public int SortNo { get; private set; }
}
