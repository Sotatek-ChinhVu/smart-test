using Domain.Models.AccountDue;
using Entity.Tenant;
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
        // Left table
        var seikyuList = _tenantNoTrackingDataContext.SyunoSeikyus
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId).ToList();

        if (isUnpaidChecked && seikyuList.Count > 0)
        {
            seikyuList = seikyuList?.Where(item => item.NyukinKbn == 1).ToList();
        }

        // Right table
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

        var accountDueList = (from seikyu in seikyuList
                              join nyukin in nyukinList on new { seikyu.HpId, seikyu.PtId, seikyu.SinDate, seikyu.RaiinNo }
                                                        equals new { nyukin.HpId, nyukin.PtId, nyukin.SinDate, nyukin.RaiinNo }
                              join raiinItem in raiinList on new { seikyu.HpId, seikyu.PtId, seikyu.RaiinNo }
                                                        equals new { raiinItem.HpId, raiinItem.PtId, raiinItem.RaiinNo }
                              join kaMst in kaMstList on new { raiinItem.KaId }
                                                        equals new { kaMst.KaId }
                              select ConvertToAccountDueListModel(hpId, ptId, sinDate, seikyu, nyukin, raiinItem, kaMst)
                         )
                         .OrderBy(item => item.SinDate)
                         .ThenBy(item => item.RaiinNo)
                         .ThenBy(item => item.SortNo)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
        return accountDueList;
    }

    private AccountDueListModel ConvertToAccountDueListModel(int hpId, long ptId, int sinDate, SyunoSeikyu seikyu, SyunoNyukin nyukin, RaiinInf raiinItem, KaMst kaMst)
    {
        return new AccountDueListModel
            (
                hpId,
                ptId,
                sinDate,
                GetMonth(sinDate),
                seikyu.RaiinNo,
                raiinItem.HokenPid,
                raiinItem.OyaRaiinNo,
                seikyu.NyukinKbn,
                seikyu.SeikyuTensu,
                seikyu.SeikyuGaku,
                nyukin.AdjustFutan,
                nyukin.NyukinGaku,
                nyukin.PaymentMethodCd,
                nyukin.NyukinDate,
                nyukin.UketukeSbt,
                nyukin.NyukinCmt,
                seikyu.NewSeikyuGaku,
                seikyu.NewAdjustFutan,
                kaMst.KaSname ?? string.Empty,
                nyukin.SortNo
            );
    }
    private int GetMonth(int date)
    {
        return (date / 100);
    }

    public Dictionary<int, string> GetPaymentMethod(int hpId)
    {
        Dictionary<int, string> result = new();
        var paymentMethodList = _tenantNoTrackingDataContext.PaymentMethodMsts.Where(item => item.HpId == hpId).OrderBy(item => item.SortNo).ToList();
        foreach (var paymentMethod in paymentMethodList)
        {
            result.Add(paymentMethod.PaymentMethodCd, paymentMethod.PayName);
        }
        return result;
    }

    public Dictionary<int, string> GetUketsukeSbt(int hpId)
    {
        Dictionary<int, string> result = new();
        var uketukeList = _tenantNoTrackingDataContext.UketukeSbtMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();
        foreach (var uketuke in uketukeList)
        {
            result.Add(uketuke.KbnId, uketuke.KbnName);
        }
        return result;
    }
}
