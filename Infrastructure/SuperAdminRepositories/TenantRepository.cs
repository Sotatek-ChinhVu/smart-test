﻿using Amazon.Runtime.Internal.Util;
using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Helper.Common;
using Helper.Enum;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;
using System.Text;

namespace Infrastructure.SuperAdminRepositories
{
    public class TenantRepository : SuperAdminRepositoryBase, ITenantRepository
    {
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;

        public TenantRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        public TenantModel Get(int tenantId)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.TenantId == tenantId && t.IsDeleted == 0).FirstOrDefault();
            var tenantModel = tenant == null ? new() : ConvertEntityToModel(tenant);
            return tenantModel;
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public int GetBySubDomainAndIdentifier(string subDomain, string Identifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain == subDomain && t.RdsIdentifier == Identifier && t.IsDeleted == 0).FirstOrDefault();
            return tenant == null ? 0 : tenant.TenantId;
        }

        public int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain == subDomain && t.RdsIdentifier == dbIdentifier && t.IsDeleted == 0);
            if (tenant != null)
            {
                return tenant.Count();
            }
            return 0;
        }

        public int CreateTenant(TenantModel model)
        {
            var tenant = new Tenant();
            tenant.Hospital = model.Hospital;
            tenant.AdminId = model.AdminId;
            tenant.Password = model.Password;
            tenant.SubDomain = model.SubDomain;

            tenant.Status = 2; //Status: creating
            tenant.Db = model.Db;
            tenant.Size = model.Size;
            tenant.SizeType = model.SizeType;
            tenant.Type = model.Type;
            tenant.EndPointDb = model.SubDomain;
            tenant.EndSubDomain = model.SubDomain;
            tenant.RdsIdentifier = model.RdsIdentifier;
            tenant.IsDeleted = 0;
            tenant.CreateDate = CIUtil.GetJapanDateTimeNow();
            tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
            TrackingDataContext.Tenants.Add(tenant);
            TrackingDataContext.SaveChanges();
            return tenant.TenantId;
        }

        public bool UpdateInfTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier)
        {
            var tenant = TrackingDataContext.Tenants.FirstOrDefault(i => i.TenantId == tenantId);
            if (tenant != null)
            {
                if (!string.IsNullOrEmpty(endPointDb))
                {
                    tenant.EndPointDb = endPointDb;
                }
                if (!string.IsNullOrEmpty(endSubDomain))
                {
                    tenant.SubDomain = endSubDomain;
                }
                if (!string.IsNullOrEmpty(dbIdentifier))
                {
                    tenant.RdsIdentifier = dbIdentifier;
                }
                tenant.Status = status;
                tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
                tenant.CreateDate = TimeZoneInfo.ConvertTimeToUtc(tenant.CreateDate);
                TrackingDataContext.Tenants.Update(tenant);
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public TenantModel UpgradePremium(int tenantId, string dbIdentifier, string endPoint)
        {
            try
            {
                var tenant = TrackingDataContext.Tenants.FirstOrDefault(x => x.TenantId == tenantId && x.IsDeleted == 0);
                if (tenant == null)
                {
                    return new();
                }
                tenant.EndPointDb = endPoint;
                tenant.Type = 1;
                tenant.Status = 1;
                tenant.RdsIdentifier = dbIdentifier;
                TrackingDataContext.SaveChanges();
                var tenantModel = ConvertEntityToModel(tenant);
                return tenantModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new();
            }
        }

        public bool UpdateStatusTenant(int tenantId, byte status)
        {
            try
            {
                var tenant = TrackingDataContext.Tenants.FirstOrDefault(x => x.TenantId == tenantId && x.IsDeleted == 0);
                if (tenant == null)
                {
                    return false;
                }
                tenant.Status = status;
                TrackingDataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}