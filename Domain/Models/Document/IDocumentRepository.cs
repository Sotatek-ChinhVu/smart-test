namespace Domain.Models.Document;

public interface IDocumentRepository
{
    List<DocCategoryMstModel> GetAllDocCategory(int hpId);
}
