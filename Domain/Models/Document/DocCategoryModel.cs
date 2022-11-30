namespace Domain.Models.Document;

public class DocCategoryModel
{
    public DocCategoryModel(int categoryCd, string categoryName, int sortNo)
    {
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        SortNo = sortNo;
        IsDelete = false;
    }
    public DocCategoryModel()
    {
        CategoryCd = 0;
        CategoryName = string.Empty;
        SortNo = 0;
        IsDelete = false;
    }

    // constructor for save data
    public DocCategoryModel(int categoryCd, string categoryName, int sortNo, bool isDelete)
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
