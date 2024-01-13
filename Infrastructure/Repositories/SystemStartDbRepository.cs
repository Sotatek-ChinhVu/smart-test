using Domain.Models.SystemStartDb;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Repositories;

public class SystemStartDbRepository : RepositoryBase, ISystemStartDbRepository
{
    private readonly ILoggingHandler _loggingHandler;
    private const string ModuleName = "SystemStartDbRepository";

    public SystemStartDbRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
        _loggingHandler = new LoggingHandler(tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public void DeleteAndUpdateData(int dateDelete)
    {

        #region Declare function
        void DeleteBackUpReq()
        {
            try
            {
                string sql = "DELETE FROM \"public\".\"backup_req\" "
                                    + " WHERE  \"to_date\" < @toDate";
                TrackingDataContext.Database.ExecuteSqlRaw(sql, new NpgsqlParameter("@toDate", dateDelete));
            }
            catch (Exception e)
            {
                _loggingHandler.WriteLogExceptionAsync(e, ModuleName);
                throw;
            }
        }

        void DeleteCalulateStatus()
        {
            try
            {
                string sql = "DELETE FROM \"public\".\"calc_status\" "
                                    + " WHERE \"create_date\" < @createDate";
                TrackingDataContext.Database.SetCommandTimeout(1800);
                TrackingDataContext.Database.ExecuteSqlRaw(sql,
                        new NpgsqlParameter("@createDate", CIUtil.IntToDate(dateDelete)));
            }
            catch (Exception e)
            {
                _loggingHandler.WriteLogExceptionAsync(e, ModuleName);
                throw;
            }
        }
        ///Temp no update status in here
        ///void UpdateCalulateStatus()
        ///{
        ///    try
        ///    {
        ///        string sql = "UPDATE \"public\".\"CALC_STATUS\"  SET  \"STATUS\" = 8"
        ///                                              + " WHERE  \"HP_ID\" = @hpId"
        ///                                              + "        AND  \"STATUS\" in (0, 1)"
        ///                                              + "        AND \"CREATE_DATE\" < @createDate";
        ///        TrackingDataContext.Database.SetCommandTimeout(1800);
        ///        TrackingDataContext.Database.ExecuteSqlRaw(sql,
        ///                new NpgsqlParameter("@hpId", hpId),
        ///                new NpgsqlParameter("@createDate", CIUtil.IntToDate(dateDelete)));
        ///    }
        ///    catch (Exception e)
        ///    {
        ///        _loggingHandler.WriteLogExceptionAsync(e, ModuleName);
        ///    }
        ///}
        ///Temp no delete lock in here
        ///void DeleteLockInf()
        ///{
        ///    try
        ///    {
        ///        string sql = "DELETE FROM \"public\".\"LOCK_INF\" "
        ///                            + " WHERE  \"HP_ID\" = @hpId"
        ///                            + "        AND  \"SIN_DATE\" < @sinDate"
        ///                            + "        AND  \"MACHINE\" = @computerName";
        ///        dbConnection.ExecuteNonQuery(sql,
        ///            new NpgsqlParameter("@hpId", hpCd),
        ///            new NpgsqlParameter("@sinDate", dateDelete),
        ///            new NpgsqlParameter("@computerName", CIUtil.GetComputerName()));
        ///    }
        ///    catch (Exception e)
        ///    {
        ///        Log.WriteLogError(ModuleName, this, nameof(DeleteLockInf), e);
        ///    }
        ///}

        void DeleteRenkeiReq()
        {
            try
            {
                string sql = "DELETE FROM \"public\".\"renkei_req\" "
                                    + " WHERE \"create_date\" < @createDate";

                TrackingDataContext.Database.ExecuteSqlRaw(sql,
                   new NpgsqlParameter("@createDate", CIUtil.IntToDate(dateDelete)));
            }
            catch (Exception e)
            {
                _loggingHandler.WriteLogExceptionAsync(e, ModuleName);
                throw;
            }
        }
        #endregion

        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

        executionStrategy.Execute(
             () =>
             {
                 using var transaction = TrackingDataContext.Database.BeginTransaction();
                 try
                 {
                     DeleteCalulateStatus();
                     DeleteBackUpReq();
                     DeleteRenkeiReq();
                     ///DeleteLockInf();
                     ///UpdateCalulateStatus();
                     transaction.Commit();
                 }
                 catch (Exception e)
                 {
                     transaction.Rollback();
                     _loggingHandler.WriteLogExceptionAsync(e, ModuleName);
                     throw;
                 }
             });
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
