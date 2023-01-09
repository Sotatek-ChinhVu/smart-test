using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public string GetTenantId()
        {
            return TempIdentity.TenantId;
        }

        private TenantNoTrackingDataContext? _noTrackingDataContext;
        public TenantNoTrackingDataContext GetNoTrackingDataContext()
        {
            if (_noTrackingDataContext == null)
            {
                var options = new DbContextOptionsBuilder<TenantNoTrackingDataContext>().UseNpgsql(GetConnectionString(), buider =>
                {
                    buider.EnableRetryOnFailure(maxRetryCount: 3);
                }).LogTo(Console.WriteLine, LogLevel.Information).Options;
                var factory = new PooledDbContextFactory<TenantNoTrackingDataContext>(options);
                _noTrackingDataContext = factory.CreateDbContext();
            }
            return _noTrackingDataContext;
        }

        private TenantDataContext? _trackingDataContext;
        public TenantDataContext GetTrackingTenantDataContext()
        {
            if (_trackingDataContext == null)
            {
                var options = new DbContextOptionsBuilder<TenantDataContext>().UseNpgsql(GetConnectionString(), buider =>
                {
                    buider.EnableRetryOnFailure(maxRetryCount: 3);
                }).LogTo(Console.WriteLine, LogLevel.Information).Options;
                var factory = new PooledDbContextFactory<TenantDataContext>(options);
                _trackingDataContext = factory.CreateDbContext();
            }
            return _trackingDataContext;
        }

        public string GetTenantInfo()
        {
            return "Tenant1";
        }

        public TenantNoTrackingDataContext ReloadNoTrackingDataContext()
        {
            _noTrackingDataContext?.Dispose();

            var options = new DbContextOptionsBuilder<TenantNoTrackingDataContext>().UseNpgsql(GetConnectionString(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            }).LogTo(Console.WriteLine, LogLevel.Information).Options;
            var factory = new PooledDbContextFactory<TenantNoTrackingDataContext>(options);
            _noTrackingDataContext = factory.CreateDbContext();
            return _noTrackingDataContext;
        }

        public TenantDataContext ReloadTrackingDataContext()
        {
            _trackingDataContext?.Dispose();

            var options = new DbContextOptionsBuilder<TenantDataContext>().UseNpgsql(GetConnectionString(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            }).LogTo(Console.WriteLine, LogLevel.Information).Options;
            var factory = new PooledDbContextFactory<TenantDataContext>(options);
            _trackingDataContext = factory.CreateDbContext();
            return _trackingDataContext;
        }

        public TenantDataContext CreateNewTrackingDataContext()
        {
            var options = new DbContextOptionsBuilder<TenantDataContext>().UseNpgsql(GetConnectionString(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            }).LogTo(Console.WriteLine, LogLevel.Information).Options;
            var factory = new PooledDbContextFactory<TenantDataContext>(options);
            return factory.CreateDbContext();
        }

        public void DisposeDataContext()
        {
            _trackingDataContext?.Dispose();
            _noTrackingDataContext?.Dispose();
        }
    }
}
