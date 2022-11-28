using Domain.Models.Document;
using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocCategory;

public class GetListDocCategoryOutputData : IOutputData
{
    public GetListDocCategoryOutputData(GetListDocCategoryStatus status)
    {
        ListDocCategories = new();
        ListTemplates = new();
        Status = status;
    }

    public GetListDocCategoryOutputData(List<DocCategoryItem> listDocCategories, List<FileDocumentModel> listTemplateName, GetListDocCategoryStatus status)
    {
        ListDocCategories = listDocCategories;
        ListTemplates = listTemplateName;
        Status = status;
    }

    public List<DocCategoryItem> ListDocCategories { get; private set; }

    public List<FileDocumentModel> ListTemplates { get; private set; }

    public GetListDocCategoryStatus Status { get; private set; }

}
