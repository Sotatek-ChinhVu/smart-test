namespace Domain.Models.Document;

public interface IDocumentRepository
{
    List<DocCategoryModel> GetAllDocCategory(int hpId);

    DocCategoryModel GetDocCategoryDetail(int hpId, int categoryCd);

    bool CheckExistDocCategory(int hpId, int categoryCd);
}
