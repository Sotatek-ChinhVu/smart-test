using Domain.Models.Online;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Infrastructure.Repositories;

public class OnlineRepository : RepositoryBase, IOnlineRepository
{
    public OnlineRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public bool InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList)
    {
        var onlineInsertList = onlineList.Select(item => new OnlineConfirmationHistory()
        {
            ID = 0,
            PtId = item.PtId,
            OnlineConfirmationDate = item.OnlineConfirmationDate,
            ConfirmationType = item.ConfirmationType,
            ConfirmationResult = item.ConfirmationResult,
            UketukeStatus = item.UketukeStatus,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateId = userId,
        }).ToList();
        TrackingDataContext.AddRange(onlineInsertList);
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<OnlineConfirmationHistoryModel> GetRegisterdPatientsFromOnline(int confirmDate, int id = 0, int confirmType = 1)
    {
        List<OnlineConfirmationHistory> onlineList;
        if (id == 0)
        {
            onlineList = NoTrackingDataContext.OnlineConfirmationHistories.Where(item => item.ConfirmationType == confirmType)
                                                                          .ToList();
            onlineList = onlineList.Where(item => CIUtil.DateTimeToInt(item.OnlineConfirmationDate) == confirmDate).ToList();
        }
        else
        {
            onlineList = NoTrackingDataContext.OnlineConfirmationHistories.Where(item => item.ID == id).ToList();
        }

        var result = onlineList.OrderBy(item => item.OnlineConfirmationDate)
                               .Select(item => ConvertToModel(item))
                               .ToList();
        return result;
    }

    public bool UpdateOnlineConfirmationHistory(int uketukeStatus, int id, int userId)
    {
        string updateDate = CIUtil.GetJapanDateTimeNow().ToString("yyyy-MM-dd HH:mm:ss.fff");

        string updateQuery = $"UPDATE \"ONLINE_CONFIRMATION_HISTORY\" SET \"UKETUKE_STATUS\" = {uketukeStatus}, \"UPDATE_DATE\" = '{updateDate}'"
                             + $", \"UPDATE_ID\" = {userId}"
                             + $" WHERE \"ID\" = {id} AND \"UKETUKE_STATUS\" = 0";

        return TrackingDataContext.Database.ExecuteSqlRaw(updateQuery) > 0;
    }

    public bool UpdateOnlineHistoryById(int userId, long id, long ptId, int uketukeStatus, int confirmationType)
    {
        var onlineHistory = TrackingDataContext.OnlineConfirmationHistories.FirstOrDefault(item => item.ID == id);
        if (onlineHistory == null)
        {
            return false;
        }
        onlineHistory.UpdateDate = CIUtil.GetJapanDateTimeNow();
        onlineHistory.UpdateId = userId;
        onlineHistory.UketukeStatus = uketukeStatus;
        onlineHistory.PtId = ptId;
        onlineHistory.ConfirmationType = confirmationType;
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool CheckExistIdList(List<long> idList)
    {
        idList = idList.Distinct().ToList();
        var countId = NoTrackingDataContext.OnlineConfirmationHistories.Count(x => idList.Contains(x.ID));
        return idList.Count == countId;
    }

    #region private function
    private OnlineConfirmationHistoryModel ConvertToModel(OnlineConfirmationHistory entity)
    {
        return new OnlineConfirmationHistoryModel(
                   entity.ID,
                   entity.PtId,
                   entity.OnlineConfirmationDate,
                   entity.ConfirmationType,
                   entity.InfoConsFlg ?? string.Empty,
                   entity.ConfirmationResult ?? string.Empty,
                   entity.PrescriptionIssueType,
                   entity.UketukeStatus
               );
    }

    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
