using Domain.Models.AccountDue;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class AccountDueRepository : IAccountDueRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;

    public AccountDueRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<AccountDueListModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize)
    {
        List<AccountDueListModel> accountDueList = new List<AccountDueListModel>();

        // Right table
        var seikyuList = _tenantNoTrackingDataContext.SyunoSeikyus
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId).ToList();

        if (isUnpaidChecked && seikyuList.Count > 0)
        {
            seikyuList = seikyuList?.Where(item => item.NyukinKbn == 1).ToList();
        }


        // Left table
        var nyukinList = _tenantNoTrackingDataContext.SyunoNyukin
                               .Where(item => item.HpId == hpId
                                                   && item.PtId == ptId
                                                   && item.IsDeleted == 0);
       
        

        var raiinList = _tenantNoTrackingDataContext.RaiinInfs
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId
                                            && item.IsDeleted == DeleteTypes.None
                                            && item.Status > RaiinState.TempSave);

        var kaMstList = _tenantNoTrackingDataContext.KaMsts
                        .Where(item => item.HpId == hpId
                                            && item.IsDeleted == 0);

        var listPtHokenPattern = _tenantNoTrackingDataContext.PtHokenPatterns
                         .Where(pattern => pattern.HpId == hpId
                                            && pattern.PtId == ptId
                                            && (pattern.IsDeleted == DeleteTypes.None));


        return accountDueList;
    }
}
