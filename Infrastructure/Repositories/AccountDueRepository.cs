using Domain.Models.AccountDue;
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

    public List<AccountDueListModel> GetAccountDueList(long ptId, int sinDate)
    {
        List<AccountDueListModel> accountDueList = new List<AccountDueListModel>();

        var nyukinList = dbService.SyunoNyukinRepository
                               .FindListQueryable(item => item.HpId == _hpId
                                                   && item.PtId == ptId
                                                   && item.IsDeleted == 0);

        var seikyuList = dbService.SyunoSeikyuRepository
                        .FindListQueryable(item => item.HpId == _hpId
                                            && item.PtId == ptId);

        var raiinList = dbService.RaiinInfRepository
                        .FindListQueryable(item => item.HpId == _hpId
                                            && item.PtId == ptId
                                            && item.IsDeleted == DeleteTypes.None
                                            && item.Status > RaiinState.TempSave);

        var kaMstList = dbService.KaMstRepositorycs
                        .FindListQueryableNoTrack(item => item.HpId == _hpId
                                            && item.IsDeleted == 0);




        return accountDueList;
    }
}
