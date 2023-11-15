using Domain.SuperAdminModels.Logger;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SuperAdminRepositories;

public class AuditLogRepository : RepositoryBase, IAuditLogRepository
{
    public AuditLogRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<AuditLogModel> GetAuditLogList(AuditLogModel requestModel)
    {
        List<AuditLogModel> result = new();
        if (requestModel == null)
        {

        }
         result = NoTrackingDataContex .Where(p => EF.Functions.ToTsVector("english", p.Title + " " + p.Description)
        .Matches("Npgsql"))
    .ToList();

        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
