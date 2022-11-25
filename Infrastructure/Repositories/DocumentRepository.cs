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
    public List<DocCategoryMstModel> GetAllDocCategory(int hpId)
    {
        var listCategoryDB = _tenantNoTrackingDataContext.DocCategoryMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).ToList();
        return listCategoryDB.Select(item => ConvertToDocCategoryMstModel(item)).ToList();
    }

    private DocCategoryMstModel ConvertToDocCategoryMstModel(DocCategoryMst entity)
    {
        return new DocCategoryMstModel(
                entity.HpId,
                entity.CategoryCd,
                entity.CategoryName ?? string.Empty,
                entity.SortNo
            );
    }
}
