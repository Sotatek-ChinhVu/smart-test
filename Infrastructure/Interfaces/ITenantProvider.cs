using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Interfaces
{
    public interface ITenantProvider
    {
        string GetConnectionString();

        string GetTenantInfo();

        string GetClinicID();

        TenantNoTrackingDataContext GetNoTrackingDataContext();

        TenantDataContext GetTrackingTenantDataContext();

        TenantNoTrackingDataContext ReloadNoTrackingDataContext();

        TenantDataContext ReloadTrackingDataContext();

        TenantDataContext CreateNewTrackingDataContext();

        TenantNoTrackingDataContext CreateNewNoTrackingDataContext();

        void DisposeDataContext();

        string GetDomainFromQueryString();

        string GetDomainFromHeader();

        string GetDomain();

        string GetClientIp();

        int GetHpId();

        int GetUserId();

        int GetDepartmentId();

        Task<string> GetRequestInfoAsync();

        string GetAdminConnectionString();

        DbContextOptions CreateNewTrackingAdminDbContextOption();

        string GetLoginKeyFromHeader();
    }
}
