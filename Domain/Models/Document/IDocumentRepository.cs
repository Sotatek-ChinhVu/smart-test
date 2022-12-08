namespace Domain.Models.Document;

public interface IDocumentRepository
{
    List<DocCategoryModel> GetAllDocCategory(int hpId);

    List<DocInfModel> GetAllDocInf(int hpId, long ptId);

    DocCategoryModel GetDocCategoryDetail(int hpId, int categoryCd);

    bool SaveListDocCategory(int hpId, int userId, List<DocCategoryModel> listModels);

    bool SortDocCategory(int hpId, int userId, int moveInCd, int moveOutCd);

    List<DocInfModel> GetDocInfByCategoryCd(int hpId, long ptId, int categoryCd);

    bool CheckExistDocCategory(int hpId, int categoryCd);

    bool CheckDuplicateCategoryName(int hpId, int categoryCd, string categoryName);

    DocInfModel GetDocInfDetail(int hpId, long ptId, int sinDate, long raiinNo, int seqNo);

    bool SaveDocInf(int userId, DocInfModel model);

    bool DeleteDocInf(int hpId, int userId, long ptId, int sinDate, long raiinNo, int seqNo);
}
