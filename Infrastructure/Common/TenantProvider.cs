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
            return "User Id=postgres;password=Emr!23456789;Host=develop-smartkarte-postgres.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com;Database=smartkarte;SSH Host=ec2-18-177-121-22.ap-northeast-1.compute.amazonaws.com;SSH User=ec2-user;SSH Private Key=C:\\Users\\vantr\\OneDrive\\Desktop\\develop-smartkarte-basion.pem;SSH Authentication Type=PublicKey;";
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
                var options = new DbContextOptionsBuilder<TenantNoTrackingDataContext>()
                     .UsePostgreSql(GetConnectionString())
                     .LogTo(Console.WriteLine, LogLevel.Information).Options;
                _noTrackingDataContext = new TenantNoTrackingDataContext(options);
            }
            return _noTrackingDataContext;
        }

        private TenantDataContext? _trackingDataContext;
        public TenantDataContext GetTrackingTenantDataContext()
        {
            if (_trackingDataContext == null)
            {
                var options = new DbContextOptionsBuilder<TenantDataContext>()
                    .UsePostgreSql(GetConnectionString())
                    .LogTo(Console.WriteLine, LogLevel.Information).Options;

                _trackingDataContext = new TenantDataContext(options);
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

        public TenantNoTrackingDataContext CreateNewNoTrackingDataContext()
        {
            var options = new DbContextOptionsBuilder<TenantNoTrackingDataContext>().UseNpgsql(GetConnectionString(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            }).LogTo(Console.WriteLine, LogLevel.Information).Options;
            var factory = new PooledDbContextFactory<TenantNoTrackingDataContext>(options);
            return factory.CreateDbContext();
        }

        public void DisposeDataContext()
        {
            _trackingDataContext?.Dispose();
            _noTrackingDataContext?.Dispose();
        }
    }
}
