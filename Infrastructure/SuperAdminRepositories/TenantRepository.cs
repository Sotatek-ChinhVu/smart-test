using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Helper.Common;
using Helper.Enum;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Npgsql;
using System.Data;
using System.Text;

namespace Infrastructure.SuperAdminRepositories;

public class TenantRepository : SuperAdminRepositoryBase, ITenantRepository
{
    public TenantRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public TenantModel Get(int tenantId)
    {
        var tenant = NoTrackingDataContext.Tenants.Where(t => t.TenantId == tenantId && t.IsDeleted == 0).FirstOrDefault();
        var tenantModel = tenant == null ? new() : ConvertEntityToModel(tenant);
        return tenantModel;
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

    public bool UpdateStatusTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier)
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

    public bool UpgradePremium(int tenantId, string dbIdentifier, string endPoint)
    {
        try
        {
            var tenant = TrackingDataContext.Tenants.FirstOrDefault(x => x.TenantId == tenantId && x.IsDeleted == 0);
            if (tenant == null)
            {
                return false;
            }
            tenant.EndPointDb = endPoint;
            tenant.Type = 1;
            tenant.Status = 1;
            tenant.RdsIdentifier = dbIdentifier;
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

    public List<TenantModel> GetTenantList(int tenantId, SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take)
    {
        List<TenantModel> result;
        IQueryable<Tenant> query = NoTrackingDataContext.Tenants;
        if (tenantId > 0)
        {
            query = query.Where(item => item.TenantId == tenantId);
        }
        if (!searchModel.IsEmptyModel)
        {
            // filte data ignore storageFull
            query = FilterData(query, searchModel);
        }

        // sort data ignore storageFull
        if (searchModel.StorageFull == StorageFullEnum.Empty && !sortDictionary.ContainsKey(TenantEnum.StorageFull))
        {
            var querySortList = SortTenantQuery(query, sortDictionary);
            querySortList = (IOrderedQueryable<Tenant>)querySortList.Skip(skip).Take(take);
            result = querySortList.Select(item => ConvertEntityToModel(item)).ToList();
            return result;
        }

        result = query.Select(item => ConvertEntityToModel(item)).ToList();
        result = CalculateStorageFull(result);
        if (searchModel.StorageFull != StorageFullEnum.Empty)
        {
            switch (searchModel.StorageFull)
            {
                case StorageFullEnum.Under70Percent:
                    result = result.Where(item => item.StorageFull <= 70).ToList();
                    break;
                case StorageFullEnum.Over70Percent:
                    result = result.Where(item => item.StorageFull >= 70).ToList();
                    break;
                case StorageFullEnum.Over80Percent:
                    result = result.Where(item => item.StorageFull >= 80).ToList();
                    break;
                case StorageFullEnum.Over90Percent:
                    result = result.Where(item => item.StorageFull >= 90).ToList();
                    break;
            }
        }
        result = SortTenantList(result, sortDictionary).Skip(skip).Take(take).ToList();
        return result;
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
        if (searchModel.Type != 0)
        {
            query = query.Where(item => item.Type == searchModel.Type);
        }
        if (searchModel.Status != 0)
        {
            query = query.Where(item => item.Status == searchModel.Status);
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
                        case TenantEnum.Status:
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
                        case TenantEnum.Status:
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
                        case TenantEnum.Status:
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
                        case TenantEnum.Status:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.Status);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.Status);
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

    private List<TenantModel> CalculateStorageFull(List<TenantModel> tenantList)
    {
        List<TenantModel> result = new();
        foreach (var tenant in tenantList)
        {
            double storageInDB = 0;
            int port = 5432;
            string id = "postgres";
            string password = "Emr!23456789";
            StringBuilder sConnectionString = new();
            sConnectionString.Append("host=");
            sConnectionString.Append(tenant.EndPointDb);
            sConnectionString.Append(";port=");
            sConnectionString.Append(port.ToString());
            sConnectionString.Append(";database=");
            sConnectionString.Append(tenant.Db);
            sConnectionString.Append(";user id=");
            sConnectionString.Append(id);
            sConnectionString.Append(";password=");
            sConnectionString.Append(password);

            var connStr = new NpgsqlConnectionStringBuilder(sConnectionString.ToString());
            connStr.TrustServerCertificate = true;
            try
            {
                using (var conn = new NpgsqlConnection(connStr.ToString()))
                {
                    conn.Open();
                    string sqlQuery = string.Format("select pg_database_size('{0}')", tenant.Db);
                    using (var command = new NpgsqlCommand(sqlQuery, conn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            storageInDB = reader.GetInt32(0);
                        }
                        reader.Close();
                    }
                }
                double storageFull = (storageInDB / tenant.Size) * 100;
                tenant.ChangeStorageFull(storageFull);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not connect to database " + tenant.EndPointDb + tenant.Db + "\n" + ex.ToString());
            }
        }

        return result;
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
            tenant.Type,
            tenant.EndPointDb,
            tenant.EndSubDomain,
            tenant.Action,
            tenant.ScheduleDate,
            tenant.ScheduleTime,
            tenant.CreateDate,
            tenant.RdsIdentifier
            );
    }
    #endregion
}
