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
