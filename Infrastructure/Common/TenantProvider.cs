using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetConnectionString()
        {
            ///var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            return "host=localhost;port=5432;database=Emr;user id=postgres;password=Emr!23";
        }

        public TenantDataContext GetDataContext()
        {
            return new TenantDataContext(GetConnectionString());
        }

        public string GetTenantInfo()
        {
            return "Tenant1";
        }
    }
}
