using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TenantNoTrackingDataContext GetNoTrackingDataContext()
        {
            return new TenantNoTrackingDataContext(GetConnectionString());
        }

        public TenantDataContext GetTrackingTenantDataContext()
        {
            return new TenantDataContext(GetConnectionString());
        }
    }
}
