using Domain.Models.Document;
using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetDocCategoryDetail;

public class GetDocCategoryDetailOutputData : IOutputData
{
    public GetDocCategoryDetailOutputData(DocCategoryItem docCategory, List<FileDocumentModel> listTemplates, GetDocCategoryDetailStatus status)
    {
        DocCategory = docCategory;
        ListTemplates = listTemplates;
        Status = status;
    }

    public GetDocCategoryDetailOutputData(GetDocCategoryDetailStatus status)
    {
        DocCategory = new();
        ListTemplates = new();
        Status = status;
    }

    public DocCategoryItem DocCategory { get; private set; }

    public List<FileDocumentModel> ListTemplates { get; private set; }

    public GetDocCategoryDetailStatus Status { get; private set; }
}
