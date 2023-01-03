using UseCase.Document;

namespace EmrCloudApi.Responses.Document.Dto;

public class DocCategoryDto
{
    public DocCategoryDto(DocCategoryItem item)
    {
        CategoryCd = item.CategoryCd;
        CategoryName = item.CategoryName;
        SortNo = item.SortNo;
    }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public int SortNo { get; private set; }
}
