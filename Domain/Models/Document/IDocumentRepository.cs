namespace Domain.Models.Document;

public interface IDocumentRepository
{
    List<DocCategoryModel> GetAllDocCategory(int hpId);

    DocCategoryModel GetDocCategoryDetail(int hpId, int categoryCd);

    bool SaveListDocCategory(int hpId, int userId, List<DocCategoryModel> listModels);

    bool CheckExistDocCategory(int hpId, int categoryCd);

    bool CheckDuplicateCategoryName(int hpId, int categoryCd, string categoryName);
}
