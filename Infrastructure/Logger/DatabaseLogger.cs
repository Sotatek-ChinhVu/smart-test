using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Security.Claims;
using Helper.Constants;

namespace Infrastructure.Logger
{
    public class DatabaseLogger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DatabaseLogger(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string meta = GetMetaData();
            string content = formatter(state, exception);

            Console.Write($"{meta} {content}");
            Console.WriteLine();
        }

        private string GetMetaData()
        {
            string domain = GetDomain();
            string hpId = string.Empty;
            string userId = string.Empty;
            string departmentId = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                hpId = user.Claims.Where(c => c.Type == ParamConstant.HpId).Select(c => c.Value).SingleOrDefault() ?? string.Empty;
                userId = user.Claims.Where(c => c.Type == ParamConstant.UserId).Select(c => c.Value).SingleOrDefault() ?? string.Empty;
                departmentId = user.Claims.Where(c => c.Type == ParamConstant.DepartmentId).Select(c => c.Value).SingleOrDefault() ?? string.Empty;
            }
            return $"{domain}-{hpId}-{userId}";
        }

        private string GetDomain()
        {
            string domainHeader = GetDomainFromHeader();
            if (!string.IsNullOrEmpty(domainHeader))
            {
                return domainHeader;
            }
            return GetDomainFromQueryString();
        }

        private string GetDomainFromHeader()
        {
            var headers = _httpContextAccessor?.HttpContext?.Request?.Headers;
            if (headers == null || !headers.ContainsKey(ParamConstant.Domain))
            {
                return string.Empty;
            }
            string? clientDomain = headers[ParamConstant.Domain];

            return clientDomain ?? string.Empty;
        }

        private string GetDomainFromQueryString()
        {
            var queryString = _httpContextAccessor?.HttpContext?.Request?.QueryString.Value;
            if (string.IsNullOrEmpty(queryString) || !queryString.Contains(ParamConstant.Domain))
            {
                return string.Empty;
            }

            var clientDomain = SubStringToGetParam(queryString);

            return clientDomain ?? string.Empty;
        }

        private string SubStringToGetParam(string queryString)
        {
            try
            {
                var indexStart = queryString.IndexOf(ParamConstant.Domain);
                var indexSub = indexStart > 0 ? indexStart + 7 : 0;
                var tempIndexEnd = queryString.IndexOf("&", indexStart);
                var indexEndSub = indexStart > 0 ? tempIndexEnd == -1 ? queryString.Length : tempIndexEnd : 0;
                var length = indexEndSub > indexSub ? indexEndSub - indexSub : 0;
                return queryString.Substring(indexSub, length);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
