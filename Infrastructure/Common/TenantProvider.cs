using Devart.Data.PostgreSql;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Data;
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

        public string GetStagingConnection()
        {
            return "User Id=postgres;password=Emr!23456789;Host=develop-smartkarte-postgres.ckthopedhq8w.ap-northeast-1.rds.amazonaws.com;Database=smartkarte;SSH Host=ec2-18-177-121-22.ap-northeast-1.compute.amazonaws.com;SSH User=ec2-user;SSH Private Key=C:\\Users\\vantr\\OneDrive\\Desktop\\develop-smartkarte-basion.pem;SSH Authentication Type=PublicKey;";
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
            return new TenantNoTrackingDataContext(GetStagingConnection(), true);
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
