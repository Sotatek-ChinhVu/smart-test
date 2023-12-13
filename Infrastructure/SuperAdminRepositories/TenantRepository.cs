using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Helper.Common;
using Helper.Constants;
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

        public int SumSubDomainToDbIdentifier(string dbIdentifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.RdsIdentifier == dbIdentifier && t.IsDeleted == 0);
            if (tenant != null)
            {
                return tenant.Count();
            }
            return 0;
        }

        public bool CheckExistsHospital(string hospital)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.Hospital.ToLower().Trim() == hospital.ToLower().Trim() && t.IsDeleted == 0);
            if (tenant.Any())
            {
                return true;
            }
            return false;
        }

        public bool CheckExistsSubDomain(string subDomain)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain.ToLower().Trim() == subDomain.ToLower().Trim() && t.IsDeleted == 0);
            if (tenant.Any())
            {
                return true;
            }
            return false;
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
            tenant.UserConnect = model.UserConnect;
            tenant.PasswordConnect = model.PasswordConnect;
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
                    tenant.EndSubDomain = endSubDomain;
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

        public TenantModel UpdateTenant(int tenantId, string dbIdentifier, string endPoint, string subDomain, int size, int sizeType, string hospital, int adminId, string password)
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
                tenant.SubDomain = subDomain;
                tenant.Size = size;
                tenant.SizeType = sizeType;
                tenant.RdsIdentifier = dbIdentifier;
                tenant.Hospital = hospital;
                tenant.AdminId = adminId;
                tenant.Password = password;
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
                    throw new Exception("Tenant does not exist");
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

        public TenantModel TerminateTenant(int tenantId, byte TerminateStatus)
        {
            try
            {
                var tenant = TrackingDataContext.Tenants.FirstOrDefault(x => x.TenantId == tenantId && x.IsDeleted == 0);
                if (tenant == null)
                {
                    return new();
                }

                tenant.Status = TerminateStatus;
                tenant.IsDeleted = 1;
                tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
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

        public void RevokeInsertPermission()
        {
            //check status tenant = available
            var listTenant = NoTrackingDataContext.Tenants.Where(i => i.Status == 1 && i.IsDeleted == 0).ToList();
            foreach (var tenant in listTenant)
            {
                var connectionString = $"Host={tenant.EndPointDb};Username=postgres;Password=Emr!23456789;Port=5432";
                try
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var sizeCheckCommand = new NpgsqlCommand())
                        {
                            sizeCheckCommand.Connection = connection;
                            sizeCheckCommand.CommandText = $"SELECT pg_database_size('{tenant.Db}')";

                            var databaseSize = sizeCheckCommand.ExecuteScalar();
                            if (databaseSize == null) continue;
                            if (tenant.SizeType == 1) //MB
                            {
                                GrantOrRevokeExecute(tenant.Size, tenant.SizeType, connection, databaseSize, tenant.UserConnect, tenant.Db);
                            }
                            else if (tenant.SizeType == 2) //GB
                            {
                                GrantOrRevokeExecute(tenant.Size, tenant.SizeType, connection, databaseSize, tenant.UserConnect, tenant.Db);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception Schedule Task: {ex.Message}");
                }
            }
        }
        private void GrantOrRevokeExecute(int size, int sizeType, NpgsqlConnection connection, object? databaseSize, string role, string dbName)
        {
            var sizeDatabase = Convert.ToInt64(databaseSize) / (1024 * 1024); //MB
            if (sizeType == 2)
            {
                sizeDatabase = Convert.ToInt64(databaseSize) / 1024; //GB
            }
            if (sizeDatabase < size)
            {
                using (var grantCommand = new NpgsqlCommand())
                {
                    grantCommand.Connection = connection;
                    grantCommand.CommandText = $"GRANT INSERT ON ALL TABLES IN SCHEMA public TO {role}";
                    grantCommand.ExecuteNonQuery();
                    Console.WriteLine($"Schedule Task: GRANT INSERT DATABASE {dbName} SUCCESS");
                }
            }
            else
            {
                using (var revokeCommand = new NpgsqlCommand())
                {
                    revokeCommand.Connection = connection;
                    revokeCommand.CommandText = $"REVOKE INSERT ON ALL TABLES IN SCHEMA public FROM {role}";
                    revokeCommand.ExecuteNonQuery();
                    Console.WriteLine($"Schedule Task: REVOKE INSERT DATABASE {dbName} SUCCESS");
                }
            }
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public (List<TenantModel> TenantList, int TotalTenant) GetTenantList(SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take)
        {
            int totalTenant = 0;
            List<TenantModel> result;
            IQueryable<Tenant> query = NoTrackingDataContext.Tenants.Where(item => item.IsDeleted == 0);
            if (!searchModel.IsEmptyModel)
            {
                // filte data ignore storageFull
                query = FilterData(query, searchModel);
            }

            // sort data ignore storageFull
            if (!searchModel.StorageFull.Any() && !sortDictionary.ContainsKey(TenantEnum.StorageFull))
            {
                // get totalTenant to FE
                totalTenant = query.Count();
                var querySortList = SortTenantQuery(query, sortDictionary);
                querySortList = (IOrderedQueryable<Tenant>)querySortList.Skip(skip).Take(take);
                result = querySortList.Select(tenant => new TenantModel(
                                                            tenant.TenantId,
                                                            tenant.Hospital,
                                                            tenant.Status,
                                                            tenant.AdminId,
                                                            tenant.Password,
                                                            tenant.SubDomain,
                                                            tenant.Db,
                                                            tenant.Size,
                                                            tenant.SizeType,
                                                            tenant.Type,
                                                            tenant.EndPointDb,
                                                            tenant.EndSubDomain,
                                                            tenant.Action,
                                                            tenant.ScheduleDate,
                                                            tenant.ScheduleTime,
                                                            tenant.CreateDate,
                                                            tenant.RdsIdentifier,
                                                            tenant.UserConnect,
                                                            tenant.PasswordConnect))
                                      .ToList();
                result = ChangeStorageFull(result);
                result = SortTenantList(result, sortDictionary).ToList();
                return (result, totalTenant);
            }
            result = query.Select(tenant => new TenantModel(
                                            tenant.TenantId,
                                            tenant.Hospital,
                                            tenant.Status,
                                            tenant.AdminId,
                                            tenant.Password,
                                            tenant.SubDomain,
                                            tenant.Db,
                                            tenant.Size,
                                            tenant.SizeType,
                                            tenant.Type,
                                            tenant.EndPointDb,
                                            tenant.EndSubDomain,
                                            tenant.Action,
                                            tenant.ScheduleDate,
                                            tenant.ScheduleTime,
                                            tenant.CreateDate,
                                            tenant.RdsIdentifier,
                                            tenant.UserConnect,
                                            tenant.PasswordConnect))
                          .ToList();
            result = ChangeStorageFull(result);
            if (searchModel.StorageFull.Any())
            {
                // filter StorageFull by multiple conditions
                if (searchModel.StorageFull.Contains(StorageFullEnum.Under70Percent))
                {
                    result = result.Where(item => item.StorageFull <= 70).ToList();
                }
                if (searchModel.StorageFull.Contains(StorageFullEnum.Over70Percent))
                {
                    result = result.Where(item => item.StorageFull >= 70).ToList();
                }
                if (searchModel.StorageFull.Contains(StorageFullEnum.Over80Percent))
                {
                    result = result.Where(item => item.StorageFull >= 80).ToList();
                }
                if (searchModel.StorageFull.Contains(StorageFullEnum.Over90Percent))
                {
                    result = result.Where(item => item.StorageFull >= 90).ToList();
                }
            }
            // get totalTenant to FE
            totalTenant = result.Count;
            result = SortTenantList(result, sortDictionary).Skip(skip).Take(take).ToList();
            return (result, totalTenant);
        }

        public TenantModel GetTenant(int tenantId)
        {
            var tenant = NoTrackingDataContext.Tenants.FirstOrDefault(item => item.TenantId == tenantId && item.IsDeleted == 0);
            if (tenant == null)
            {
                return new();
            }
            var tenantModel = ConvertEntityToModel(tenant);
            var result = GetStorageFullItem(tenantModel, true);
            tenantModel.ChangeStorageFull(result.storageFull, result.storageUsed);
            return tenantModel;
        }

        #region private function
        private IQueryable<Tenant> FilterData(IQueryable<Tenant> query, SearchTenantModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.KeyWord))
            {
                int tenantIdQuery = searchModel.KeyWord.AsInteger();
                query = query.Where(item => (tenantIdQuery > 0 && item.TenantId == tenantIdQuery)
                                            || item.SubDomain.Contains(searchModel.KeyWord)
                                            || (tenantIdQuery > 0 && item.AdminId == tenantIdQuery)
                                            || item.Hospital.Contains(searchModel.KeyWord));
            }
            if (searchModel.FromDate != null)
            {
                query = query.Where(item => item.CreateDate >= searchModel.FromDate);
            }
            if (searchModel.ToDate != null)
            {
                query = query.Where(item => item.CreateDate <= searchModel.ToDate);
            }
            if (searchModel.Type != -1)
            {
                query = query.Where(item => item.Type == searchModel.Type);
            }
            if (searchModel.StatusTenant != 0)
            {
                // if filter by statusTenant, get real status in the database
                var statusTenantQuery = StatusTenantDisplayConst.StatusTenantDisplayDictionnary.Where(item => item.Value == searchModel.StatusTenant).Select(item => item.Key).Distinct().ToList();
                query = query.Where(item => statusTenantQuery.Contains(item.Status));
            }
            return query;
        }

        private IOrderedQueryable<Tenant> SortTenantQuery(IQueryable<Tenant> query, Dictionary<TenantEnum, int> sortDictionary)
        {
            bool firstSort = true;
            IOrderedQueryable<Tenant> querySortList = query.OrderByDescending(item => item.TenantId);
            foreach (var sortItem in sortDictionary)
            {
                switch (sortItem.Value)
                {
                    // DESC
                    case 1:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Size);
                                break;
                            case TenantEnum.StatusTenant:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Status);
                                break;
                        }
                        break;
                    // ASC
                    default:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Size);
                                break;
                            case TenantEnum.StatusTenant:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Status);
                                break;
                        }
                        break;
                }
                firstSort = false;
            }
            return querySortList;
        }

        private IOrderedEnumerable<TenantModel> SortTenantList(List<TenantModel> tenantList, Dictionary<TenantEnum, int> sortDictionary)
        {
            bool firstSort = true;
            IOrderedEnumerable<TenantModel> querySortList = tenantList.OrderByDescending(item => item.TenantId);
            foreach (var sortItem in sortDictionary)
            {
                switch (sortItem.Value)
                {
                    // DESC
                    case 1:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Size);
                                break;
                            case TenantEnum.StatusTenant:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.Status);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.Status);
                                break;
                            case TenantEnum.StorageFull:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderByDescending(item => item.StorageFull);
                                    continue;
                                }
                                querySortList = querySortList.ThenByDescending(item => item.StorageFull);
                                break;
                        }
                        break;
                    // ASC
                    default:
                        switch (sortItem.Key)
                        {
                            case TenantEnum.CreateDate:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.CreateDate);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.CreateDate);
                                break;
                            case TenantEnum.TenantId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.TenantId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.TenantId);
                                break;
                            case TenantEnum.Domain:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.SubDomain);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.SubDomain);
                                break;
                            case TenantEnum.AdminId:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.AdminId);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.AdminId);
                                break;
                            case TenantEnum.HospitalName:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Hospital);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Hospital);
                                break;
                            case TenantEnum.Type:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Type);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Type);
                                break;
                            case TenantEnum.Size:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.Size);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.Size);
                                break;
                            case TenantEnum.StatusTenant:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.StatusTenant);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.StatusTenant);
                                break;
                            case TenantEnum.StorageFull:
                                if (firstSort)
                                {
                                    querySortList = querySortList.OrderBy(item => item.StorageFull);
                                    continue;
                                }
                                querySortList = querySortList.ThenBy(item => item.StorageFull);
                                break;
                        }
                        break;
                }
                firstSort = false;
            }
            return querySortList;
        }

        private List<TenantModel> ChangeStorageFull(List<TenantModel> tenantList)
        {
            Parallel.ForEach(tenantList, tenant =>
            {
                var result = GetStorageFullItem(tenant, false);
                tenant.ChangeStorageFull(result.storageFull, result.storageUsed);
            });
            return tenantList;
        }

        private (double storageFull, double storageUsed) GetStorageFullItem(TenantModel tenant, bool isClearCache)
        {
            double storageFull = 0;
            double storageInDB = 0;
            int port = 5432;
            StringBuilder connectionStringBuilder = new();
            connectionStringBuilder.Append("host=");
            connectionStringBuilder.Append(tenant.EndPointDb);
            connectionStringBuilder.Append(";port=");
            connectionStringBuilder.Append(port.ToString());
            connectionStringBuilder.Append(";database=");
            connectionStringBuilder.Append(tenant.Db);
            connectionStringBuilder.Append(";user id=");
            connectionStringBuilder.Append(tenant.UserConnect);
            connectionStringBuilder.Append(";password=");
            connectionStringBuilder.Append(tenant.PasswordConnect);
            string connectionString = connectionStringBuilder.ToString();
            string finalKey = string.Format("{0}_{1}_{2}", connectionString, tenant.Size.ToString(), tenant.SizeType);
            if (isClearCache)
            {
                _cache.KeyDelete(finalKey);
            }
            if (_cache.KeyExists(finalKey))
            {
                storageFull = _cache.StringGet(finalKey).AsInteger();
                // return storageUsed in database
                return (storageFull, Math.Round((tenant.Size * storageFull) / 100));
            }
            else
            {
                var connStr = new NpgsqlConnectionStringBuilder(connectionString);
                connStr.TrustServerCertificate = true;
                try
                {
                    using (var conn = new NpgsqlConnection(connStr.ToString()))
                    {
                        conn.Open();
                        string sqlQuery = string.Format("select pg_database_size('{0}')", tenant.Db);
                        using (var command = new NpgsqlCommand(sqlQuery, conn))
                        {
                            NpgsqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Read();

                                // calculate storageInDB
                                double sizeInDB = reader.GetInt64(0);
                                /// 1: MB; 2: GB
                                switch (tenant.SizeType)
                                {
                                    case 1:
                                        storageInDB = Math.Round(sizeInDB / 1024 / 1024, 2);
                                        break;
                                    case 2:
                                        storageInDB = Math.Round(sizeInDB / 1024 / 1024 / 1024, 2);
                                        break;
                                }
                            }
                            reader.Close();
                        }
                    }
                    storageFull = Math.Round((storageInDB / tenant.Size) * 100);
                    if (storageFull > 0)
                    {
                        _cache.StringSet(finalKey, storageFull.ToString());
                        _cache.KeyExpire(finalKey, new TimeSpan(1, 0, 0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can not connect to database " + tenant.EndPointDb + tenant.Db + "\n" + ex.ToString());
                }
            }
            // return storageUsed in database
            return (storageFull, storageInDB);
        }

        private TenantModel ConvertEntityToModel(Tenant tenant)
        {
            return new TenantModel(
                       tenant.TenantId,
                       tenant.Hospital,
                       tenant.Status,
                       tenant.AdminId,
                       tenant.Password,
                       tenant.SubDomain,
                       tenant.Db,
                       tenant.Size,
                       tenant.SizeType,
                       tenant.Type,
                       tenant.EndPointDb,
                       tenant.EndSubDomain,
                       tenant.Action,
                       tenant.ScheduleDate,
                       tenant.ScheduleTime,
                       tenant.CreateDate,
                       tenant.RdsIdentifier,
                       tenant.UserConnect,
                       tenant.PasswordConnect);
        }
        #endregion
    }
}