using Helper.Constants;
using Infrastructure.Common;
using Helper.Redis;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using PostgreDataContext;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using StackExchange.Redis;
using Helper.Extension;
using System.IdentityModel.Tokens.Jwt;
using Helper.Common;

namespace Infrastructure.CommonDB
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _cache;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        private void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public string GetConnectionString()
        {
            string dbSample = _configuration["TenantDb"] ?? string.Empty;
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
            return clientDomainInConfig;
        }

        public string GetAdminConnectionString()
        {
            string dbSample = _configuration["AdminDatabase"] ?? string.Empty;
            return dbSample;
        }

        public string GetDomainName()
        {
            var domain = GetDomainFromHeader();
            domain = string.IsNullOrEmpty(domain) ? GetDomainFromQueryString() : domain;
            return string.IsNullOrEmpty(domain) ? TempIdentity.ClinicID : domain;
        }

        public int GetTenantId()
        {
            // get domain then get tenantId from Tenant table
            string domain = GetDomainName();
            string key = "cache_tenantId_" + domain;
            int tenantId = 0;
            if (_cache.KeyExists(key))
            {
                return _cache.StringGet(key).AsInteger();
            }
            var superAdminNoTrackingDataContext = CreateNewSuperAdminNoTrackingDataContext();
            tenantId = superAdminNoTrackingDataContext.Tenants.FirstOrDefault(item => item.SubDomain == domain)?.TenantId ?? 0;
            _cache.StringSet(key, tenantId.ToString());
            return tenantId;
        }

        #region Expose data
        private string _clientIp = string.Empty;
        private string _domain = string.Empty;
        private string _requestInfo = string.Empty;
        private int _hpId;
        private int _userId;
        private int _departmentId;

        public async Task<string> GetRequestInfoAsync()
        {
            if (!string.IsNullOrEmpty(_requestInfo))
            {
                return _requestInfo;
            }

            var request = _httpContextAccessor.HttpContext.Request;
            string method = request.Method;
            string path = request.Path;
            string body = await GetRawBodyAsync(request);
            string query = request.QueryString.ToString();

            RequestInfo requestInfo = new RequestInfo(method, path, "body-data-key", query);

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            _requestInfo = JsonSerializer.Serialize<RequestInfo>(requestInfo, options);
            _requestInfo = _requestInfo.Replace("body-data-key", body);
            return _requestInfo;
        }

        private async Task<string> GetRawBodyAsync(HttpRequest request)
        {
            string body = string.Empty;
            if (request.ContentType != null)
            {
                if (request.ContentType.StartsWith("text/") || request.ContentType == "application/json")
                {
                    // Leave the body open so the next middleware can read it.
                    using (var reader = new StreamReader(
                                            request.Body,
                                            encoding: Encoding.UTF8,
                                            detectEncodingFromByteOrderMarks: false,
                                            leaveOpen: true))
                    {
                        body = await reader.ReadToEndAsync();

                        // Reset the request body stream position so the next middleware can read it
                        request.Body.Position = 0;
                    }
                }
                else
                {
                    body = request.ContentType;
                }
            }
            return body;
        }

        public int GetHpId()
        {
            if (_hpId != 0)
            {
                return _hpId;
            }
            string hpId = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                hpId = user.Claims.Where(c => c.Type == ParamConstant.HpId).Select(c => c.Value).SingleOrDefault() ?? string.Empty;
            }
            return int.TryParse(hpId, out _hpId) ? _hpId : 0;
        }

        public int GetUserId()
        {
            if (_userId != 0)
            {
                return _userId;
            }
            string userId = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                userId = user.Claims.Where(c => c.Type == ParamConstant.UserId).Select(c => c.Value).SingleOrDefault() ?? string.Empty;
            }
            return int.TryParse(userId, out _userId) ? _userId : 0;
        }

        public int GetDepartmentId()
        {
            if (_departmentId != 0)
            {
                return _departmentId;
            }
            string departmentId = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                departmentId = user.Claims.Where(c => c.Type == ParamConstant.DepartmentId).Select(c => c.Value).SingleOrDefault() ?? string.Empty;
            }
            return int.TryParse(departmentId, out _departmentId) ? _departmentId : 0;
        }

        public string GetClientIp()
        {
            if (!string.IsNullOrEmpty(_clientIp))
            {
                return _clientIp;
            }
            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            _clientIp = clientIp.ToString();
            return _clientIp;
        }

        public string GetDomain()
        {
            if (!string.IsNullOrEmpty(_domain))
            {
                return _domain;
            }
            _domain = GetDomainFromHeader();
            if (!string.IsNullOrEmpty(_domain))
            {
                return _domain;
            }
            _domain = GetDomainFromQueryString();
            return _domain;
        }

        #endregion

        #region Domain handler


        public string GetDomainFromHeader()
        {
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.Domain))
            {
                return string.Empty;
            }
            string? clientDomain = headers[ParamConstant.Domain];

            return clientDomain ?? string.Empty;
        }

        public string GetLoginKeyFromHeader()
        {
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.LoginKey))
            {
                return string.Empty;
            }
            string? loginkey = headers[ParamConstant.LoginKey];

            return loginkey ?? string.Empty;
        }

        public string GetDomainFromQueryString()
        {
            var queryString = _httpContextAccessor.HttpContext?.Request?.QueryString.Value ?? string.Empty;

            // get domain from param
            string clientDomain = SubStringToGetParam(queryString);

            // get domain from cookie
            if (string.IsNullOrEmpty(clientDomain))
            {
                return GetDomainFromCookie();
            }

            return clientDomain;
        }

        /// <summary>
        /// Get domain from cookie
        /// </summary>
        public string GetDomainFromCookie()
        {
            string cookieValue = _httpContextAccessor.HttpContext?.Request?.Cookies[DomainCookie.CookieReportKey] ?? string.Empty;

            if (!string.IsNullOrEmpty(cookieValue))
            {
                var cookie = JsonSerializer.Deserialize<CookieModel>(cookieValue);
                if (cookie == null)
                {
                    return string.Empty;
                }
                var jwtToken = new JwtSecurityToken(cookie.Token);
                if (jwtToken.ValidFrom < DateTime.UtcNow && jwtToken.ValidTo > DateTime.UtcNow)
                {
                    return cookie.Domain;
                }
            }
            return string.Empty;
        }

        public string SubStringToGetParam(string queryString)
        {
            try
            {
                var indexStart = queryString.IndexOf(ParamConstant.Domain);
                var indexSub = indexStart > 0 ? indexStart + 7 : 0;
                var tempInedexEnd = queryString.IndexOf("&", indexStart);
                var indexEndSub = 0;
                if (indexStart > 0)
                {
                    if (tempInedexEnd == -1)
                    {
                        indexEndSub = queryString.Length;
                    }
                    else
                    {
                        indexEndSub = tempInedexEnd;
                    }
                }
                var length = indexEndSub > indexSub ? indexEndSub - indexSub : 0;
                return queryString.Substring(indexSub, length);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

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

        private DbContextOptions? _dbAdminContextOptions;
        public DbContextOptions GetAdminTrackingDbContextOption()
        {
            if (_dbAdminContextOptions == null)
            {
                _dbAdminContextOptions = CreateNewTrackingAdminDbContextOption();
            }
            return _dbAdminContextOptions;
        }

        private SuperAdminNoTrackingContext? _superAdminNoTrackingDataContext;
        public SuperAdminNoTrackingContext GetSuperAdminNoTrackingDataContext()
        {
            if (_superAdminNoTrackingDataContext == null)
            {
                _superAdminNoTrackingDataContext = CreateNewSuperAdminNoTrackingDataContext();
            }
            return _superAdminNoTrackingDataContext;
        }

        private SuperAdminContext? _superAdminTrackingDataContext;
        public SuperAdminContext GetSuperAdminTrackingTenantDataContext()
        {
            if (_superAdminTrackingDataContext == null)
            {
                _superAdminTrackingDataContext = CreateNewSuperAdminTrackingDataContext();
            }
            return _superAdminTrackingDataContext;
        }

        public TenantDataContext CreateNewTrackingDataContext()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<TenantDataContext>().UseNpgsql(GetConnectionString(), buider =>
                    {
                        buider.EnableRetryOnFailure(maxRetryCount: 3);
                    })
                    //.UseLoggerFactory(loggerFactory)
                    .Options;
            var factory = new PooledDbContextFactory<TenantDataContext>(options);
            return factory.CreateDbContext();
        }

        public TenantNoTrackingDataContext CreateNewNoTrackingDataContext()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<TenantNoTrackingDataContext>().UseNpgsql(GetConnectionString(), buider =>
                {
                    buider.EnableRetryOnFailure(maxRetryCount: 3);
                })
                //.UseLoggerFactory(loggerFactory)
                .Options;
            var factory = new PooledDbContextFactory<TenantNoTrackingDataContext>(options);
            return factory.CreateDbContext();
        }

        public SuperAdminContext CreateNewSuperAdminTrackingDataContext()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<SuperAdminContext>().UseNpgsql(GetConnectionStringForSuperAdmin(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            })
                    //.UseLoggerFactory(loggerFactory)
                    .Options;
            var factory = new PooledDbContextFactory<SuperAdminContext>(options);
            return factory.CreateDbContext();
        }

        public SuperAdminNoTrackingContext CreateNewSuperAdminNoTrackingDataContext()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<SuperAdminNoTrackingContext>().UseNpgsql(GetConnectionStringForSuperAdmin(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            })
                //.UseLoggerFactory(loggerFactory)
                .Options;
            var factory = new PooledDbContextFactory<SuperAdminNoTrackingContext>(options);
            return factory.CreateDbContext();
        }

        public DbContextOptions CreateNewTrackingAdminDbContextOption()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<AdminDataContext>().UseNpgsql(GetAdminConnectionString(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            })
                    //.UseLoggerFactory(loggerFactory)
                    .Options;
            return options;
        }

        public string GetConnectionStringForSuperAdmin()
        {
            string dbSample = _configuration["SuperAdminDb"] ?? string.Empty;
            return dbSample;
        }

        #endregion

        public void DisposeDataContext()
        {
            _trackingDataContext?.Dispose();
            _noTrackingDataContext?.Dispose();
            _superAdminNoTrackingDataContext?.Dispose();
            _superAdminTrackingDataContext?.Dispose();
        }

        public AdminDataContext CreateNewAuditLogTrackingDataContext()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<AdminDataContext>().UseNpgsql(GetConnectionStringForAuditLog(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            })
                    //.UseLoggerFactory(loggerFactory)
                    .Options;
            var factory = new PooledDbContextFactory<AdminDataContext>(options);
            return factory.CreateDbContext();
        }

        public AdminNoTrackingContext CreateNewAuditLogNoTrackingDataContext()
        {
            //ILoggerFactory loggerFactory = new LoggerFactory(new[] { new DatabaseLoggerProvider(_httpContextAccessor) });
            var options = new DbContextOptionsBuilder<AdminNoTrackingContext>().UseNpgsql(GetConnectionStringForAuditLog(), buider =>
            {
                buider.EnableRetryOnFailure(maxRetryCount: 3);
            })
                //.UseLoggerFactory(loggerFactory)
                .Options;
            var factory = new PooledDbContextFactory<AdminNoTrackingContext>(options);
            return factory.CreateDbContext();
        }

        private AdminNoTrackingContext? _auditLogNoTrackingDataContext;
        public AdminNoTrackingContext GetAuditLogNoTrackingDataContext()
        {
            if (_auditLogNoTrackingDataContext == null)
            {
                _auditLogNoTrackingDataContext = CreateNewAuditLogNoTrackingDataContext();
            }
            return _auditLogNoTrackingDataContext;
        }

        private AdminDataContext? _auditLogTrackingDataContext;
        public AdminDataContext GetAuditLogTrackingDataContext()
        {
            if (_auditLogTrackingDataContext == null)
            {
                _auditLogTrackingDataContext = CreateNewAuditLogTrackingDataContext();
            }
            return _auditLogTrackingDataContext;
        }

        public string GetConnectionStringForAuditLog()
        {
            string dbSample = _configuration["AuditLogDb"] ?? string.Empty;
            return dbSample;
        }
    }
}
