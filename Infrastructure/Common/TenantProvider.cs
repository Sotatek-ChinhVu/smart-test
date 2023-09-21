using Helper.Constants;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
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
            string dbSample = _configuration["TenantDbSample"] ?? string.Empty;
            string clientDomain = GetDomainFromHeader();
            clientDomain = string.IsNullOrEmpty(clientDomain) ? GetDomainFromQueryString() : clientDomain;
            if (string.IsNullOrEmpty(clientDomain))
            {
                return dbSample;
            }
            var domainList = _configuration.GetSection("DomainList").Path;
            if (string.IsNullOrEmpty(domainList))
            {
                return dbSample;
            }
            var clientDomainInConfig = _configuration[domainList + ":" + clientDomain] ?? string.Empty;
            if (string.IsNullOrEmpty(clientDomainInConfig))
            {
                return dbSample;
            }
            string result = clientDomainInConfig ?? string.Empty;

            return result;
        }

        public string GetClinicID()
        {
            var domain = GetDomainFromHeader();
            domain = string.IsNullOrEmpty(domain) ? GetDomainFromQueryString() : domain;
            return string.IsNullOrEmpty(domain) ? TempIdentity.ClinicID : domain;
        }

        public string GetDomainFromHeader()
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.Domain))
            {
                return string.Empty;
            }
            string? clientDomain = headers[ParamConstant.Domain];

            return clientDomain ?? string.Empty;
        }

        public string GetDomainFromQueryString()
        {
            var queryString = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            if (string.IsNullOrEmpty(queryString) || !queryString.Contains(ParamConstant.Domain))
            {
                return string.Empty;
            }

            var clientDomain = SubStringToGetParam(queryString);

            return clientDomain ?? string.Empty;
        }

        public string SubStringToGetParam(string queryString)
        {
            try
            {
                var indexStart = queryString.IndexOf(ParamConstant.Domain);
                var indexSub = indexStart > 0 ? indexStart + 7 : 0;
                var tempInedexEnd = queryString.IndexOf("&", indexStart);
                var indexEndSub = indexStart > 0 ? tempInedexEnd == -1 ? queryString.Length : tempInedexEnd : 0;
                var length = indexEndSub > indexSub ? indexEndSub - indexSub : 0;
                return queryString.Substring(indexSub, length);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetTenantInfo()
        {
            return "Tenant1";
        }

        #region Return DataContext
        private TenantNoTrackingDataContext? _noTrackingDataContext;
        public TenantNoTrackingDataContext GetNoTrackingDataContext()
        {
            if (_noTrackingDataContext == null)
            {
                _noTrackingDataContext = CreateNewNoTrackingDataContext();
            }
            return _noTrackingDataContext;
        }

        private TenantDataContext? _trackingDataContext;
        public TenantDataContext GetTrackingTenantDataContext()
        {
            if (_trackingDataContext == null)
            {
                _trackingDataContext = CreateNewTrackingDataContext();
            }
            return _trackingDataContext;
        }

        public TenantNoTrackingDataContext ReloadNoTrackingDataContext()
        {
            _noTrackingDataContext?.Dispose();
            _noTrackingDataContext = CreateNewNoTrackingDataContext();
            return _noTrackingDataContext;
        }

        public TenantDataContext ReloadTrackingDataContext()
        {
            _trackingDataContext?.Dispose();
            _trackingDataContext = CreateNewTrackingDataContext();
            return _trackingDataContext;
        }

        public TenantDataContext CreateNewTrackingDataContext()
        {
            ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<TenantDataContext>().UseNpgsql(GetConnectionString(), buider =>
                    {
                        buider.EnableRetryOnFailure(maxRetryCount: 3);
                    })
                    .UseLoggerFactory(loggerFactory)
                    .Options;
            var factory = new PooledDbContextFactory<TenantDataContext>(options);
            return factory.CreateDbContext();
        }

        public TenantNoTrackingDataContext CreateNewNoTrackingDataContext()
        {
            ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<TenantNoTrackingDataContext>().UseNpgsql(GetConnectionString(), buider =>
                {
                    buider.EnableRetryOnFailure(maxRetryCount: 3);
                })
                .UseLoggerFactory(loggerFactory)
                .Options;
            var factory = new PooledDbContextFactory<TenantNoTrackingDataContext>(options);
            return factory.CreateDbContext();
        }

        #endregion

        public void DisposeDataContext()
        {
            _trackingDataContext?.Dispose();
            _noTrackingDataContext?.Dispose();
        }
    }
}
