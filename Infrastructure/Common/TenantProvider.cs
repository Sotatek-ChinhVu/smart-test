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
            Console.WriteLine("Start to get connection string");
            string dbSample = _configuration["TenantDbSample"] ?? string.Empty;
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers == null || !headers.ContainsKey("domain"))
            {
                Console.WriteLine("Header doesn't contain the domain key.");
                return dbSample;
            }

            string? clientDomain = headers["domain"];
            if (string.IsNullOrEmpty(clientDomain))
            {
                Console.WriteLine("ClientDomain is empty.");
                return dbSample;
            }

            if (clientDomain == "smartkarte.sotatek.works")
            {
                return "host=develop-smartkarte-postgres.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com;port=5432;database=smartkarte;user id=postgres;password=Emr!23456789";
            }
            else if (clientDomain == "uat-tenant.smartkarte.org")
            {
                return "host=develop-smartkarte-postgres.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com;port=5432;database=smartkarte_new;user id=postgres;password=Emr!23456789";
            }
            else
            {
                return dbSample;
            }

            //var domainList = _configuration.GetSection("DomainList");
            //if (domainList == null || !domainList.Key.Contains(clientDomain))
            //{
            //    Console.WriteLine("Domain list is incorrect.");
            //    Console.WriteLine(clientDomain);
            //    if (domainList != null)
            //    {
            //        Console.WriteLine("DomainList value: " + domainList.Value);
            //    }
            //    else
            //    {
            //        Console.WriteLine("DomainList is null");
            //    }
                
            //    return dbSample;
            //}
            
            //string result = domainList[clientDomain] ?? string.Empty;
            //Console.WriteLine("Start to get connection string: " + result);

            //return result;
        }

        public string GetClinicID()
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers == null || !headers.ContainsKey("domain"))
            {
                return TempIdentity.ClinicID;
            }

            string? clientDomain = headers["domain"];
            if (string.IsNullOrEmpty(clientDomain))
            {
                return TempIdentity.ClinicID;
            }
            return clientDomain;
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
