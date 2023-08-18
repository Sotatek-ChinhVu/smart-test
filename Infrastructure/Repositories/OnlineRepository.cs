using Domain.Models.Online;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

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
