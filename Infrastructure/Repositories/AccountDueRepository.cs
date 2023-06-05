﻿using Domain.Models.AccountDue;
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
                nyukin != null ? nyukin.SortNo : 0,
                nyukin != null ? nyukin.SeqNo : 0,
                seikyu.SeikyuDetail ?? string.Empty,
                raiinItem.Status,
                seikyu.AdjustFutan,
                seikyu.NewSeikyuDetail ?? string.Empty,
                seikyu.NewSeikyuTensu
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

    public bool SaveAccountDueList(int hpId, long ptId, int userId, int sinDate, List<AccountDueModel> listAccountDues, string kaikeiTime)
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

            foreach (var model in listAccountDues)
            {
                // Update raiin status
                UpdateStatusRaiin(userId, dateTimeNow, model, raiinLists, kaikeiTime);

                // Update left table SyunoSeikyu
                UpdateStatusSyunoSeikyu(userId, dateTimeNow, model, seikyuLists);

                // Update right table SyunoNyukin
                UpdateSyunoNyukin(hpId, ptId, userId, dateTimeNow, model, nyukinLists);
            }

            TrackingDataContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void UpdateStatusRaiin(int userId, DateTime dateTimeNow, AccountDueModel model, List<RaiinInf> raiinLists, string kaikeiTime)
    {
        var raiin = raiinLists.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
        int tempStatus = model.NyukinKbn == 0 ? RaiinState.Waiting : RaiinState.Settled;
        if (raiin != null)
        {
            if (tempStatus != raiin.Status)
            {
                raiin.Status = tempStatus;
                raiin.KaikeiTime = kaikeiTime;
                raiin.UpdateDate = dateTimeNow;
                raiin.UpdateId = userId;
            }

            // update menjo
            if (model.NyukinKbn == 2)
            {
                raiin.UpdateDate = dateTimeNow;
                raiin.UpdateId = userId;
            }
        }
    }

    private void UpdateStatusSyunoSeikyu(int userId, DateTime dateTimeNow, AccountDueModel model, List<SyunoSeikyu> seikyuLists)
    {
        var seikyu = seikyuLists.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
        if (seikyu != null)
        {
            seikyu.NyukinKbn = model.NyukinKbn;
            seikyu.SeikyuGaku = model.SeikyuGaku;
            seikyu.AdjustFutan = model.SeikyuAdjustFutan;
            seikyu.UpdateDate = dateTimeNow;
            seikyu.UpdateId = userId;
        }
    }

    private void UpdateSyunoNyukin(int hpId, long ptId, int userId, DateTime dateTimeNow, AccountDueModel model, List<SyunoNyukin> nyukinLists)
    {
        if (model.SeqNo == 0) // Create new SyunoNyukin
        {
            SyunoNyukin nyukin = new();
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
        }
        else // Update SyunoNyukin
        {
            var nyukin = nyukinLists.FirstOrDefault(item => item.SeqNo == model.SeqNo);
            if (nyukin != null)
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
                nyukin.UpdateDate = dateTimeNow;
                nyukin.UpdateId = userId;
                if (model.IsDelete)
                {
                    nyukin.IsDeleted = 1;
                }
            }
        }
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

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
