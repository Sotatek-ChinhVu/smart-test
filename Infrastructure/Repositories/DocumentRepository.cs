using Domain.Models.Document;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class DocumentRepository : RepositoryBase, IDocumentRepository
{
    public DocumentRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public bool CheckExistDocCategory(int hpId, int categoryCd)
    {
        return NoTrackingDataContext.DocCategoryMsts.Any(item => item.HpId == hpId && item.IsDeleted == 0 && item.CategoryCd == categoryCd);
    }

    public bool CheckDuplicateCategoryName(int hpId, int categoryCd, string categoryName)
    {
        return NoTrackingDataContext.DocCategoryMsts.Any(
                                                        item => item.HpId == hpId
                                                        && item.CategoryCd != categoryCd
                                                        && item.CategoryName != null
                                                        && item.IsDeleted == 0
                                                        && item.CategoryName.Equals(categoryName));
    }

    public List<DocCategoryModel> GetAllDocCategory(int hpId)
    {
        var listCategoryDB = NoTrackingDataContext.DocCategoryMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(item => item.SortNo).ToList();
        return listCategoryDB.Select(item => ConvertToDocCategoryModel(item)).ToList();
    }

    public List<DocInfModel> GetAllDocInf(int hpId, long ptId)
    {
        var listDocCategory = GetAllDocCategory(hpId);
        var listDocDB = NoTrackingDataContext.DocInfs
                                                                .Where(item => item.HpId == hpId
                                                                            && item.IsDeleted == 0
                                                                            && item.PtId == ptId)
                                                                .OrderByDescending(x => x.SinDate)
                                                                .ThenBy(x => x.UpdateDate)
                                                                .ToList();
        return listDocDB.Select(item => ConvertToDocInfModel(item, listDocCategory)).ToList();
    }

    public DocCategoryModel GetDocCategoryDetail(int hpId, int categoryCd)
    {
        var categoryDB = NoTrackingDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.CategoryCd == categoryCd && item.IsDeleted == 0);
        if (categoryDB != null)
        {
            return ConvertToDocCategoryModel(categoryDB);
        }
        return new DocCategoryModel();
    }

    public bool SaveListDocCategory(int hpId, int userId, List<DocCategoryModel> listModels)
    {
        // add new item
        var listAddNews = listModels
                                .Where(item => item.CategoryCd == 0)
                                .Select(model => ConvertToNewDocCategoryMst(hpId, userId, model))
                                .ToList();
        TrackingDataContext.DocCategoryMsts.AddRange(listAddNews);

        // update item
        var listUpdateModels = listModels
                                .Where(item => item.CategoryCd > 0)
                                .ToList();
        var listUpdateCd = listUpdateModels.Select(item => item.CategoryCd).ToList();
        var listDocUpdateDB = TrackingDataContext.DocCategoryMsts.Where(entity => listUpdateCd.Contains(entity.CategoryCd)).ToList();
        foreach (var entity in listDocUpdateDB)
        {
            var modelUpdate = listUpdateModels.FirstOrDefault(model => model.CategoryCd == entity.CategoryCd);
            if (modelUpdate != null)
            {
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateId = userId;
                entity.CategoryName = modelUpdate.CategoryName;
                entity.SortNo = modelUpdate.SortNo;
            }
        }
        SortDocCategory();
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<DocInfModel> GetDocInfByCategoryCd(int hpId, long ptId, int categoryCd)
    {
        var docCategory = GetDocCategoryDetail(hpId, categoryCd);
        var listDocDB = TrackingDataContext.DocInfs
                                                            .Where(item => item.HpId == hpId
                                                                        && item.IsDeleted == 0
                                                                        && item.CategoryCd == categoryCd
                                                                        && item.PtId == ptId)
                                                            .OrderByDescending(x => x.SinDate)
                                                            .ThenBy(x => x.UpdateDate)
                                                            .ToList();
        return listDocDB.Select(item => ConvertToDocInfModel(item, new List<DocCategoryModel> { docCategory })).ToList();
    }

    public bool SortDocCategory(int hpId, int userId, int moveInCd, int moveOutCd)
    {
        // get in DB
        var moveInItem = TrackingDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.IsDeleted == 0 && item.CategoryCd == moveInCd);
        var moveOutItem = TrackingDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.IsDeleted == 0 && item.CategoryCd == moveOutCd);

        // change sortNo
        if (moveInItem != null && moveOutItem != null)
        {
            int moveInSortNo = moveInItem.SortNo;
            int moveOutSortNo = moveOutItem.SortNo;

            // move in item
            moveInItem.SortNo = moveOutSortNo;
            moveInItem.UpdateDate = DateTime.UtcNow;
            moveInItem.UpdateId = userId;

            // move out item
            moveOutItem.SortNo = moveInSortNo;
            moveOutItem.UpdateDate = DateTime.UtcNow;
            moveOutItem.UpdateId = userId;
            TrackingDataContext.SaveChanges();
            return true;
        }
        return false;
    }

    public DocInfModel GetDocInfDetail(int hpId, long ptId, int sinDate, long raiinNo, int seqNo)
    {
        var docInfDB = NoTrackingDataContext.DocInfs.FirstOrDefault(entity =>
                                                                entity.HpId == hpId
                                                                && entity.PtId == ptId
                                                                && entity.SinDate == sinDate
                                                                && entity.RaiinNo == raiinNo
                                                                && entity.SeqNo == seqNo
                                                                && entity.IsDeleted == 0
                                                            );
        if (docInfDB == null)
        {
            return new DocInfModel();
        }
        var docCategory = GetDocCategoryDetail(hpId, docInfDB.CategoryCd);
        return ConvertToDocInfModel(docInfDB, new List<DocCategoryModel> { docCategory });
    }

    public bool SaveDocInf(int userId, DocInfModel model)
    {
        if (model.SeqNo <= 0)
        {
            TrackingDataContext.DocInfs.Add(ConvertToNewDocInf(userId, model));
        }
        else
        {
            var docInfDB = TrackingDataContext.DocInfs.FirstOrDefault(entity =>
                                                                entity.HpId == model.HpId
                                                                && entity.PtId == model.PtId
                                                                && entity.SinDate == model.SinDate
                                                                && entity.RaiinNo == model.RaiinNo
                                                                && entity.SeqNo == model.SeqNo
                                                                && entity.IsDeleted == 0
                                                            );
            if (docInfDB == null)
            {
                return false;
            }
            docInfDB.CategoryCd = model.CategoryCd;
            docInfDB.DspFileName = model.DisplayFileName;
            docInfDB.UpdateDate = DateTime.UtcNow;
            docInfDB.UpdateId = userId;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool DeleteDocInf(int hpId, int userId, long ptId, int sinDate, long raiinNo, int seqNo)
    {
        var docInfDB = TrackingDataContext.DocInfs.FirstOrDefault(entity =>
                                                                entity.HpId == hpId
                                                                && entity.PtId == ptId
                                                                && entity.SinDate == sinDate
                                                                && entity.RaiinNo == raiinNo
                                                                && entity.SeqNo == seqNo
                                                                && entity.IsDeleted == 0
                                                            );
        if (docInfDB == null)
        {
            return false;
        }
        docInfDB.IsDeleted = 1;
        docInfDB.UpdateDate = DateTime.UtcNow;
        docInfDB.UpdateId = userId;
        TrackingDataContext.SaveChanges();
        return true;
    }

    public bool DeleteDocInfs(int hpId, int userId, long ptId, int categoryCd)
    {
        var docInfsDB = TrackingDataContext.DocInfs.Where(entity =>
                                                                entity.HpId == hpId
                                                                && entity.PtId == ptId
                                                                && entity.CategoryCd == categoryCd
                                                                && entity.IsDeleted == 0
                                                            );
        if (docInfsDB.Any())
        {
            foreach (var item in docInfsDB)
            {
                item.IsDeleted = 1;
                item.UpdateDate = DateTime.UtcNow;
                item.UpdateId = userId;
            }
            return TrackingDataContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool DeleteDocCategory(int hpId, int userId, int categoryCd)
    {
        var docCategoryDB = TrackingDataContext.DocCategoryMsts.FirstOrDefault(entity =>
                                                                entity.HpId == hpId
                                                                && entity.CategoryCd == categoryCd
                                                                && entity.IsDeleted == 0
                                                            );
        if (docCategoryDB == null)
        {
            return false;
        }
        docCategoryDB.IsDeleted = 1;
        docCategoryDB.UpdateDate = DateTime.UtcNow;
        docCategoryDB.UpdateId = userId;
        SortDocCategory();
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool MoveDocInf(int hpId, int userId, int categoryCd, int moveCategoryCd)
    {
        var listDocInfs = TrackingDataContext.DocInfs.Where(item =>
                                                                item.HpId == hpId
                                                                && item.CategoryCd == categoryCd
                                                                && item.IsDeleted == 0)
                                                            .ToList();
        foreach (var item in listDocInfs)
        {
            item.CategoryCd = moveCategoryCd;
            item.UpdateDate = DateTime.UtcNow;
            item.UpdateId = userId;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<DocCommentModel> GetListDocComment(List<string> listReplaceWord)
    {
        var query = NoTrackingDataContext.DocComments.Where(item => item.IsDeleted == 0);
        if (listReplaceWord.Any())
        {
            query = query.Where(item => item.ReplaceWord != null && listReplaceWord.Contains(item.ReplaceWord));
        }
        var listDocComments = query.ToList();
        var listDocCommentDetails = GetListDocCommentDetail(listDocComments.Select(item => item.CategoryId).ToList());
        return listDocComments.OrderBy(item => item.SortNo)
                    .Select(item => ConvertToDocCommentModel(item, listDocCommentDetails))
                    .ToList();
    }

    #region private function
    private DocCategoryModel ConvertToDocCategoryModel(DocCategoryMst entity)
    {
        return new DocCategoryModel(
                entity.CategoryCd,
                entity.CategoryName ?? string.Empty,
                entity.SortNo
            );
    }

    private DocCategoryMst ConvertToNewDocCategoryMst(int hpId, int userId, DocCategoryModel model)
    {
        DocCategoryMst entity = new();
        entity.HpId = hpId;
        entity.CategoryCd = model.CategoryCd;
        entity.CategoryName = model.CategoryName;
        entity.SortNo = model.SortNo;
        entity.UpdateDate = DateTime.UtcNow;
        entity.UpdateId = userId;
        entity.IsDeleted = 0;
        entity.CreateDate = DateTime.UtcNow;
        entity.CreateId = userId;
        return entity;
    }

    private DocInf ConvertToNewDocInf(int userId, DocInfModel model)
    {
        DocInf entity = new();
        entity.HpId = model.HpId;
        entity.PtId = model.PtId;
        entity.SinDate = model.SinDate;
        entity.RaiinNo = model.RaiinNo;
        entity.SeqNo = GetLastDocInfSeqNo(model.HpId, model.PtId, model.SinDate, model.RaiinNo) + 1;
        entity.CategoryCd = model.CategoryCd;
        entity.FileName = model.FileName;
        entity.DspFileName = model.DisplayFileName;
        entity.UpdateDate = DateTime.UtcNow;
        entity.UpdateId = userId;
        entity.IsDeleted = 0;
        entity.CreateDate = DateTime.UtcNow;
        entity.CreateId = userId;
        return entity;
    }

    private DocInfModel ConvertToDocInfModel(DocInf entity, List<DocCategoryModel> listDocCategory)
    {
        return new DocInfModel(
                entity.HpId,
                entity.PtId,
                entity.SinDate,
                entity.RaiinNo,
                entity.SeqNo,
                entity.CategoryCd,
                listDocCategory.FirstOrDefault(item => item.CategoryCd == entity.CategoryCd)?.CategoryName ?? string.Empty,
                entity.FileName ?? string.Empty,
                entity.DspFileName ?? string.Empty,
                entity.UpdateDate
            );
    }

    private int GetLastDocInfSeqNo(int hpId, long ptId, int sinDate, long raiinNo)
    {
        var listDocInf = NoTrackingDataContext.DocInfs.Where(entity =>
                                                                entity.HpId == hpId
                                                                && entity.PtId == ptId
                                                                && entity.SinDate == sinDate
                                                                && entity.RaiinNo == raiinNo
                                                            ).ToList();
        return listDocInf.Any() ? listDocInf.Max(item => item.SeqNo) : 0;
    }

    private void SortDocCategory()
    {
        var listDocCategory = TrackingDataContext.DocCategoryMsts
                                                    .Where(item => item.IsDeleted == 0)
                                                    .OrderBy(item => item.SortNo)
                                                    .ToList();
        int sortNo = 1;
        foreach (var item in listDocCategory)
        {
            item.SortNo = sortNo;
            sortNo++;
        }
    }

    private List<DocCommentDetailModel> GetListDocCommentDetail(List<int> listCategoryId)
    {
        var query = NoTrackingDataContext.DocCommentDetails.Where(item => item.IsDeleted == 0);
        if (listCategoryId.Any())
        {
            query = query.Where(item => listCategoryId.Contains(item.CategoryId));
        }
        return query.OrderBy(item => item.SortNo)
                    .Select(item => new DocCommentDetailModel(item.CategoryId,
                                                              item.Comment ?? string.Empty))
                    .ToList();
    }

    private DocCommentModel ConvertToDocCommentModel(DocComment docComment, List<DocCommentDetailModel> listDocCommentDetails)
    {
        var listDocCommentModels = listDocCommentDetails.Where(detail => detail.CategoryId == docComment.CategoryId).ToList();
        return new DocCommentModel(docComment.CategoryId,
                                   docComment.CategoryName ?? string.Empty,
                                   docComment.ReplaceWord ?? string.Empty,
                                   listDocCommentModels);
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
    #endregion
}
