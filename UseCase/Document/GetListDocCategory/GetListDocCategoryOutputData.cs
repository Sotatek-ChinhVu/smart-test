using Domain.Models.Document;
using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocCategory;

public class GetListDocCategoryOutputData : IOutputData
{
    public GetListDocCategoryOutputData(GetListDocCategoryStatus status)
    {
        ListDocCategories = new();
        ListTemplates = new();
        DocInfs = new();
        Status = status;
    }

    public GetListDocCategoryOutputData(List<DocCategoryItem> listDocCategories, List<FileDocumentModel> listTemplateName, List<DocInfModel> docInfs, GetListDocCategoryStatus status)
    {
        ListDocCategories = listDocCategories;
        ListTemplates = listTemplateName;
        DocInfs = docInfs;
        Status = status;
    }

    public List<DocCategoryItem> ListDocCategories { get; private set; }

    public List<FileDocumentModel> ListTemplates { get; private set; }

    public List<DocInfModel> DocInfs { get; private set; }

    public GetListDocCategoryStatus Status { get; private set; }

}
