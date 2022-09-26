using Domain.Models.OrdInfs;
using Domain.Models.SuperSetDetail;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories;

public class SuperSetDetailRepository : ISuperSetDetailRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;
    private readonly AmazonS3Options _options;
    private const string SUSPECTED = "の疑い";
    private const string SUSPECTED_CD = "8002";
    private const string FREE_WORD = "0000999";
    public SuperSetDetailRepository(IOptions<AmazonS3Options> optionsAccessor, ITenantProvider tenantProvider)
    {
        _options = optionsAccessor.Value;
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd, int sindate)
    {
        return new SuperSetDetailModel(
                GetSetByomeiList(hpId, setCd),
                GetSetKarteInfModel(hpId, setCd),
                GetSetGroupOrdInfModel(hpId, setCd, sindate)
            );
    }

    #region GetSetByomeiList
    private List<SetByomeiModel> GetSetByomeiList(int hpId, int setCd)
    {
        var listByomeis = _tenantNoTrackingDataContext.SetByomei.Where(odr => odr.HpId == hpId && odr.SetCd == setCd && odr.IsDeleted != 1).ToList();

        // get list ByomeiMst
        List<string> codeLists = new();
        foreach (var item in listByomeis)
        {
            codeLists.AddRange(GetCodeLists(item));
        }
        var byomeiMstList = _tenantNoTrackingDataContext.ByomeiMsts.Where(b => codeLists.Contains(b.ByomeiCd)).ToList();

        var listSetByomeiModels = listByomeis.Select(mst => ConvertSetByomeiModel(mst, byomeiMstList)).ToList();
        return listSetByomeiModels;
    }

    private SetByomeiModel ConvertSetByomeiModel(SetByomei mst, List<ByomeiMst> byomeiMstList)
    {
        bool isSyobyoKbn = mst.SyobyoKbn == 1;
        int sikkanKbn = mst.SikkanKbn;
        int nanByoCd = mst.NanbyoCd;
        string fullByomei = mst.Byomei ?? string.Empty;
        bool isDspRece = mst.IsNodspRece == 0;
        bool isDspKarte = mst.IsNodspKarte == 0;
        string byomeiCmt = mst.HosokuCmt ?? string.Empty;
        string byomeiCd = mst.ByomeiCd ?? string.Empty;
        var codeLists = GetCodeLists(mst);
        //prefix and suffix
        var prefixSuffixList = codeLists?.Select(code => new PrefixSuffixModel(code, byomeiMstList.FirstOrDefault(item => item.ByomeiCd.Equals(code))?.Byomei ?? string.Empty)).ToList();
        bool isSuspected = false;
        if (codeLists != null)
        {
            isSuspected = codeLists.Any(c => c == "8002");
        }
        return new SetByomeiModel(
                mst.Id,
                isSyobyoKbn,
                sikkanKbn,
                nanByoCd,
                fullByomei,
                isSuspected,
                isDspRece,
                isDspKarte,
                byomeiCmt,
                byomeiCd,
                prefixSuffixList ?? new List<PrefixSuffixModel>()
            );
    }

    private List<string> GetCodeLists(SetByomei mst)
    {
        var codeLists = new List<string>()
            {
                mst.SyusyokuCd1 ?? string.Empty,
                mst.SyusyokuCd2 ?? string.Empty,
                mst.SyusyokuCd3 ?? string.Empty,
                mst.SyusyokuCd4 ?? string.Empty,
                mst.SyusyokuCd5 ?? string.Empty,
                mst.SyusyokuCd6 ?? string.Empty,
                mst.SyusyokuCd7 ?? string.Empty,
                mst.SyusyokuCd8 ?? string.Empty,
                mst.SyusyokuCd9 ?? string.Empty,
                mst.SyusyokuCd10 ?? string.Empty,
                mst.SyusyokuCd11 ?? string.Empty,
                mst.SyusyokuCd12 ?? string.Empty,
                mst.SyusyokuCd13 ?? string.Empty,
                mst.SyusyokuCd14 ?? string.Empty,
                mst.SyusyokuCd15 ?? string.Empty,
                mst.SyusyokuCd16 ?? string.Empty,
                mst.SyusyokuCd17 ?? string.Empty,
                mst.SyusyokuCd18 ?? string.Empty,
                mst.SyusyokuCd19 ?? string.Empty,
                mst.SyusyokuCd20 ?? string.Empty,
                mst.SyusyokuCd21 ?? string.Empty
            };
        return codeLists?.Where(c => c != string.Empty).ToList() ?? new List<string>();
    }

    #endregion

    #region GetSetKarteInfModelList
    private SetKarteInfModel GetSetKarteInfModel(int hpId, int setCd)
    {
        var setKarteInf = _tenantNoTrackingDataContext.SetKarteInf.FirstOrDefault(odr => odr.HpId == hpId && odr.SetCd == setCd && odr.IsDeleted != 1) ?? new SetKarteInf();
        return new SetKarteInfModel(
                setKarteInf.HpId,
                setKarteInf.SetCd,
                setKarteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(setKarteInf.RichText)
            );
    }

    #endregion

    #region GetSetGroupOrdInfModelList
    private List<SetGroupOrderInfModel> GetSetGroupOrdInfModel(int hpId, int setCd, int sindate)
    {
        List<SetGroupOrderInfModel> result = new();
        List<SetOrderInfModel> listSetOrderInfModel = new();

        // Get list SetOrderInf and SetOrderInfDetail
        var allSetOdrInfs = _tenantNoTrackingDataContext.SetOdrInf.Where(order => order.HpId == hpId && order.SetCd == setCd && order.IsDeleted != 1)
                .OrderBy(order => order.OdrKouiKbn)
                .ThenBy(order => order.RpNo)
                .ThenBy(order => order.RpEdaNo)
                .ThenBy(order => order.SortNo)
                .ToList();
        var allSetOdrInfDetails = _tenantNoTrackingDataContext.SetOdrInfDetail.Where(detail => detail.HpId == hpId && detail.SetCd == setCd)?.ToList();

        // Get list to map
        var itemCds = allSetOdrInfDetails?.Select(detail => detail.ItemCd);
        var ipnCds = allSetOdrInfDetails?.Select(detail => detail.IpnCd);
        var tenMsts = _tenantDataContext.TenMsts.Where(t => t.HpId == hpId && t.StartDate <= sindate && t.EndDate >= sindate && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
        var kensaMsts = _tenantDataContext.KensaMsts.Where(kensa => kensa.HpId == hpId && kensa.IsDelete != 1).ToList();
        var yakkas = _tenantDataContext.IpnMinYakkaMsts.Where(ipn => ipn.StartDate <= sindate && ipn.EndDate >= sindate && ipn.IsDeleted != 1 && (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd))).OrderByDescending(e => e.StartDate).ToList();
        var ipnKasanExcludes = _tenantDataContext.ipnKasanExcludes.Where(item => item.HpId == hpId && (item.StartDate <= sindate && item.EndDate >= sindate)).ToList();
        var ipnKasanExcludeItems = _tenantDataContext.ipnKasanExcludeItems.Where(item => item.HpId == hpId && (item.StartDate <= sindate && item.EndDate >= sindate)).ToList();
        var checkKensaIrai = _tenantDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 2019 && item.GrpEdaNo == 0);
        var kensaIrai = checkKensaIrai?.Val ?? 0;
        var checkKensaIraiCondition = _tenantDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 2019 && item.GrpEdaNo == 1);
        var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;
        var listUserId = allSetOdrInfs.Select(user => user.CreateId).ToList();

        if (!(allSetOdrInfs?.Count > 0))
        {
            return result;
        }

        var listOrderInfModels = from odrInf in allSetOdrInfs
                                 join user in _tenantDataContext.UserMsts.Where(u => u.HpId == hpId && listUserId.Contains(u.UserId))
                                 on odrInf.CreateId equals user.UserId into odrUsers
                                 from odrUser in odrUsers.DefaultIfEmpty()
                                 select ConvertToOrderInfModel(odrInf, odrUser?.Name ?? string.Empty);

        // Convert to list SetOrderInfModel
        foreach (var itemOrderModel in listOrderInfModels)
        {
            List<SetOrderInfDetailModel> odrDetailModels = new();

            var setOdrInfDetails = allSetOdrInfDetails?.Where(detail => detail.RpNo == itemOrderModel.RpNo && detail.RpEdaNo == itemOrderModel.RpEdaNo)
                .ToList();

            if (setOdrInfDetails?.Count > 0)
            {
                int count = 0;
                var usage = setOdrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);
                foreach (var odrInfDetail in setOdrInfDetails)
                {
                    var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd);
                    var ten = tenMst?.Ten ?? 0;
                    var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);
                    var alternationIndex = count % 2;
                    var bunkatuKoui = 0;
                    if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                    {
                        bunkatuKoui = usage?.SinKouiKbn ?? 0;
                    }
                    var yakka = yakkas.FirstOrDefault(p => p.IpnNameCd == odrInfDetail.IpnCd)?.Yakka ?? 0;
                    var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);
                    int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst, itemOrderModel.InoutKbn, itemOrderModel.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                    var odrInfDetailModel = ConvertToDetailModel(odrInfDetail, yakka, ten, isGetPriceInYakka, kensaGaichu, bunkatuKoui, itemOrderModel.InoutKbn, alternationIndex, tenMst?.OdrTermVal ?? 0, tenMst?.CnvTermVal ?? 0, tenMst?.YjCd ?? string.Empty, tenMst?.MasterSbt ?? string.Empty);
                    odrDetailModels.Add(odrInfDetailModel);
                    count++;
                }
            }
            itemOrderModel.OrdInfDetails.AddRange(odrDetailModels);
            listSetOrderInfModel.Add(itemOrderModel);
        }

        // Find By Group
        var groupSetOdrInfs = listSetOrderInfModel.GroupBy(odr => new { odr.GroupKoui, odr.InoutKbn, odr.SyohoSbt, odr.SikyuKbn, odr.TosekiKbn, odr.SanteiKbn })
                                          .Select(grp => grp.FirstOrDefault())
                                          .ToList();
        foreach (var groupItem in groupSetOdrInfs)
        {
            // Find By RP
            var rpOdrInfs = listSetOrderInfModel.Where(odrInf => odrInf.GroupKoui == groupItem?.GroupKoui &&
                                                                 odrInf.InoutKbn == groupItem.InoutKbn &&
                                                                 odrInf.SyohoSbt == groupItem.SyohoSbt &&
                                                                 odrInf.SikyuKbn == groupItem.SikyuKbn &&
                                                                 odrInf.TosekiKbn == groupItem.TosekiKbn &&
                                                                 odrInf.SanteiKbn == groupItem.SanteiKbn)
                                               .OrderBy(odrInf => odrInf.SortNo)
                                               .ToList();
            var setGroup = ConvertGroupModel(rpOdrInfs?.FirstOrDefault() ?? new SetOrderInfModel(), rpOdrInfs ?? new List<SetOrderInfModel>());
            result.Add(setGroup);
        }

        return result;
    }

    private SetGroupOrderInfModel ConvertGroupModel(SetOrderInfModel order, List<SetOrderInfModel> setOrdInfModels)
    {
        var inOutName = OdrUtil.GetInOutName(order.OdrKouiKbn, order.InoutKbn);
        var isDrug = (order.OdrKouiKbn >= 20 && order.OdrKouiKbn <= 23) || order.OdrKouiKbn == 28 || order.OdrKouiKbn == 100 || order.OdrKouiKbn == 101;
        var isKensa = (order.OdrKouiKbn >= 60 && order.OdrKouiKbn < 70);
        var sikyuName = string.Empty;
        if (isDrug)
        {
            sikyuName = OdrUtil.GetSikyuName(order.SyohoSbt);
        }
        else if (isKensa)
        {
            sikyuName = OdrUtil.GetSikyuKensa(order.SikyuKbn, order.TosekiKbn);
        }
        var isShowSikyu = !string.IsNullOrEmpty(sikyuName)
                        && sikyuName != "日数" // in case 日数, dosen't need display, Comment #375
                        && sikyuName != "通常";
        var santeiName = string.Empty;
        if (order.SanteiKbn == 1)
        {
            santeiName = "算定外";
        }
        else if (order.SanteiKbn == 2)
        {
            santeiName = "自費算定";
        }
        return new SetGroupOrderInfModel(
                kouiCode: order.OdrKouiKbn,
                groupKouiCode: order.GroupKoui,
                groupName: OdrUtil.GetOdrGroupName(order.OdrKouiKbn),
                isShowInOut: !string.IsNullOrEmpty(inOutName),
                inOutKbn: order.InoutKbn,
                inOutName: inOutName,
                isShowSikyu: isShowSikyu,
                sikyuKbn: order.SikyuKbn,
                tosekiKbn: order.TosekiKbn,
                sikyuName: sikyuName,
                isShowSantei: !string.IsNullOrEmpty(santeiName),
                santeiKbn: order.SanteiKbn,
                santeiName: santeiName,
                syohoSbt: order.SyohoSbt,
                isDrug: isDrug,
                isKensa: isKensa,
                setOrdInfModels: setOrdInfModels
            );
    }

    private SetOrderInfModel ConvertToOrderInfModel(SetOdrInf ordInf, string createName)
    {
        return new SetOrderInfModel(
                    ordInf.Id,
                    ordInf.HpId,
                    ordInf.SetCd,
                    ordInf.RpNo,
                    ordInf.RpEdaNo,
                    ordInf.OdrKouiKbn,
                    ordInf.RpName ?? string.Empty,
                    ordInf.InoutKbn,
                    ordInf.SikyuKbn,
                    ordInf.SyohoSbt,
                    ordInf.SanteiKbn,
                    ordInf.TosekiKbn,
                    ordInf.DaysCnt,
                    ordInf.SortNo,
                    ordInf.IsDeleted,
                    ordInf.CreateDate,
                    ordInf.CreateId,
                    createName,
                    GroupKoui.From(ordInf.OdrKouiKbn),
                    new List<SetOrderInfDetailModel>()
               );
    }

    private SetOrderInfDetailModel ConvertToDetailModel(SetOdrInfDetail ordInfDetail, double yakka, double ten, bool isGetPriceInYakka, int kensaGaichu, int bunkatuKoui, int inOutKbn, int alternationIndex, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt)
    {
        string displayItemName = ordInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu ? ordInfDetail.ItemName + TenUtils.GetBunkatu(ordInfDetail.SinKouiKbn, ordInfDetail.Bunkatu ?? string.Empty) : ordInfDetail.ItemName ?? string.Empty;
        return new SetOrderInfDetailModel(
                        ordInfDetail.HpId,
                        ordInfDetail.SetCd,
                        ordInfDetail.RpNo,
                        ordInfDetail.RpEdaNo,
                        ordInfDetail.RowNo,
                        ordInfDetail.SinKouiKbn,
                        ordInfDetail.ItemCd ?? string.Empty,
                        ordInfDetail.ItemName ?? string.Empty,
                        displayItemName,
                        ordInfDetail.Suryo,
                        ordInfDetail.UnitName ?? string.Empty,
                        ordInfDetail.UnitSbt,
                        ordInfDetail.OdrTermVal,
                        ordInfDetail.KohatuKbn,
                        ordInfDetail.SyohoKbn,
                        ordInfDetail.SyohoLimitKbn,
                        ordInfDetail.DrugKbn,
                        ordInfDetail.YohoKbn,
                        ordInfDetail.Kokuji1 ?? string.Empty,
                        ordInfDetail.Kokuji2 ?? string.Empty,
                        ordInfDetail.IsNodspRece,
                        ordInfDetail.IpnCd ?? string.Empty,
                        ordInfDetail.IpnName ?? string.Empty,
                        ordInfDetail.Bunkatu ?? string.Empty,
                        ordInfDetail.CmtName ?? string.Empty,
                        ordInfDetail.CmtOpt ?? string.Empty,
                        ordInfDetail.FontColor ?? string.Empty,
                        ordInfDetail.CommentNewline,
                        masterSbt,
                        inOutKbn,
                        yakka,
                        isGetPriceInYakka,
                        ten,
                        bunkatuKoui,
                        alternationIndex,
                        kensaGaichu,
                        odrTermVal,
                        cnvTermVal,
                        yjCd
            );
    }

    private bool IsGetPriceInYakka(TenMst? tenMst, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems)
    {
        if (tenMst == null) return false;

        var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.IpnNameCd == tenMst.IpnNameCd);

        var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.ItemCd == tenMst.ItemCd);

        return ipnKasanExclude == null && ipnKasanExcludeItem == null;
    }

    private int GetKensaGaichu(SetOdrInfDetail? odrInfDetail, TenMst? tenMst, int inOutKbn, int odrKouiKbn, KensaMst? kensaMst, int kensaIraiCondition, int kensaIrai)
    {
        if (string.IsNullOrEmpty(odrInfDetail?.ItemCd) &&
               string.IsNullOrEmpty(odrInfDetail?.ItemName?.Trim()) &&
               odrInfDetail?.SinKouiKbn == 0)
        {
            return KensaGaichuTextConst.NONE;
        }

        if (odrInfDetail?.SinKouiKbn == 61 || odrInfDetail?.SinKouiKbn == 64)
        {
            bool kensaCondition;
            if (kensaIraiCondition == 0)
            {
                kensaCondition = (odrInfDetail.SinKouiKbn == 61 || odrInfDetail.SinKouiKbn == 64) && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9";
            }
            else
            {
                kensaCondition = odrInfDetail.SinKouiKbn == 61 && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9" && (tenMst == null ? 0 : tenMst.HandanGrpKbn) != 6;
            }

            if (kensaCondition && inOutKbn == 1)
            {
                int kensaSetting = kensaIrai;
                if (kensaMst == null)
                {
                    if (kensaSetting > 0)
                    {
                        return KensaGaichuTextConst.GAICHU_NONE;
                    }
                }
                else if (string.IsNullOrEmpty(kensaMst.CenterItemCd1)
                    && string.IsNullOrEmpty(kensaMst.CenterItemCd2) && kensaSetting > 1)
                {
                    return KensaGaichuTextConst.GAICHU_NOT_SET;
                }
            }
        }

        if (!string.IsNullOrEmpty(odrInfDetail?.ItemName) && string.IsNullOrEmpty(odrInfDetail.ItemCd))
        {
            if (inOutKbn == 1 && (odrKouiKbn >= 20 && odrKouiKbn <= 23) || odrKouiKbn == 28)
            {
                if (odrInfDetail.IsNodspRece == 0)
                {
                    return KensaGaichuTextConst.IS_DISPLAY_RECE_ON;
                }
            }
            else
            {
                if (odrInfDetail.IsNodspRece == 1)
                {
                    return KensaGaichuTextConst.IS_DISPLAY_RECE_OFF;
                }
            }
        }
        return KensaGaichuTextConst.NONE;
    }
    #endregion

    public int SaveSuperSetDetail(int setCd, int userId, int hpId, List<SetByomeiModel> SetByomeiList, SetKarteInfModel SetKarteInf, List<SetOrderInfModel> ListSetOrdInfModels)
    {
        int status = 0;

        if (!SaveSetByomei(setCd, userId, hpId, SetByomeiList))
        {
            status = 1;
        }
        if (!SaveSetKarte(userId, SetKarteInf))
        {
            status = 2;
        }
        if (!SaveSetOrderInf(setCd, userId, hpId, ListSetOrdInfModels))
        {
            status = 3;
        }
        return status;
    }

    #region SaveSetByomei
    private bool SaveSetByomei(int setCd, int userId, int hpId, List<SetByomeiModel> setByomeiModels)
    {
        bool status = false;
        try
        {
            var listOldSetByomeis = _tenantDataContext.SetByomei.Where(mst => mst.SetCd == setCd && mst.HpId == hpId && mst.IsDeleted != 1).ToList();

            // Add new SetByomei
            var listAddNewSetByomeis = setByomeiModels.Where(model => model.Id == 0).Select(model => ConvertToSetByomeiEntity(setCd, userId, hpId, new SetByomei(), model)).ToList();
            if (listAddNewSetByomeis != null && listAddNewSetByomeis.Count > 0)
            {
                _tenantDataContext.SetByomei.AddRange(listAddNewSetByomeis);
            }

            // Update SetByomei
            foreach (var model in setByomeiModels.Where(model => model.Id != 0).ToList())
            {
                var mst = listOldSetByomeis.FirstOrDefault(mst => mst.Id == model.Id);
                if (mst != null)
                {
                    mst = ConvertToSetByomeiEntity(setCd, userId, hpId, mst, model) ?? new SetByomei();
                }
            }

            // Delete SetByomei
            var listSetByomeiDelete = listOldSetByomeis.Where(mst => !setByomeiModels.Select(model => model.Id).ToList().Contains(mst.Id)).ToList();
            foreach (var mst in listSetByomeiDelete)
            {
                mst.IsDeleted = DeleteTypes.Deleted;
                mst.UpdateDate = DateTime.UtcNow;
                mst.UpdateId = userId;
            }
            _tenantDataContext.SaveChanges();
            status = true;
            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }

    private SetByomei ConvertToSetByomeiEntity(int setCd, int userId, int hpId, SetByomei mst, SetByomeiModel model)
    {
        mst.HpId = hpId;
        mst.SetCd = setCd;
        mst.SyobyoKbn = model.IsSyobyoKbn ? 1 : 0;
        mst.SikkanKbn = model.SikkanKbn;
        mst.NanbyoCd = model.NanByoCd;
        mst.IsNodspRece = model.IsDspRece ? 0 : 1;
        mst.IsNodspKarte = model.IsDspKarte ? 0 : 1;
        mst.HosokuCmt = model.ByomeiCmt ?? string.Empty;
        if (model.FullByomei.StartsWith("//"))
        {
            mst.Byomei = model.FullByomei.Substring(2) ?? string.Empty;
            mst.ByomeiCd = FREE_WORD;
        }
        else
        {
            mst.Byomei = model.FullByomei ?? string.Empty;
            mst.ByomeiCd = model.ByomeiCd ?? string.Empty;
        }

        var listPrefixSuffix = mst.ByomeiCd != FREE_WORD ? model.PrefixSuffixList : new();
        var itemSuspected = listPrefixSuffix.FirstOrDefault(item => item.Code.Equals(SUSPECTED_CD));

        if (itemSuspected != null)
        {
            listPrefixSuffix.Remove(itemSuspected);
        }
        mst.SyusyokuCd1 = listPrefixSuffix.Count > 0 ? listPrefixSuffix[0].Code : string.Empty;
        mst.SyusyokuCd2 = listPrefixSuffix.Count > 1 ? listPrefixSuffix[1].Code : string.Empty;
        mst.SyusyokuCd3 = listPrefixSuffix.Count > 2 ? listPrefixSuffix[2].Code : string.Empty;
        mst.SyusyokuCd4 = listPrefixSuffix.Count > 3 ? listPrefixSuffix[3].Code : string.Empty;
        mst.SyusyokuCd5 = listPrefixSuffix.Count > 4 ? listPrefixSuffix[4].Code : string.Empty;
        mst.SyusyokuCd6 = listPrefixSuffix.Count > 5 ? listPrefixSuffix[5].Code : string.Empty;
        mst.SyusyokuCd7 = listPrefixSuffix.Count > 6 ? listPrefixSuffix[6].Code : string.Empty;
        mst.SyusyokuCd8 = listPrefixSuffix.Count > 7 ? listPrefixSuffix[7].Code : string.Empty;
        mst.SyusyokuCd9 = listPrefixSuffix.Count > 8 ? listPrefixSuffix[8].Code : string.Empty;
        mst.SyusyokuCd10 = listPrefixSuffix.Count > 9 ? listPrefixSuffix[9].Code : string.Empty;
        mst.SyusyokuCd11 = listPrefixSuffix.Count > 10 ? listPrefixSuffix[10].Code : string.Empty;
        mst.SyusyokuCd12 = listPrefixSuffix.Count > 11 ? listPrefixSuffix[11].Code : string.Empty;
        mst.SyusyokuCd13 = listPrefixSuffix.Count > 12 ? listPrefixSuffix[12].Code : string.Empty;
        mst.SyusyokuCd14 = listPrefixSuffix.Count > 13 ? listPrefixSuffix[13].Code : string.Empty;
        mst.SyusyokuCd15 = listPrefixSuffix.Count > 14 ? listPrefixSuffix[14].Code : string.Empty;
        mst.SyusyokuCd16 = listPrefixSuffix.Count > 15 ? listPrefixSuffix[15].Code : string.Empty;
        mst.SyusyokuCd17 = listPrefixSuffix.Count > 16 ? listPrefixSuffix[16].Code : string.Empty;
        mst.SyusyokuCd18 = listPrefixSuffix.Count > 17 ? listPrefixSuffix[17].Code : string.Empty;
        mst.SyusyokuCd19 = listPrefixSuffix.Count > 18 ? listPrefixSuffix[18].Code : string.Empty;
        mst.SyusyokuCd20 = listPrefixSuffix.Count > 19 ? listPrefixSuffix[19].Code : string.Empty;
        mst.SyusyokuCd21 = listPrefixSuffix.Count > 20 ? listPrefixSuffix[20].Code : string.Empty;

        if (model.IsSuspected && mst.ByomeiCd != FREE_WORD && itemSuspected == null)
        {
            mst.SyusyokuCd21 = SUSPECTED_CD;
        }
        else if (!model.IsSuspected && mst.ByomeiCd != FREE_WORD)
        {
            mst.Byomei = mst.Byomei?.Replace(SUSPECTED, string.Empty);
        }

        if (model.Id == 0)
        {
            mst.CreateDate = DateTime.UtcNow;
            mst.CreateId = userId;
        }
        mst.UpdateDate = DateTime.UtcNow;
        mst.UpdateId = userId;
        return mst;
    }
    #endregion

    #region SaveSetKarte
    private bool SaveSetKarte(int userId, SetKarteInfModel model)
    {
        bool status = false;

        try
        {
            // update SetKarte
            var entity = _tenantDataContext.SetKarteInf.FirstOrDefault(mst => mst.SetCd == model.SetCd && mst.HpId == model.HpId && mst.IsDeleted != 1);
            if (entity == null)
            {
                entity = new();
                entity.SetCd = model.SetCd;
                entity.HpId = model.HpId;
                entity.RichText = Encoding.UTF8.GetBytes(model.RichText);
                entity.IsDeleted = 0;
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateId = userId;
                entity.CreateId = userId;
                _tenantDataContext.SetKarteInf.Add(entity);
            }
            else
            {
                entity.RichText = Encoding.UTF8.GetBytes(model.RichText);
                entity.UpdateId = userId;
                entity.UpdateDate = DateTime.UtcNow;
            }

            // if set karte have image, update setKarteImage
            var listKarteImgInfs = _tenantDataContext.SetKarteImgInf.Where(item => item.HpId == model.HpId && item.SetCd == model.SetCd && item.Position <= 0).ToList();
            foreach (var item in listKarteImgInfs.Where(item => model.RichText.Contains(ConvertToLinkImage(item.FileName))).ToList())
            {
                item.Position = 10;
            }

            _tenantDataContext.SaveChanges();
            status = true;

            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }

    private string ConvertToLinkImage(string FileName)
    {
        string link = "src=\"" + _options.BaseAccessUrl + "/" + FileName + "\"";
        return link;
    }
    #endregion

    #region SaveSetOrderInf
    private bool SaveSetOrderInf(int setCd, int userId, int hpId, List<SetOrderInfModel> setOrderInfModels)
    {
        bool status = false;
        try
        {
            // Add new SetOdrInf
            List<SetOdrInf> listSetOdrInfAddNews = new();
            List<SetOdrInfDetail> listSetOdrInfDetailAddNews = new();
            var listAddNewSetOrderModels = setOrderInfModels.Where(model => model.IsDeleted == 0).ToList();
            if (listAddNewSetOrderModels != null && listAddNewSetOrderModels.Count > 0)
            {
                foreach (var model in listAddNewSetOrderModels)
                {
                    var entityMst = ConvertToSetOdrInfEntity(setCd, userId, hpId, new SetOdrInf(), model);
                    entityMst.RpNo = model.RpNo;
                    entityMst.RpEdaNo = model.RpEdaNo + 1;
                    entityMst.SortNo = model.SortNo;
                    entityMst.Id = 0;
                    int rowNo = 1;
                    foreach (var detail in model.OrdInfDetails)
                    {
                        var entityDetail = ConvertToSetOdrInfDetailEntity(setCd, hpId, new SetOdrInfDetail(), detail);
                        entityDetail.RpNo = entityMst.RpNo;
                        entityDetail.RpEdaNo = entityMst.RpEdaNo;
                        entityDetail.RowNo = rowNo;
                        listSetOdrInfDetailAddNews.Add(entityDetail);
                        rowNo++;
                    }
                    listSetOdrInfAddNews.Add(entityMst);
                }
                _tenantDataContext.SetOdrInf.AddRange(listSetOdrInfAddNews);
                _tenantDataContext.SetOdrInfDetail.AddRange(listSetOdrInfDetailAddNews);
            }

            // Delete SetOdrInf
            var listIdDeletes = setOrderInfModels.Where(model => model.IsDeleted != 0 || model.Id > 0).Select(item => item.Id).ToList();
            if (listIdDeletes != null && listIdDeletes.Count > 0)
            {
                List<SetOdrInf> listSetOdrInfDeletes = _tenantDataContext.SetOdrInf.Where(item => item.SetCd == setCd && item.HpId == hpId && listIdDeletes.Contains(item.Id)).ToList();
                foreach (var mst in listSetOdrInfDeletes)
                {
                    mst.IsDeleted = DeleteTypes.Deleted;
                    mst.UpdateDate = DateTime.UtcNow;
                    mst.UpdateId = userId;
                }
            }

            _tenantDataContext.SaveChanges();
            status = true;
            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }

    private SetOdrInf ConvertToSetOdrInfEntity(int setCd, int userId, int hpId, SetOdrInf entity, SetOrderInfModel model)
    {
        entity.SetCd = setCd;
        entity.RpNo = model.RpNo;
        entity.RpEdaNo = model.RpEdaNo;
        entity.HpId = hpId;
        entity.OdrKouiKbn = model.OdrKouiKbn;
        entity.RpName = model.RpName;
        entity.InoutKbn = model.InoutKbn;
        entity.SikyuKbn = model.SikyuKbn;
        entity.SyohoSbt = model.SyohoSbt;
        entity.SanteiKbn = model.SanteiKbn;
        entity.TosekiKbn = model.TosekiKbn;
        entity.DaysCnt = model.DaysCnt;
        entity.SortNo = model.SortNo;
        entity.UpdateDate = DateTime.UtcNow;
        entity.UpdateId = userId;
        if (entity.Id == 0)
        {
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateId = userId;
            entity.IsDeleted = 0;
        }
        return entity;
    }

    private SetOdrInfDetail ConvertToSetOdrInfDetailEntity(int setCd, int hpId, SetOdrInfDetail entity, SetOrderInfDetailModel model)
    {
        entity.SetCd = setCd;
        entity.HpId = hpId;
        entity.RpNo = model.RpNo;
        entity.RpEdaNo = model.RpEdaNo;
        entity.SinKouiKbn = model.SinKouiKbn;
        entity.ItemCd = model.ItemCd;
        entity.ItemName = model.ItemName;
        entity.Suryo = model.Suryo;
        entity.UnitName = model.UnitName;
        entity.UnitSbt = model.UnitSBT;
        entity.OdrTermVal = model.OdrTermVal;
        entity.KohatuKbn = model.KohatuKbn;
        entity.SyohoKbn = model.SyohoKbn;
        entity.SyohoLimitKbn = model.SyohoLimitKbn;
        entity.DrugKbn = model.DrugKbn;
        entity.YohoKbn = model.YohoKbn;
        entity.Kokuji1 = model.Kokuji1;
        entity.Kokuji2 = model.Kokuji2;
        entity.IsNodspRece = model.IsNodspRece;
        entity.IpnCd = model.IpnCd;
        entity.IpnName = model.IpnName;
        entity.Bunkatu = model.Bunkatu;
        entity.CmtName = model.CmtName;
        entity.CmtOpt = model.CmtOpt;
        entity.FontColor = model.FontColor;
        entity.CommentNewline = model.CommentNewline;
        return entity;
    }
    #endregion

    public bool SaveListSetKarteImgTemp(List<SetKarteImgInfModel> listModel)
    {
        bool status = false;
        try
        {
            var hpId = listModel.FirstOrDefault()?.HpId;
            var setCd = listModel.FirstOrDefault()?.SetCd;
            var listPosition = listModel.Select(item => item.Position).ToList();
            var listOldFileName = listModel.Select(item => item.OldFileName).ToList();
            var listKarteImgInfs = _tenantDataContext.SetKarteImgInf.Where(item => item.HpId == hpId && item.SetCd == setCd && listPosition.Contains(item.Position) && listOldFileName.Contains(item.FileName)).ToList();

            foreach (var model in listModel)
            {
                var karteImgInf = listKarteImgInfs.FirstOrDefault(item => item.SetCd == model.SetCd && item.FileName.Equals(model.OldFileName));
                if (karteImgInf == null)
                {
                    karteImgInf = new SetKarteImgInf();
                    karteImgInf.HpId = model.HpId;
                    karteImgInf.SetCd = model.SetCd;
                    karteImgInf.FileName = model.FileName;
                    karteImgInf.Position = model.Position;
                    _tenantDataContext.SetKarteImgInf.Add(karteImgInf);
                }
                else
                {
                    if (model.FileName != String.Empty)
                    {
                        karteImgInf.Position = model.Position;
                        karteImgInf.FileName = model.FileName;
                    }
                    else
                    {
                        _tenantDataContext.SetKarteImgInf.Remove(karteImgInf);
                    }
                }
            }
            _tenantDataContext.SaveChanges();
            status = true;
            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }

    public List<SetOrderInfModel> GetOnlyListOrderInfModel(int hpId, int setCd)
    {
        var listOrder = _tenantNoTrackingDataContext.SetOdrInf.Where(mst => mst.HpId == hpId && mst.SetCd == setCd).ToList();
        return listOrder.Select(model => ConvertToOrderInfModel(model, string.Empty)).ToList();
    }
}
