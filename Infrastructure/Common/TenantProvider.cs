using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PostgreDataContext;

namespace Infrastructure.CommonDB
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public TenantProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration["TenantDbSample"];
        }

        public string GetClinicID()
        {
            return TempIdentity.ClinicID;
        }

        private TenantNoTrackingDataContext? _noTrackingDataContext;
        public TenantNoTrackingDataContext GetNoTrackingDataContext()
        {
            if (_noTrackingDataContext == null)
            {
                _noTrackingDataContext = new TenantNoTrackingDataContext(GetConnectionString());
            }
            return _noTrackingDataContext;
        }

        private TenantDataContext? _trackingDataContext;
        public TenantDataContext GetTrackingTenantDataContext()
        {
            if (_trackingDataContext == null)
            {
                _trackingDataContext = new TenantDataContext(GetConnectionString());
            }
            return _trackingDataContext;
        }

        public string GetTenantInfo()
        {
            return "Tenant1";
        }

        public void ReloadNoTrackingDataContext()
        {
            _noTrackingDataContext?.Dispose();
            _noTrackingDataContext = new TenantNoTrackingDataContext(GetConnectionString());
        }

        public void ReloadTrackingDataContext()
        {
            _trackingDataContext?.Dispose();
            _trackingDataContext = new TenantDataContext(GetConnectionString());
        }

        public void DisposeDataContext()
        {
            _trackingDataContext?.Dispose();
            _noTrackingDataContext?.Dispose();
        }
    }
}
