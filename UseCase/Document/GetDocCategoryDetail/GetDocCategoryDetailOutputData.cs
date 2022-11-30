using Domain.Models.Document;
using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetDocCategoryDetail;

public class GetDocCategoryDetailOutputData : IOutputData
{
    public GetDocCategoryDetailOutputData(GetDocCategoryDetailStatus status)
    {
        DocCategory = new();
        ListTemplates = new();
        DocInfs = new();
        Status = status;
    }

    public GetDocCategoryDetailOutputData(DocCategoryItem docCategory, List<FileDocumentModel> listTemplates, List<DocInfModel> docInfs, GetDocCategoryDetailStatus status)
    {
        DocCategory = docCategory;
        ListTemplates = listTemplates;
        DocInfs = docInfs;
        Status = status;
    }

    public DocCategoryItem DocCategory { get; private set; }

    public List<FileDocumentModel> ListTemplates { get; private set; }

    public List<DocInfModel> DocInfs { get; private set; }

    public GetDocCategoryDetailStatus Status { get; private set; }
}
