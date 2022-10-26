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

    public List<AccountDueItemModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize)
    {
        // Left table
        var seikyuList = _tenantNoTrackingDataContext.SyunoSeikyus
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId).ToList();

        if (isUnpaidChecked && seikyuList.Count > 0)
        {
            seikyuList = seikyuList?.Where(item => item.NyukinKbn == 1 || !new List<int> { 1, 2, 3 }.Contains(item.NyukinKbn)).ToList();
        }

        // Right table
        var nyukinList = _tenantNoTrackingDataContext.SyunoNyukin
                               .Where(item => item.HpId == hpId
                                                   && item.PtId == ptId
                                                   && item.IsDeleted == 0).ToList();

        var raiinList = _tenantNoTrackingDataContext.RaiinInfs
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId
                                            && item.IsDeleted == DeleteTypes.None
                                            && item.Status > RaiinState.TempSave).ToList();

        var kaMstList = _tenantNoTrackingDataContext.KaMsts
                        .Where(item => item.HpId == hpId
                                            && item.IsDeleted == 0).ToList();

        var accountDueList = (from seikyu in seikyuList
                              join nyukin in nyukinList on new { seikyu.HpId, seikyu.PtId, seikyu.SinDate, seikyu.RaiinNo }
                                                          equals new { nyukin.HpId, nyukin.PtId, nyukin.SinDate, nyukin.RaiinNo } into nyukinLeft
                              from nyukinItem in nyukinLeft.DefaultIfEmpty()
                              join raiinItem in raiinList on new { seikyu.HpId, seikyu.PtId, seikyu.RaiinNo }
                                                          equals new { raiinItem.HpId, raiinItem.PtId, raiinItem.RaiinNo }
                              join kaMst in kaMstList on new { raiinItem.KaId }
                                                          equals new { kaMst.KaId } into kaMstLeft
                              from kaMstItem in kaMstLeft.DefaultIfEmpty()
                              select ConvertToAccountDueListModel(hpId, ptId, seikyu, nyukinItem, raiinItem, kaMstItem)
                         )
                         .OrderByDescending(item => item.SinDate)
                         .ThenByDescending(item => item.RaiinNo)
                         .ThenByDescending(item => item.SortNo)
                         .Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();
        return accountDueList;
    }

    private AccountDueItemModel ConvertToAccountDueListModel(int hpId, long ptId, SyunoSeikyu seikyu, SyunoNyukin nyukin, RaiinInf raiinItem, KaMst kaMst)
    {
        return new AccountDueItemModel
            (
                hpId,
                ptId,
                seikyu.SinDate,
                GetMonth(seikyu.SinDate),
                seikyu.RaiinNo,
                raiinItem.HokenPid,
                raiinItem.OyaRaiinNo,
                seikyu.NyukinKbn,
                seikyu.SeikyuTensu,
                seikyu.SeikyuGaku,
                nyukin != null ? nyukin.AdjustFutan : 0,
                nyukin != null ? nyukin.NyukinGaku : 0,
                nyukin != null ? nyukin.PaymentMethodCd : 0,
                nyukin != null ? nyukin.NyukinDate : 0,
                nyukin != null ? nyukin.UketukeSbt : 0,
                nyukin != null ? nyukin.NyukinCmt ?? string.Empty : string.Empty,
                seikyu.NewSeikyuGaku,
                seikyu.NewAdjustFutan,
                kaMst.KaSname ?? string.Empty,
                nyukin != null ? nyukin.SortNo : 0
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
            result.Add(paymentMethod.PaymentMethodCd, paymentMethod.PayName ?? string.Empty);
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
