namespace UseCase.Document.SaveListDocCategory;

public class SaveListDocCategoryInputItem
{
    public SaveListDocCategoryInputItem(int categoryCd, string categoryName, int sortNo, bool isDelete)
    {
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        SortNo = sortNo;
        IsDelete = isDelete;
    }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDelete { get; private set; }
}
