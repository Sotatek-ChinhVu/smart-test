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
            //var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            return "host=192.168.1.70;port=5432;database=EmrYamamoto;user id=postgres;password=Emr!23";
        }

        public TenantNoTrackingDataContext GetDataContext()
        {
            return new TenantNoTrackingDataContext(GetConnectionString());
        }
    }
}
