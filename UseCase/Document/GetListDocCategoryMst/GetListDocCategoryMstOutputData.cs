using Domain.Models.Document;
using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocCategoryMst;

public class GetListDocCategoryMstOutputData : IOutputData
{
    public GetListDocCategoryMstOutputData(GetListDocCategoryMstStatus status)
    {
        ListDocCategories = new();
        ListTemplates = new();
        Status = status;
    }

    public GetListDocCategoryMstOutputData(List<DocCategoryMstOutputItem> listDocCategories, List<FileDocumentModel> listTemplateName, GetListDocCategoryMstStatus status)
    {
        ListDocCategories = listDocCategories;
        ListTemplates = listTemplateName;
        Status = status;
    }

    public List<DocCategoryMstOutputItem> ListDocCategories { get; private set; }

    public List<FileDocumentModel> ListTemplates { get; private set; }

    public GetListDocCategoryMstStatus Status { get; private set; }

}
