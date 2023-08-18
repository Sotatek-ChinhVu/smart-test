using Domain.Models.Online;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
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

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
