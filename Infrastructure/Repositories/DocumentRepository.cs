using Domain.Models.Document;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;

    public DocumentRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public bool CheckExistDocCategory(int hpId, int categoryCd)
    {
        return _tenantDataContext.DocCategoryMsts.Any(item => item.HpId == hpId && item.IsDeleted == 0 && item.CategoryCd == categoryCd);
    }

    public bool CheckDuplicateCategoryName(int hpId, int categoryCd, string categoryName)
    {
        return _tenantDataContext.DocCategoryMsts.Any(
                                                        item => item.HpId == hpId
                                                        && item.CategoryCd != categoryCd
                                                        && item.CategoryName != null
                                                        && item.IsDeleted == 0
                                                        && item.CategoryName.Equals(categoryName));
    }

    public List<DocCategoryModel> GetAllDocCategory(int hpId)
    {
        var listCategoryDB = _tenantNoTrackingDataContext.DocCategoryMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(item => item.SortNo).ToList();
        return listCategoryDB.Select(item => ConvertToDocCategoryModel(item)).ToList();
    }

    public List<DocInfModel> GetAllDocInf(int hpId, long ptId)
    {
        var listDocCategory = GetAllDocCategory(hpId);
        var listDocDB = _tenantNoTrackingDataContext.DocInfs
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
        var categoryDB = _tenantNoTrackingDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.CategoryCd == categoryCd && item.IsDeleted == 0);
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
        _tenantDataContext.DocCategoryMsts.AddRange(listAddNews);

        // update item
        var listUpdateModels = listModels
                                .Where(item => item.CategoryCd > 0)
                                .ToList();
        var listUpdateCd = listUpdateModels.Select(item => item.CategoryCd).ToList();
        var listDocUpdateDB = _tenantDataContext.DocCategoryMsts.Where(entity => listUpdateCd.Contains(entity.CategoryCd)).ToList();
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
        _tenantDataContext.SaveChanges();
        return true;
    }

    public List<DocInfModel> GetDocInfByCategoryCd(int hpId, long ptId, int categoryCd)
    {
        var docCategory = GetDocCategoryDetail(hpId, categoryCd);
        var listDocDB = _tenantNoTrackingDataContext.DocInfs
                                                            .Where(item => item.HpId == hpId
                                                                        && item.IsDeleted == 0
                                                                        && item.CategoryCd == categoryCd
                                                                        && item.PtId == ptId)
                                                            .OrderByDescending(x => x.SinDate)
                                                            .ThenBy(x => x.UpdateDate)
                                                            .ToList();
        return listDocDB.Select(item => ConvertToDocInfModel(item, new List<DocCategoryModel> { docCategory })).ToList();
    }

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

    public bool SortDocCategory(int hpId, int userId, int moveInCd, int moveOutCd)
    {
        // get in DB
        var moveInItem = _tenantDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.IsDeleted == 0 && item.CategoryCd == moveInCd);
        var moveOutItem = _tenantDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.IsDeleted == 0 && item.CategoryCd == moveOutCd);

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
            _tenantDataContext.SaveChanges();
            return true;
        }
        return false;
    }

    public bool DeleteDocCategory(int hpId, int userId, int catgoryCd)
    {
        // delete this item
        var itemDelete = _tenantDataContext.DocCategoryMsts.FirstOrDefault(item => item.HpId == hpId && item.CategoryCd == catgoryCd);
        if (itemDelete != null)
        {
            itemDelete.UpdateDate = DateTime.UtcNow;
            itemDelete.UpdateId = userId;
            itemDelete.IsDeleted = 1;

            // update sortNo other item
            var listUpdateSortNo = _tenantDataContext.DocCategoryMsts
                                                                    .Where(item =>
                                                                                item.HpId == hpId
                                                                                && item.IsDeleted == 0
                                                                                && item.SortNo > itemDelete.SortNo
                                                                    ).ToList();
            foreach (var item in listUpdateSortNo)
            {
                item.SortNo = item.SortNo - 1;
                item.UpdateDate = DateTime.UtcNow;
                item.UpdateId = userId;
            }
        }
        _tenantDataContext.SaveChanges();
        return true;
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
                entity.FileName,
                entity.DspFileName,
                entity.UpdateDate
            );
    }
}
