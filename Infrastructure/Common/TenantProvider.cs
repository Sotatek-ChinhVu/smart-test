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
            return "host=develop-smartkarte-postgres.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com;port=5432;database=smartkarte;user id=postgres;password=Emr!23456789";
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
