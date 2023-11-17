using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SuperAdminModels.Tenant
{
    public interface ITenantRepository
    {
        TenantModel Get(int tenantId);
        int GetBySubDomainAndIdentifier(string subDomain, string Identifier);
        int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier);
        int CreateTenant(TenantModel model);
        bool UpdateStatusTenant(int tenantId, byte status, string endSubDomain, string endPoint);
        void ReleaseResource();
    }
}
