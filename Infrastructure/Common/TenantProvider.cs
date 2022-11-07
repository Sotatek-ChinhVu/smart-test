﻿using Helper.Constants;
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

        public string GetTenantId()
        {
            return TempIdentity.TenantId;
        }

        public TenantNoTrackingDataContext GetNoTrackingDataContext()
        {
            return new TenantNoTrackingDataContext(GetConnectionString());
        }

        private TenantDataContext? _trackingTenantDataContext;
        public TenantDataContext GetTrackingTenantDataContext()
        {
            if (_trackingTenantDataContext == null)
            {
                _trackingTenantDataContext = new TenantDataContext(GetConnectionString());
            }
            return _trackingTenantDataContext;
        }

        public string GetTenantInfo()
        {
            return "Tenant1";
        }
    }
}
