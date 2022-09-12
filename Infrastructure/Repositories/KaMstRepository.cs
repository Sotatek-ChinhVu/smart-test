using Domain.Models.KaMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KaMstRepository : IKaMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public KaMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public KaMstModel? GetByKaId(int kaId)
    {
        var entity = _tenantDataContext.KaMsts
            .Where(k => k.KaId == kaId && k.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? null : ToModel(entity);
    }

    public List<KaMstModel> GetByKaIds(List<int> kaIds)
    {
        var entities = _tenantDataContext.KaMsts
           .Where(k => kaIds.Contains(k.KaId) && k.IsDeleted == DeleteTypes.None).AsEnumerable();
        return entities is null ? new List<KaMstModel>() : entities.Select(e => ToModel(e)).ToList();
    }

    public List<KaMstModel> GetList()
    {
        return _tenantDataContext.KaMsts
            .Where(k => k.IsDeleted == DeleteTypes.None)
            .OrderBy(k => k.SortNo).AsEnumerable()
            .Select(k => ToModel(k)).ToList();
    }

    private KaMstModel ToModel(KaMst k)
    {
        return new KaMstModel(
            k.Id,
            k.KaId,
            k.SortNo,
            k.ReceKaCd ?? string.Empty,
            k.KaSname ?? string.Empty,
            k.KaName ?? string.Empty,
            k.IsDeleted);
    }
}
