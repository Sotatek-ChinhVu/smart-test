using Domain.Models.AccountDue;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class AccountDueRepository : RepositoryBase, IAccountDueRepository
{
    public AccountDueRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<AccountDueModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked)
    {
        // Left table
        var seikyuList = NoTrackingDataContext.SyunoSeikyus
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId).ToList();

        if (isUnpaidChecked && seikyuList.Count > 0)
        {
            seikyuList = seikyuList?.Where(item => item.NyukinKbn == 1 || !new List<int> { 1, 2, 3 }.Contains(item.NyukinKbn)).ToList();
        }

        // Right table
        var nyukinList = NoTrackingDataContext.SyunoNyukin
                               .Where(item => item.HpId == hpId
                                                   && item.PtId == ptId
                                                   && item.IsDeleted == 0).ToList();

        var raiinList = NoTrackingDataContext.RaiinInfs
                        .Where(item => item.HpId == hpId
                                            && item.PtId == ptId
                                            && item.IsDeleted == DeleteTypes.None
                                            && item.Status > RaiinState.TempSave).ToList();

        var kaMstList = NoTrackingDataContext.KaMsts
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
                         .OrderBy(item => item.SeikyuSinDate)
                         .ThenBy(item => item.RaiinNo)
                         .ThenBy(item => item.SortNo)
                         .ToList();
        return accountDueList;
    }

    private AccountDueModel ConvertToAccountDueListModel(int hpId, long ptId, SyunoSeikyu seikyu, SyunoNyukin nyukin, RaiinInf raiinItem, KaMst kaMst)
    {
        return new AccountDueModel
            (
                hpId,
                ptId,
                seikyu?.SinDate ?? 0,
                GetMonth(seikyu?.SinDate ?? 0),
                seikyu?.RaiinNo ?? 0,
                raiinItem?.HokenPid ?? 0,
                raiinItem?.OyaRaiinNo ?? 0,
                seikyu?.NyukinKbn ?? 0,
                seikyu?.SeikyuTensu ?? 0,
                seikyu?.SeikyuGaku ?? 0,
                nyukin != null ? nyukin.AdjustFutan : 0,
                nyukin != null ? nyukin.NyukinGaku : 0,
                nyukin != null ? nyukin.PaymentMethodCd : 0,
                nyukin != null ? nyukin.NyukinDate : 0,
                nyukin != null ? nyukin.UketukeSbt : 0,
                nyukin != null ? nyukin.NyukinCmt ?? string.Empty : string.Empty,
                seikyu?.NewSeikyuGaku ?? 0,
                seikyu?.NewAdjustFutan ?? 0,
                kaMst?.KaSname ?? string.Empty,
                nyukin != null ? nyukin.SortNo : 0,
                nyukin != null ? nyukin.SeqNo : 0,
                seikyu?.SeikyuDetail ?? string.Empty,
                raiinItem?.Status ?? 0,
                seikyu?.AdjustFutan ?? 0,
                seikyu?.NewSeikyuDetail ?? string.Empty,
                seikyu?.NewSeikyuTensu ?? 0
            );
    }
    private int GetMonth(int date)
    {
        return (date / 100);
    }

    public Dictionary<int, string> GetPaymentMethod(int hpId)
    {
        Dictionary<int, string> result = new();
        var paymentMethodList = NoTrackingDataContext.PaymentMethodMsts.Where(item => item.HpId == hpId).OrderBy(item => item.SortNo).ToList();
        foreach (var paymentMethod in paymentMethodList)
        {
            result.Add(paymentMethod.PaymentMethodCd, paymentMethod.PayName ?? string.Empty);
        }
        return result;
    }

    public Dictionary<int, string> GetUketsukeSbt(int hpId)
    {
        Dictionary<int, string> result = new();
        var uketukeList = NoTrackingDataContext.UketukeSbtMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();
        foreach (var uketuke in uketukeList)
        {
            result.Add(uketuke.KbnId, uketuke.KbnName ?? string.Empty);
        }
        return result;
    }

    public List<AccountDueModel> SaveAccountDueList(int hpId, long ptId, int userId, int sinDate, List<AccountDueModel> listAccountDues, string kaikeiTime)
    {
        var listRaiinNo = listAccountDues.Select(item => item.RaiinNo).ToList();
        var raiinLists = TrackingDataContext.RaiinInfs
                                .Where(item => item.HpId == hpId
                                                    && item.PtId == ptId
                                                    && item.IsDeleted == DeleteTypes.None
                                                    && item.Status > RaiinState.TempSave
                                                    && listRaiinNo.Contains(item.RaiinNo))
                                .ToList();

        // Left table
        var seikyuLists = TrackingDataContext.SyunoSeikyus
                            .Where(item => item.HpId == hpId
                                                && item.PtId == ptId
                                                && listRaiinNo.Contains(item.RaiinNo))
                            .ToList();

        // Right table
        var nyukinLists = TrackingDataContext.SyunoNyukin
                               .Where(item => item.HpId == hpId
                                                   && item.PtId == ptId
                                                   && item.IsDeleted == 0
                                                   && listRaiinNo.Contains(item.RaiinNo))
                               .ToList();
        try
        {
            var dateTimeNow = CIUtil.GetJapanDateTimeNow();

            var originalList = listAccountDues;

            List<long> raiinUpdateList = new();

            foreach (var model in listAccountDues)
            {
                // Update raiin status
                var raiinInf = UpdateStatusRaiin(userId, dateTimeNow, model, raiinLists, kaikeiTime);
                if (raiinInf != null)
                {
                    raiinUpdateList.Add(raiinInf.RaiinNo);
                }

                // Update left table SyunoSeikyu
                var syunoSeikyu = UpdateStatusSyunoSeikyu(userId, dateTimeNow, model, seikyuLists);
                if (syunoSeikyu != null)
                {
                    raiinUpdateList.Add(syunoSeikyu.RaiinNo);
                }

                // Update right table SyunoNyukin
                var syunoNyukin = UpdateSyunoNyukin(hpId, ptId, userId, dateTimeNow, model, nyukinLists);
                if (syunoNyukin != null)
                {
                    raiinUpdateList.Add(syunoNyukin.RaiinNo);
                }
            }
            raiinUpdateList = raiinUpdateList.Distinct().ToList();
            TrackingDataContext.SaveChanges();

            var result = CompareResultList(originalList, raiinUpdateList);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private List<AccountDueModel> CompareResultList(List<AccountDueModel> originalList, List<long> raiinNoUpdateList)
    {
        var result = originalList.Where(original => raiinNoUpdateList.Contains(original.RaiinNo)).ToList();
        return result;
    }

    private RaiinInf? UpdateStatusRaiin(int userId, DateTime dateTimeNow, AccountDueModel model, List<RaiinInf> raiinLists, string kaikeiTime)
    {
        var raiin = raiinLists.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
        int tempStatus = model.NyukinKbn == 0 ? RaiinState.Waiting : RaiinState.Settled;
        if (raiin != null)
        {
            if (tempStatus != raiin.Status)
            {
                raiin.UpdateDate = dateTimeNow;
                raiin.UpdateId = userId;
                if (raiin.Status != tempStatus
                    || raiin.KaikeiTime != kaikeiTime)
                {
                    raiin.Status = tempStatus;
                    raiin.KaikeiTime = kaikeiTime;
                    return raiin;
                }
            }

            // update menjo
            if (model.NyukinKbn == 2)
            {
                raiin.UpdateDate = dateTimeNow;
                raiin.UpdateId = userId;
            }
        }
        return null;
    }

    private SyunoSeikyu? UpdateStatusSyunoSeikyu(int userId, DateTime dateTimeNow, AccountDueModel model, List<SyunoSeikyu> seikyuLists)
    {
        var seikyu = seikyuLists.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
        if (seikyu != null)
        {
            seikyu.UpdateDate = dateTimeNow;
            seikyu.UpdateId = userId;
            if (seikyu.NyukinKbn != model.NyukinKbn
                || seikyu.SeikyuGaku != model.SeikyuGaku
                || seikyu.AdjustFutan != model.SeikyuAdjustFutan)
            {
                seikyu.NyukinKbn = model.NyukinKbn;
                seikyu.SeikyuGaku = model.SeikyuGaku;
                seikyu.AdjustFutan = model.SeikyuAdjustFutan;
                return seikyu;
            }
        }
        return null;
    }

    private SyunoNyukin? UpdateSyunoNyukin(int hpId, long ptId, int userId, DateTime dateTimeNow, AccountDueModel model, List<SyunoNyukin> nyukinLists)
    {
        SyunoNyukin? nyukin;
        if (model.SeqNo == 0) // Create new SyunoNyukin
        {
            nyukin = new();
            nyukin.HpId = hpId;
            nyukin.PtId = ptId;
            nyukin.IsDeleted = 0;
            nyukin.SinDate = model.SeikyuSinDate;
            nyukin.RaiinNo = model.RaiinNo;
            nyukin.SortNo = model.SortNo;
            nyukin.AdjustFutan = model.AdjustFutan;
            nyukin.NyukinGaku = model.NyukinGaku;
            nyukin.PaymentMethodCd = model.PaymentMethodCd;
            nyukin.NyukinDate = model.NyukinDate;
            nyukin.UketukeSbt = model.UketukeSbt;
            nyukin.NyukinCmt = model.NyukinCmt;
            nyukin.NyukinjiSeikyu = model.SeikyuGaku;
            nyukin.NyukinjiTensu = model.SeikyuTensu;
            nyukin.NyukinjiDetail = model.SeikyuDetail;
            nyukin.CreateDate = dateTimeNow;
            nyukin.UpdateDate = dateTimeNow;
            nyukin.CreateId = userId;
            nyukin.UpdateId = userId;
            TrackingDataContext.SyunoNyukin.Add(nyukin);
            return nyukin;
        }
        else // Update SyunoNyukin
        {
            nyukin = nyukinLists.FirstOrDefault(item => item.SeqNo == model.SeqNo);
            if (nyukin != null)
            {
                nyukin.UpdateDate = dateTimeNow;
                nyukin.UpdateId = userId;
                if (nyukin.SortNo != model.SortNo
                    || nyukin.RaiinNo != model.RaiinNo
                    || nyukin.AdjustFutan != model.AdjustFutan
                    || nyukin.NyukinGaku != model.NyukinGaku
                    || nyukin.PaymentMethodCd != model.PaymentMethodCd
                    || nyukin.NyukinDate != model.NyukinDate
                    || nyukin.UketukeSbt != model.UketukeSbt
                    || nyukin.NyukinCmt != model.NyukinCmt
                    || nyukin.NyukinjiSeikyu != model.SeikyuGaku
                    || nyukin.NyukinjiTensu != model.SeikyuTensu
                    || nyukin.NyukinjiDetail != model.SeikyuDetail
                    || model.IsDelete)
                {
                    nyukin.SortNo = model.SortNo;
                    nyukin.RaiinNo = model.RaiinNo;
                    nyukin.AdjustFutan = model.AdjustFutan;
                    nyukin.NyukinGaku = model.NyukinGaku;
                    nyukin.PaymentMethodCd = model.PaymentMethodCd;
                    nyukin.NyukinDate = model.NyukinDate;
                    nyukin.UketukeSbt = model.UketukeSbt;
                    nyukin.NyukinCmt = model.NyukinCmt;
                    nyukin.NyukinjiSeikyu = model.SeikyuGaku;
                    nyukin.NyukinjiTensu = model.SeikyuTensu;
                    nyukin.NyukinjiDetail = model.SeikyuDetail;
                    if (model.IsDelete)
                    {
                        nyukin.IsDeleted = 1;
                    }
                    return nyukin;
                }
            }
        }
        return null;
    }

    public List<SyunoSeikyuModel> GetListSyunoSeikyuModel(List<long> listRaiinNo)
    {
        var result = TrackingDataContext.SyunoSeikyus.Where(item => listRaiinNo.Contains(item.RaiinNo))
                                                             .Select(item => new SyunoSeikyuModel(
                                                                    item.HpId,
                                                                    item.PtId,
                                                                    item.SinDate,
                                                                    item.RaiinNo,
                                                                    item.NyukinKbn,
                                                                    item.SeikyuTensu,
                                                                    item.AdjustFutan,
                                                                    item.SeikyuGaku,
                                                                    item.SeikyuDetail ?? string.Empty,
                                                                    item.NewSeikyuTensu,
                                                                    item.NewAdjustFutan,
                                                                    item.NewSeikyuGaku,
                                                                    item.NewSeikyuDetail ?? string.Empty
                                                             )).ToList();
        return result;
    }

    public List<SyunoNyukinModel> GetListSyunoNyukinModel(List<long> listRaiinNo)
    {
        var result = TrackingDataContext.SyunoNyukin.Where(item => listRaiinNo.Contains(item.RaiinNo) && item.IsDeleted == 0)
                                                             .Select(item => new SyunoNyukinModel(
                                                                    item.HpId,
                                                                    item.PtId,
                                                                    item.SinDate,
                                                                    item.RaiinNo,
                                                                    item.SeqNo,
                                                                    item.SortNo,
                                                                    item.AdjustFutan,
                                                                    item.NyukinGaku,
                                                                    item.PaymentMethodCd,
                                                                    item.NyukinDate,
                                                                    item.UketukeSbt,
                                                                    item.NyukinCmt ?? string.Empty,
                                                                    item.NyukinjiTensu,
                                                                    item.NyukinjiSeikyu,
                                                                    item.NyukinjiDetail ?? string.Empty
                                                             )).ToList();
        return result;
    }

    public bool IsNyukinExisted(int hpId, long ptId, long raiinNo, int sinDate)
    {
        var seikyuList = NoTrackingDataContext.SyunoSeikyus
                         .Where(item => item.HpId == hpId
                                        && item.PtId == ptId
                                        && item.RaiinNo == raiinNo
                                        && item.SinDate == sinDate
                                        && item.NyukinKbn != 0);

        var nyukinList = NoTrackingDataContext.SyunoNyukin
                         .Where(item => item.HpId == hpId
                                        && item.PtId == ptId
                                        && item.RaiinNo == raiinNo
                                        && item.SinDate == sinDate
                                        && item.IsDeleted == 0);

        var query = from seikyu in seikyuList
                    join nyukin in nyukinList on new { seikyu.HpId, seikyu.PtId, seikyu.SinDate, seikyu.RaiinNo }
                                              equals new { nyukin.HpId, nyukin.PtId, nyukin.SinDate, nyukin.RaiinNo }
                    select new
                    {
                        Seikyu = seikyu
                    };

        return query.Any();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
