using Domain.Models.KarteFilterDetail;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KarteFilterDetailRepository : IKarteFilterDetailRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public KarteFilterDetailRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<KarteFilterDetailModel> GetList(int hpId, int userId)
    {
        var result = _tenantDataContext.KarteFilterDetails
                                 .Where(u => u.HpId == hpId && u.UserId == userId)
                                 .Select(u => new KarteFilterDetailModel(
                                        u.HpId,
                                        u.UserId,
                                        u.FilterId,
                                        u.FilterItemCd,
                                        u.FilterEdaNo,
                                        u.Val,
                                        u.Param
                                     ))
                                 .ToList();
        return result;
    }
}
