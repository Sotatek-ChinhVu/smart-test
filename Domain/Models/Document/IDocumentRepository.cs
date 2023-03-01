using Domain.Common;

namespace Domain.Models.Document;

public interface IDocumentRepository : IRepositoryBase
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

    bool SaveDocInf(int userId, DocInfModel model, bool overwriteFile);

    bool DeleteDocInf(int hpId, int userId, long ptId, int sinDate, long raiinNo, int seqNo);

    bool DeleteDocInfs(int hpId, int userId, long ptId, int categoryCd);

    bool DeleteDocCategory(int hpId, int userId, int categoryCd);

    bool MoveDocInf(int hpId, int userId, int categoryCd, int moveCategoryCd);

    List<DocCommentModel> GetListDocComment(List<string> listReplaceWord);
}
