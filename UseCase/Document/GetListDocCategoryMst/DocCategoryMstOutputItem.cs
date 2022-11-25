namespace UseCase.Document.GetListDocCategoryMst;

public class DocCategoryMstOutputItem
{
    public DocCategoryMstOutputItem(int hpId, int categoryCd, string categoryName, int sortNo)
    {
        HpId = hpId;
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        SortNo = sortNo;
    }

    public int HpId { get; private set; }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public int SortNo { get; private set; }
}
