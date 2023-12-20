using Domain.Common;
using Domain.Models.ReleasenoteRead;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositoriesp;

public class ReleasenoteReadRepository : RepositoryBase, IReleasenoteReadRepository
{
    public ReleasenoteReadRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }
    public List<string> GetListReleasenote(int hpId, int userId)
    {
        List<ReleasenoteRead> releasenote = NoTrackingDataContext.ReleasenoteReads.Where(u => u.HpId == hpId && u.UserId == userId).ToList();
        if (releasenote == null)
        {
            return new List<string>();
        }

        var result = new List<string>();

        foreach (var version in releasenote.Select(u => u.Version))
        {
            result.Add(version.Substring(0, 2) + "." + version.Substring(2, 2) + "." + version.Substring(4, 2) + "." + version.Substring(6, 2) + "." + version.Substring(8, 2));
        }

        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
