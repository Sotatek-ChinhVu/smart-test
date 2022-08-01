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

    private KaMstModel ToModel(KaMst k)
    {
        return new KaMstModel(
            k.Id,
            k.KaId,
            k.SortNo,
            k.ReceKaCd,
            k.KaSname,
            k.KaName,
            k.IsDeleted);
    }
}
