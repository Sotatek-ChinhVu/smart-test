using Domain.Constant;
using Domain.Models.OrdInfDetails;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using Domain.Types;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories;

public class SuperSetDetailRepository : RepositoryBase, ISuperSetDetailRepository
{
    private readonly AmazonS3Options _options;
    private readonly ISetMstRepository _setMstRepository;
    private const string SUSPECTED = "の疑い";
    private const string SUSPECTED_CD = "8002";
    private const string FREE_WORD = "0000999";

    public SuperSetDetailRepository(IOptions<AmazonS3Options> optionsAccessor, ITenantProvider tenantProvider, ISetMstRepository setMstRepository) : base(tenantProvider)
    {
        _options = optionsAccessor.Value;
        _setMstRepository = setMstRepository;
    }

    public SuperSetDetailModel GetSuperSetDetail(int hpId, int userId, int setCd, int sindate)
    {
        return new SuperSetDetailModel(
                GetSetByomeiList(hpId, setCd),
                GetSetKarteInfModel(hpId, setCd),
                GetSetGroupOrdInfModel(hpId, userId, setCd, sindate),
                GetListSetKarteFileModel(hpId, setCd)
            );
    }

    public (List<SetByomeiModel> byomeis, List<SetKarteInfModel> karteInfs, List<SetOrderInfModel> orderInfModels, List<(int setCd, List<SetFileInfModel> setFiles)> setFileInfModels) GetSuperSetDetailForTodayOrder(int hpId, int userId, int setCd, int sinDate)
    {
        var rootSuperSet = NoTrackingDataContext.SetMsts.FirstOrDefault(s => s.SetCd == setCd && s.HpId == hpId && s.IsDeleted == DeleteTypes.None);
        List<int> setCds;
        if (rootSuperSet == null) return (new(), new(), new(), new());

        if (rootSuperSet.Level2 == 0)
            setCds = NoTrackingDataContext.SetMsts.Where(s => s.HpId == hpId && s.Level1 == rootSuperSet.Level1 && s.SetKbn == rootSuperSet.SetKbn && s.IsDeleted == DeleteTypes.None && s.GenerationId == rootSuperSet.GenerationId && rootSuperSet.SetKbnEdaNo == s.SetKbnEdaNo).Select(s => s.SetCd).ToList();
        else if (rootSuperSet.Level3 == 0)
            setCds = NoTrackingDataContext.SetMsts.Where(s => s.HpId == hpId && s.Level1 == rootSuperSet.Level1 && s.Level2 == rootSuperSet.Level2 && s.SetKbn == rootSuperSet.SetKbn && s.IsDeleted == DeleteTypes.None && s.GenerationId == rootSuperSet.GenerationId && rootSuperSet.SetKbnEdaNo == s.SetKbnEdaNo).Select(s => s.SetCd).ToList();
        else
            setCds = NoTrackingDataContext.SetMsts.Where(s => s.HpId == hpId && s.Level1 == rootSuperSet.Level1 && s.Level2 == rootSuperSet.Level2 && rootSuperSet.Level3 == s.Level3 && s.SetKbn == rootSuperSet.SetKbn && s.IsDeleted == DeleteTypes.None && s.GenerationId == rootSuperSet.GenerationId && s.SetKbnEdaNo == rootSuperSet.SetKbnEdaNo).Select(s => s.SetCd).ToList();

        var allSetByomeis = NoTrackingDataContext.SetByomei.Where(b => b.HpId == hpId && setCds.Contains(b.SetCd) && b.IsDeleted == DeleteTypes.None).ToList();
        var allKarteFiles = NoTrackingDataContext.SetKarteImgInf.Where(k => k.HpId == hpId && setCds.Contains(k.SetCd)).ToList();
        List<(int setCd, long seqNo)> lastSeqNos = new();
        foreach (var setCdItem in setCds)
        {
            var lastSeq = allKarteFiles.Where(item => item.HpId == hpId && item.SetCd == setCdItem).Select(item => item.SeqNo)?.DefaultIfEmpty(0).Max() ?? 0;
            if (lastSeq > 0)
            {
                lastSeqNos.Add(new(setCdItem, lastSeq));
            }
        }
        lastSeqNos = lastSeqNos.Distinct().ToList();

        List<string> codeLists = new();
        foreach (var item in allSetByomeis)
        {
            codeLists.AddRange(GetCodeLists(item));
            codeLists.Add(item.ByomeiCd ?? string.Empty);
        }
        var allByomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => b.HpId == hpId && codeLists.Contains(b.ByomeiCd)).ToList();
        var allKarteInfs = NoTrackingDataContext.SetKarteInf.Where(k => k.HpId == hpId && setCds.Contains(k.SetCd) && k.KarteKbn == 1 && k.IsDeleted == DeleteTypes.None).ToList();
        var allSetOrderInfs = NoTrackingDataContext.SetOdrInf.Where(o => o.HpId == hpId && setCds.Contains(o.SetCd) && o.IsDeleted == DeleteTypes.None).ToList() ?? new();
        var allSetOrderInfDetails = NoTrackingDataContext.SetOdrInfDetail.Where(o => o.HpId == hpId && setCds.Contains(o.SetCd)).ToList() ?? new();
        var itemCds = allSetOrderInfDetails?.Select(detail => detail.ItemCd);
        var ipnCds = allSetOrderInfDetails?.Select(detail => detail.IpnCd);
        var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.StartDate <= sinDate && t.EndDate >= sinDate && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
        var kensaMsts = NoTrackingDataContext.KensaMsts.Where(kensa => kensa.HpId == hpId && kensa.IsDelete != 1).ToList();
        var ipnKansanMsts = NoTrackingDataContext.IpnKasanMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.StartDate <= sinDate && ipn.IsDeleted == 0).ToList();
        var yakkas = NoTrackingDataContext.IpnMinYakkaMsts.Where(ipn => ipn.StartDate <= sinDate && ipn.EndDate >= sinDate && ipn.IsDeleted != 1 && (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd))).OrderByDescending(e => e.StartDate).ToList();
        var ipnKasanExcludes = NoTrackingDataContext.ipnKasanExcludes.Where(item => item.StartDate <= sinDate && item.EndDate >= sinDate).ToList();
        var ipnKasanExcludeItems = NoTrackingDataContext.ipnKasanExcludeItems.Where(item => item.StartDate <= sinDate && item.EndDate >= sinDate).ToList();
        var checkKensaIrai = NoTrackingDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 2019 && item.GrpEdaNo == 0);
        var kensaIrai = checkKensaIrai?.Val ?? 0;
        var checkKensaIraiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 2019 && item.GrpEdaNo == 1);
        var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;
        var allIpnNameMsts = NoTrackingDataContext.IpnNameMsts.Where(p =>
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   (ipnCds != null && ipnCds.Contains(p.IpnNameCd))).ToList();
        var settingValues = GetSettingValues(hpId);
        var sinKouiKbns = allSetOrderInfDetails?.Select(od => od.SinKouiKbn).Distinct().ToList() ?? new();
        var yohoSetMsts = NoTrackingDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
        var itemCdYohos = yohoSetMsts?.Select(od => od.ItemCd ?? string.Empty);
        var tenMstYohos = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sinDate && t.EndDate >= sinDate && (sinKouiKbns != null && sinKouiKbns.Contains(t.SinKouiKbn)) && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

        List<SetByomeiModel> byomeis = new();
        List<SetKarteInfModel> karteInfs = new();
        List<(int, List<SetFileInfModel>)> karteFiles = new();
        List<SetOrderInfModel> ordInfs = new();
        var byomeiObj = new object();
        var karteObj = new object();
        var orderObj = new object();
        var karteFileObj = new object();
        Parallel.ForEach(setCds, currentSetCd =>
        {
            var taskByomei = Task<List<SetByomeiModel>>.Factory.StartNew(() => ExcuGetByomeisForEachDetailItem(currentSetCd, byomeiObj, allSetByomeis, allByomeiMstList));
            var taskKarte = Task<SetKarteInfModel?>.Factory.StartNew(() => ExcuGetKarteForEachDetailItem(currentSetCd, karteObj, allKarteInfs));
            var taskOrder = Task<List<SetOrderInfModel>>.Factory.StartNew(() => ExcuGetOrderForEachDetailItem(currentSetCd, orderObj, hpId, allSetOrderInfs, allSetOrderInfDetails ?? new(), tenMsts, kensaMsts, yakkas, ipnKasanExcludes, ipnKasanExcludeItems, allIpnNameMsts, ipnKansanMsts, yohoSetMsts ?? new(), tenMstYohos, settingValues, kensaIrai, kensaIraiCondition));
            var taskKarteFile = Task<List<SetFileInfModel>>.Factory.StartNew(() => ExcuGetKarteFileForEachDetailItem(currentSetCd, karteFileObj, allKarteFiles, lastSeqNos));

            Task.WaitAll(taskByomei, taskKarte, taskOrder, taskKarteFile);

            var karteInf = taskKarte.Result;
            var karteFileOfItem = taskKarteFile.Result;

            byomeis.AddRange(taskByomei.Result);
            if (karteInf != null)
                karteInfs.Add(karteInf);
            if (karteFileOfItem != null && karteFileOfItem.Count > 0)
                karteFiles.Add(new(currentSetCd, karteFileOfItem));
            ordInfs.AddRange(taskOrder.Result);
        });

        return new(byomeis, karteInfs, ordInfs, karteFiles);
    }

    private List<SetByomeiModel> ExcuGetByomeisForEachDetailItem(int setCd, object byomeiObj, List<SetByomei> allSetByomeis, List<ByomeiMst> allByomeiMstList)
    {
        var currentSetByomeis = allSetByomeis.Where(b => b.SetCd == setCd).ToList();
        var byomeis = new List<SetByomeiModel>();
        List<string> currentCodeLists = new();

        foreach (var item in currentSetByomeis)
        {
            currentCodeLists.AddRange(GetCodeLists(item));
            currentCodeLists.Add(item.ByomeiCd ?? string.Empty);
        }
        var byomeiMstList = allByomeiMstList.Where(b => currentCodeLists.Contains(b.ByomeiCd)).ToList();
        lock (byomeiObj)
        {
            byomeis.AddRange(currentSetByomeis.Select(mst => ConvertSetByomeiModel(mst, byomeiMstList)));
        }

        return byomeis;
    }

    private SetKarteInfModel? ExcuGetKarteForEachDetailItem(int setCd, object karteObj, List<SetKarteInf> allKarteInfs)
    {
        var setKarteInf = allKarteInfs.FirstOrDefault(k => k.SetCd == setCd);

        if (setKarteInf != null)
            lock (karteObj)
            {
                return new SetKarteInfModel(
                    setKarteInf.HpId,
                    setKarteInf.SetCd,
                    setKarteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(setKarteInf.RichText),
                    string.Empty
                    );
            }

        return null;
    }

    private List<SetFileInfModel> ExcuGetKarteFileForEachDetailItem(int setCd, object karteFileObj, List<SetKarteImgInf> allKarteFiles, List<(int setCd, long seqNo)> lastSeqNos)
    {
        long lastSeqNo = lastSeqNos.FirstOrDefault(s => s.setCd == setCd).seqNo;
        var result = allKarteFiles.Where(item => item.SetCd == setCd && item.SeqNo == lastSeqNo && item.FileName != string.Empty).OrderBy(item => item.Position)
                       .Select(item => new SetFileInfModel(item.KarteKbn > 0, item.FileName ?? string.Empty)).ToList();
        lock (karteFileObj)
        {
            return result;
        }
    }

    private List<SetOrderInfModel> ExcuGetOrderForEachDetailItem(int setCd, object orderObj, int hpId, List<SetOdrInf> setOdrInfs, List<SetOdrInfDetail> setOdrInfDetails, List<TenMst> tenMsts, List<KensaMst> kensaMsts, List<IpnMinYakkaMst> yakkas, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems, List<IpnNameMst> allIpnNameMsts, List<IpnKasanMst> ipnKansanMsts, List<YohoSetMst> yohoSetMsts, List<TenMst> tenMstYohos, Dictionary<string, int> settingValues, double kensaIrai, double kensaIraiCondition)
    {
        List<SetOrderInfModel> ordInfs = new();

        lock (orderObj)
        {
            ordInfs.AddRange(GetSetOrdInfModel(hpId, setCd, setOdrInfs ?? new(), setOdrInfDetails ?? new(), tenMsts, kensaMsts, yakkas, ipnKasanExcludes, ipnKasanExcludeItems, allIpnNameMsts, ipnKansanMsts, yohoSetMsts, tenMstYohos, settingValues, kensaIrai, kensaIraiCondition));
        }

        return ordInfs;
    }


    #region GetSetByomeiList
    private List<SetByomeiModel> GetSetByomeiList(int hpId, int setCd)
    {
        var listByomeis = NoTrackingDataContext.SetByomei.Where(odr => odr.HpId == hpId && odr.SetCd == setCd && odr.IsDeleted != 1).ToList();

        // get list ByomeiMst
        List<string> codeLists = new();
        foreach (var item in listByomeis)
        {
            codeLists.AddRange(GetCodeLists(item));
            codeLists.Add(item.ByomeiCd ?? string.Empty);
        }
        var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => codeLists.Contains(b.ByomeiCd)).ToList();

        var listSetByomeiModels = listByomeis.Select(mst => ConvertSetByomeiModel(mst, byomeiMstList)).ToList();
        return listSetByomeiModels;
    }

    private SetByomeiModel ConvertSetByomeiModel(SetByomei mst, List<ByomeiMst> byomeiMstList)
    {
        bool isSyobyoKbn = mst.SyobyoKbn == 1;
        int sikkanKbn = mst.SikkanKbn;
        int nanByoCd = mst.NanbyoCd;
        string displayByomei = mst.Byomei ?? string.Empty; // displayByomei is saved in the database
        bool isDspRece = mst.IsNodspRece == 0;
        bool isDspKarte = mst.IsNodspKarte == 0;
        string byomeiCmt = mst.HosokuCmt ?? string.Empty;
        string byomeiCd = mst.ByomeiCd ?? string.Empty;

        // fullByomei is main byomei get by byomeiCd
        string fullByomei = mst.ByomeiCd != FREE_WORD ? byomeiMstList.FirstOrDefault(item => item.ByomeiCd.Equals(mst.ByomeiCd))?.Byomei ?? string.Empty : displayByomei;
        var codeLists = GetCodeLists(mst);

        //prefix and suffix
        var prefixSuffixList = codeLists?.Select(code => new PrefixSuffixModel(code, byomeiMstList.FirstOrDefault(item => item.ByomeiCd.Equals(code))?.Byomei ?? string.Empty)).ToList();
        bool isSuspected = false;
        if (codeLists != null)
        {
            isSuspected = codeLists.Any(c => c == "8002");
            codeLists.Add(mst.ByomeiCd ?? string.Empty);
        }
        codeLists = codeLists?.Distinct().ToList();
        var byomeiMst = byomeiMstList.FirstOrDefault(b => codeLists?.Contains(b.ByomeiCd) == true) ?? new();
        return new SetByomeiModel(
                mst.Id,
                isSyobyoKbn,
                sikkanKbn,
                nanByoCd,
                displayByomei,
                fullByomei,
                isSuspected,
                isDspRece,
                isDspKarte,
                byomeiCmt,
                byomeiCd,
                byomeiMst?.Icd101 ?? string.Empty,
                byomeiMst?.Icd1012013 ?? string.Empty,
                byomeiMst?.Icd1012013 ?? string.Empty,
                byomeiMst?.Icd1022013 ?? string.Empty,
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

        return codeLists?.Where(c => c != string.Empty).Distinct().ToList() ?? new List<string>();
    }

    #endregion

    #region GetSetKarteInfModelList
    private SetKarteInfModel GetSetKarteInfModel(int hpId, int setCd)
    {
        var setKarteInf = NoTrackingDataContext.SetKarteInf.Where(odr => odr.HpId == hpId && odr.SetCd == setCd && odr.KarteKbn == 1 && odr.IsDeleted != 1).OrderByDescending(o => o.SeqNo).FirstOrDefault() ?? new SetKarteInf();
        return new SetKarteInfModel(
                setKarteInf.HpId,
                setKarteInf.SetCd,
                setKarteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(setKarteInf.RichText),
                string.Empty
            );
    }

    private List<SetFileInfModel> GetListSetKarteFileModel(int hpId, int setCd)
    {
        long lastSeqNo = GetLastSeqNo(hpId, setCd);
        var result = NoTrackingDataContext.SetKarteImgInf.Where(item =>
                                                                    item.HpId == hpId
                                                                    && item.SetCd == setCd
                                                                    && item.SeqNo == lastSeqNo
                                                                    && item.FileName != string.Empty)
                                                                .OrderBy(item => item.Position)
                                                                .Select(item => new SetFileInfModel(
                                                                   item.KarteKbn > 0,
                                                                   item.FileName ?? string.Empty
                                                                 )).ToList();
        return result;
    }

    #endregion

    #region GetSetGroupOrdInfModelList
    private List<SetGroupOrderInfModel> GetSetGroupOrdInfModel(int hpId, int userId, int setCd, int sindate)
    {
        List<SetGroupOrderInfModel> result = new();
        List<SetOrderInfModel> listSetOrderInfModel = new();

        // Get list SetOrderInf and SetOrderInfDetail
        var allSetOdrInfs = NoTrackingDataContext.SetOdrInf.Where(order => order.HpId == hpId && order.SetCd == setCd && order.IsDeleted != 1)
                .OrderBy(order => order.OdrKouiKbn)
                .ThenBy(order => order.RpNo)
                .ThenBy(order => order.RpEdaNo)
                .ThenBy(order => order.SortNo)
                .ToList();
        var allSetOdrInfDetails = NoTrackingDataContext.SetOdrInfDetail.Where(detail => detail.HpId == hpId && detail.SetCd == setCd)?.ToList();

        // Get list to map
        var itemCds = allSetOdrInfDetails?.Select(detail => detail.ItemCd);
        var ipnCds = allSetOdrInfDetails?.Select(detail => detail.IpnCd);
        var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.StartDate <= sindate && t.EndDate >= sindate && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
        var kensaMsts = NoTrackingDataContext.KensaMsts.Where(kensa => kensa.HpId == hpId && kensa.IsDelete != 1).ToList();
        var yakkas = NoTrackingDataContext.IpnMinYakkaMsts.Where(ipn => ipn.StartDate <= sindate && ipn.EndDate >= sindate && ipn.IsDeleted != 1 && (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd))).OrderByDescending(e => e.StartDate).ToList();
        var ipnKasanExcludes = NoTrackingDataContext.ipnKasanExcludes.Where(item => item.StartDate <= sindate && item.EndDate >= sindate).ToList();
        var ipnKasanExcludeItems = NoTrackingDataContext.ipnKasanExcludeItems.Where(item => item.StartDate <= sindate && item.EndDate >= sindate).ToList();
        var checkKensaIrai = NoTrackingDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 2019 && item.GrpEdaNo == 0);
        var kensaIrai = checkKensaIrai?.Val ?? 0;
        var checkKensaIraiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 2019 && item.GrpEdaNo == 1);
        var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;
        var listUserId = allSetOdrInfs.Select(user => user.CreateId).ToList();
        var ipnKansanMsts = NoTrackingDataContext.IpnKasanMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.StartDate <= sindate && ipn.IsDeleted == 0).ToList();
        var yohoSetMsts = NoTrackingDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
        var sinKouiKbns = allSetOdrInfDetails?.Select(od => od.SinKouiKbn).Distinct().ToList() ?? new();
        var itemCdYohos = yohoSetMsts?.Select(od => od.ItemCd ?? string.Empty);
        var tenMstYohos = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sindate && t.EndDate >= sindate && (sinKouiKbns != null && sinKouiKbns.Contains(t.SinKouiKbn)) && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

        if (!(allSetOdrInfs?.Count > 0))
        {
            return result;
        }

        var listOrderInfModels = from odrInf in allSetOdrInfs
                                 join user in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && listUserId.Contains(u.UserId))
                                 on odrInf.CreateId equals user.UserId into odrUsers
                                 from odrUser in odrUsers.DefaultIfEmpty()
                                 select ConvertToOrderInfModel(odrInf, odrUser?.Sname ?? string.Empty);

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
                    var ipnKansanMst = ipnKansanMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);
                    int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst, itemOrderModel.InoutKbn, itemOrderModel.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                    var yohoSets = GetListYohoSetMstModelByUserID(yohoSetMsts ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrInfDetail.SinKouiKbn)?.ToList() ?? new List<TenMst>());
                    var odrInfDetailModel = ConvertToDetailModel(odrInfDetail, kensaMst ?? new(), ipnKansanMst ?? new(), yohoSets, yakka, ten, isGetPriceInYakka, kensaGaichu, bunkatuKoui, itemOrderModel.InoutKbn, alternationIndex, tenMst?.OdrTermVal ?? 0, tenMst?.CnvTermVal ?? 0, tenMst?.YjCd ?? string.Empty, tenMst?.MasterSbt ?? string.Empty, tenMst ?? new());
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

    private List<SetOrderInfModel> GetSetOrdInfModel(int hpId, int setCd, List<SetOdrInf> setOdrInfs, List<SetOdrInfDetail> setOdrInfDetails, List<TenMst> tenMsts, List<KensaMst> kensaMsts, List<IpnMinYakkaMst> yakkas, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems, List<IpnNameMst> allIpnNameMsts, List<IpnKasanMst> ipnKansanMsts, List<YohoSetMst> yohoSetMsts, List<TenMst> tenMstYohos, Dictionary<string, int> settingValues, double kensaIrai, double kensaIraiCondition)
    {
        List<SetOrderInfModel> listSetOrderInfModel = new();

        // Get list SetOrderInf and SetOrderInfDetail
        var allSetOdrInfs = setOdrInfs.Where(order => order.HpId == hpId && order.SetCd == setCd && order.IsDeleted != 1)
                .OrderBy(order => order.OdrKouiKbn)
                .ThenBy(order => order.RpNo)
                .ThenBy(order => order.RpEdaNo)
                .ThenBy(order => order.SortNo)
                .ToList();
        var allSetOdrInfDetails = setOdrInfDetails.Where(detail => detail.HpId == hpId && detail.SetCd == setCd)?.ToList();
        var listUserId = allSetOdrInfs.Select(user => user.CreateId).ToList();

        if (!(allSetOdrInfs?.Count > 0))
        {
            return listSetOrderInfModel;
        }

        var listOrderInfModels = from odrInf in allSetOdrInfs
                                 join user in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId && listUserId.Contains(u.UserId))
                                 on odrInf.CreateId equals user.UserId into odrUsers
                                 from odrUser in odrUsers.DefaultIfEmpty()
                                 select ConvertToOrderInfModel(odrInf, odrUser?.Sname ?? string.Empty);

        // Convert to list SetOrderInfModel
        foreach (var itemOrderModel in listOrderInfModels)
        {
            List<SetOrderInfDetailModel> odrDetailModels = new();

            var currentSetOdrInfDetails = allSetOdrInfDetails?.Where(detail => detail.RpNo == itemOrderModel.RpNo && detail.RpEdaNo == itemOrderModel.RpEdaNo)
                .ToList();

            if (currentSetOdrInfDetails?.Count > 0)
            {
                var usage = setOdrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);
                foreach (var odrInfDetail in currentSetOdrInfDetails)
                {
                    var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd) ?? new TenMst();
                    var ipnNameMst = allIpnNameMsts.FirstOrDefault(i => i.IpnNameCd == tenMst?.IpnNameCd);
                    var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);
                    var bunkatuKoui = 0;
                    if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                    {
                        bunkatuKoui = usage?.SinKouiKbn ?? 0;
                    }
                    var yakka = yakkas.FirstOrDefault(p => p.IpnNameCd == odrInfDetail.IpnCd)?.Yakka ?? 0;
                    var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);
                    int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst, itemOrderModel.InoutKbn, itemOrderModel.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                    var ipnKansanMst = ipnKansanMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);
                    var yohoSets = GetListYohoSetMstModelByUserID(yohoSetMsts ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrInfDetail.SinKouiKbn)?.ToList() ?? new());
                    var odrInfDetailModel = ConvertToDetailModel(odrInfDetail, settingValues, yakka, tenMst ?? new(), ipnNameMst ?? new(), ipnKansanMst ?? new(), kensaMst ?? new(), yohoSets, isGetPriceInYakka, kensaGaichu, bunkatuKoui, itemOrderModel.InoutKbn, itemOrderModel.OdrKouiKbn);
                    odrDetailModels.Add(odrInfDetailModel);
                }
            }
            itemOrderModel.OrdInfDetails.AddRange(odrDetailModels);
            listSetOrderInfModel.Add(itemOrderModel);
        }

        return listSetOrderInfModel;
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

    private SetOrderInfDetailModel ConvertToDetailModel(SetOdrInfDetail ordInfDetail, KensaMst kensaMst, IpnKasanMst ipnKansanMst, List<YohoSetMstModel> yohoSets, double yakka, double ten, bool isGetPriceInYakka, int kensaGaichu, int bunkatuKoui, int inOutKbn, int alternationIndex, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt, TenMst tenMst)
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
                        yjCd,
                        kensaMst?.CenterItemCd1 ?? string.Empty,
                        kensaMst?.CenterItemCd2 ?? string.Empty,
                        ipnKansanMst?.Kasan1 ?? 0,
                        ipnKansanMst?.Kasan2 ?? 0,
                        yohoSets,
                        tenMst.CmtColKeta1,
                        tenMst.CmtColKeta2,
                        tenMst.CmtColKeta3,
                        tenMst.CmtColKeta4,
                        tenMst.CmtCol1,
                        tenMst.CmtCol2,
                        tenMst.CmtCol3,
                        tenMst.CmtCol4,
                        tenMst.HandanGrpKbn,
                        kensaMst == null
            );
    }

    private SetOrderInfDetailModel ConvertToDetailModel(SetOdrInfDetail ordInfDetail, Dictionary<string, int> settingValues, double yakka, TenMst tenMst, IpnNameMst ipnNameMst, IpnKasanMst ipnKansanMst, KensaMst kensaMst, List<YohoSetMstModel> yohoSets, bool isGetPriceInYakka, int kensaGaichu, int bunkatuKoui, int inOutKbn, int odrInfOdrKouiKbn)
    {
        var syohoKbn = CalculateSyoho(ordInfDetail, settingValues, tenMst, odrInfOdrKouiKbn, ipnNameMst.IpnName ?? string.Empty);
        var termVal = CorrectTermVal(ordInfDetail.UnitSbt, tenMst, ordInfDetail.OdrTermVal);

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
                        tenMst.KohatuKbn == 0 ? ordInfDetail.KohatuKbn : tenMst.KohatuKbn,
                        syohoKbn.Item1,
                        syohoKbn.Item2,
                        ordInfDetail.DrugKbn,
                        ordInfDetail.YohoKbn,
                        tenMst.Kokuji1 ?? string.Empty,
                        tenMst.Kokuji2 ?? string.Empty,
                        ordInfDetail.IsNodspRece,
                        tenMst.IpnNameCd ?? string.Empty,
                        ipnNameMst.IpnName ?? string.Empty,
                        ordInfDetail.Bunkatu ?? string.Empty,
                        ordInfDetail.CmtName ?? string.Empty,
                        ordInfDetail.CmtOpt ?? string.Empty,
                        ordInfDetail.FontColor ?? string.Empty,
                        ordInfDetail.CommentNewline,
                        tenMst.MasterSbt ?? string.Empty,
                        inOutKbn,
                        yakka,
                        isGetPriceInYakka,
                        tenMst.Ten,
                        bunkatuKoui,
                        0,
                        kensaGaichu,
                        tenMst.OdrTermVal,
                        termVal,
                        tenMst.YjCd ?? string.Empty,
                        kensaMst?.CenterItemCd1 ?? string.Empty,
                        kensaMst?.CenterItemCd2 ?? string.Empty,
                        ipnKansanMst?.Kasan1 ?? 0,
                        ipnKansanMst?.Kasan2 ?? 0,
                        yohoSets,
                        tenMst.CmtColKeta1,
                        tenMst.CmtColKeta2,
                        tenMst.CmtColKeta3,
                        tenMst.CmtColKeta4,
                        tenMst.CmtCol1,
                        tenMst.CmtCol2,
                        tenMst.CmtCol3,
                        tenMst.CmtCol4,
                        tenMst.HandanGrpKbn,
                        kensaMst == null
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
                kensaCondition = (odrInfDetail.SinKouiKbn == 61 || odrInfDetail.SinKouiKbn == 64) && odrInfDetail.Kokuji2 != "7" && odrInfDetail.Kokuji2 != "9";
            }
            else
            {
                kensaCondition = odrInfDetail.SinKouiKbn == 61 && odrInfDetail.Kokuji2 != "7" && odrInfDetail.Kokuji2 != "9" && (tenMst == null ? 0 : tenMst.HandanGrpKbn) != 6;
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
        var listOldSetByomeis = TrackingDataContext.SetByomei.Where(mst => mst.SetCd == setCd && mst.HpId == hpId && mst.IsDeleted != 1).ToList();

        // Add new SetByomei
        var listAddNewSetByomeis = setByomeiModels.Where(model => model.Id == 0).Select(model => ConvertToSetByomeiEntity(setCd, userId, hpId, new SetByomei(), model)).ToList();
        if (listAddNewSetByomeis != null && listAddNewSetByomeis.Any())
        {
            TrackingDataContext.SetByomei.AddRange(listAddNewSetByomeis);
        }

        // Update SetByomei
        foreach (var model in setByomeiModels.Where(model => model.Id != 0).ToList())
        {
            var mst = listOldSetByomeis.FirstOrDefault(mst => mst.Id == model.Id);
            if (mst != null)
            {
                mst = ConvertToSetByomeiEntity(setCd, userId, hpId, mst, model) ?? new();
            }
        }

        // Delete SetByomei
        var listSetByomeiDelete = listOldSetByomeis.Where(mst => !setByomeiModels.Select(model => model.Id).ToList().Contains(mst.Id)).ToList();
        foreach (var mst in listSetByomeiDelete)
        {
            mst.IsDeleted = DeleteTypes.Deleted;
            mst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            mst.UpdateId = userId;
        }
        TrackingDataContext.SaveChanges();
        status = true;
        return status;
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

        // if item IsSuspected, alway set ByomeiCd = SUSPECTED_CD in SyusyokuCd21
        if (model.IsSuspected && mst.ByomeiCd != FREE_WORD)
        {
            mst.SyusyokuCd21 = SUSPECTED_CD;
        }
        else if (!model.IsSuspected && mst.ByomeiCd != FREE_WORD)
        {
            mst.Byomei = mst.Byomei?.Replace(SUSPECTED, string.Empty);
        }

        if (model.Id == 0)
        {
            mst.CreateDate = CIUtil.GetJapanDateTimeNow();
            mst.CreateId = userId;
        }
        mst.UpdateDate = CIUtil.GetJapanDateTimeNow();
        mst.UpdateId = userId;
        return mst;
    }
    #endregion

    #region SaveSetKarte
    private bool SaveSetKarte(int userId, SetKarteInfModel model)
    {
        bool status = false;
        // update SetKarte
        var entity = TrackingDataContext.SetKarteInf.FirstOrDefault(mst => mst.SetCd == model.SetCd && mst.HpId == model.HpId && mst.IsDeleted != 1 && mst.KarteKbn == 1);
        if (entity == null)
        {
            if (!string.IsNullOrEmpty(model.Text) && !string.IsNullOrEmpty(model.RichText))
            {
                entity = new();
                entity.SetCd = model.SetCd;
                entity.HpId = model.HpId;
                entity.RichText = Encoding.UTF8.GetBytes(model.RichText);
                entity.IsDeleted = 0;
                entity.KarteKbn = 1;
                entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
                entity.CreateId = userId;
                entity.Text = model.Text;
                TrackingDataContext.SetKarteInf.Add(entity);
            }
        }
        else
        {
            if (entity.IsDeleted == DeleteTypes.None && model.Text != entity.Text || (entity.RichText != null && model.RichText != Encoding.UTF8.GetString(entity.RichText)))
            {
                entity.RichText = Encoding.UTF8.GetBytes(model.RichText);
                entity.Text = model.Text;
                entity.UpdateId = userId;
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            }
        }

        // if set karte have image, update setKarteImage
        var listKarteImgInfs = TrackingDataContext.SetKarteImgInf.Where(item => item.HpId == model.HpId && item.SetCd == model.SetCd && item.Position <= 0).ToList();
        foreach (var item in listKarteImgInfs.Where(item => model.RichText.Contains(ConvertToLinkImage(item.FileName ?? string.Empty))).ToList())
        {
            item.Position = 10;
        }

        TrackingDataContext.SaveChanges();
        status = true;

        return status;
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
        // Add new SetOdrInf
        List<SetOdrInf> listSetOdrInfAddNews = new();
        List<SetOdrInfDetail> listSetOdrInfDetailAddNews = new();
        var listAddNewSetOrderModels = setOrderInfModels.Where(model => model.IsDeleted == 0).ToList();
        if (listAddNewSetOrderModels != null && listAddNewSetOrderModels.Count > 0)
        {
            int plusRpNo = 1;
            foreach (var model in listAddNewSetOrderModels)
            {
                var entityMst = ConvertToSetOdrInfEntity(setCd, userId, hpId, new SetOdrInf(), model);
                entityMst.RpNo = model.RpNo;
                entityMst.RpEdaNo = model.RpEdaNo + 1;
                if (entityMst.RpNo == 0)
                {
                    entityMst.RpNo = GetMaxRpNo(setCd, hpId) + plusRpNo;
                    entityMst.RpEdaNo = 1;
                    plusRpNo++;
                }
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
            TrackingDataContext.SetOdrInf.AddRange(listSetOdrInfAddNews);
            TrackingDataContext.SetOdrInfDetail.AddRange(listSetOdrInfDetailAddNews);
        }

        // Delete SetOdrInf
        var listIdDeletes = setOrderInfModels.Where(model => model.IsDeleted != 0 || model.Id > 0).Select(item => item.Id).ToList();
        if (listIdDeletes != null && listIdDeletes.Count > 0)
        {
            List<SetOdrInf> listSetOdrInfDeletes = TrackingDataContext.SetOdrInf.Where(item => item.SetCd == setCd && item.HpId == hpId && listIdDeletes.Contains(item.Id)).ToList();
            foreach (var mst in listSetOdrInfDeletes)
            {
                mst.IsDeleted = DeleteTypes.Deleted;
                mst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                mst.UpdateId = userId;
            }
        }

        TrackingDataContext.SaveChanges();
        status = true;
        return status;
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
        entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        entity.UpdateId = userId;
        if (entity.Id == 0)
        {
            entity.CreateDate = CIUtil.GetJapanDateTimeNow();
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

    public List<SetOrderInfModel> GetOnlyListOrderInfModel(int hpId, int setCd)
    {
        var listOrder = NoTrackingDataContext.SetOdrInf.Where(mst => mst.HpId == hpId && mst.SetCd == setCd).ToList();
        return listOrder.Select(model => ConvertToOrderInfModel(model, string.Empty)).ToList();
    }

    public long GetMaxRpNo(int setCd, int hpId)
    {
        if (setCd <= 0)
        {
            return 0;
        }

        var result = NoTrackingDataContext.SetOdrInf.Where(k => k.HpId == hpId && k.SetCd == setCd).ToList();
        if (result.Any())
        {
            return result.Max(item => item.RpNo);
        }
        return 0;
    }

    private (int, int) CalculateSyoho(SetOdrInfDetail odrDetail, Dictionary<string, int> settingValues, TenMst tenMst, int odrInfOdrKouiKbn, string ipnName)
    {
        int syohoKbn = 0;
        int syohoLimitKbn = 0;

        if ((odrDetail.SinKouiKbn == 20 && odrDetail.DrugKbn > 0) || (((odrInfOdrKouiKbn >= 20 && odrInfOdrKouiKbn <= 23) || odrInfOdrKouiKbn == 28) && odrDetail.SinKouiKbn == 30))
        {
            bool isChangeKouhatu = odrDetail.KohatuKbn != tenMst.KohatuKbn;
            if (isChangeKouhatu)
            {
                switch (tenMst.KohatuKbn)
                {
                    case 0:
                        // 先発品
                        syohoKbn = 0;
                        syohoLimitKbn = 0;
                        break;
                    case 1:
                        // 後発品
                        syohoKbn = settingValues["autoSetSyohoKbnKohatuDrug"] + 1;
                        syohoLimitKbn = settingValues["autoSetSyohoLimitKohatuDrug"];
                        break;
                    case 2:
                        // 後発品のある先発品
                        syohoKbn = settingValues["autoSetSyohoKbnSenpatuDrug"] + 1;
                        syohoLimitKbn = settingValues["autoSetSyohoLimitSenpatuDrug"];
                        break;
                }
                if (odrDetail.SyohoKbn == 3 && string.IsNullOrEmpty(ipnName))
                {
                    // 一般名マスタに登録がない
                    syohoKbn = 2;
                }
            }
        }

        if ((odrDetail.SinKouiKbn == 20 && odrDetail.DrugKbn > 0) || (((odrInfOdrKouiKbn >= 20 && odrInfOdrKouiKbn <= 23) || odrInfOdrKouiKbn == 28) && odrDetail.SinKouiKbn == 30))
        {
            switch (tenMst.KohatuKbn)
            {
                case 0:
                    // 先発品
                    syohoKbn = 0;
                    syohoLimitKbn = 0;
                    break;
                case 1:
                    // 後発品
                    if (settingValues["autoSetKohatu"] == 0)
                    {
                        //マスタ設定に準じる
                        syohoKbn = settingValues["autoSetSyohoKbnKohatuDrug"] + 1;
                        syohoLimitKbn = settingValues["autoSetSyohoLimitKohatuDrug"];
                    }
                    else
                    {
                        //各セットの設定に準じる
                        syohoKbn = odrDetail.SyohoKbn;
                        syohoLimitKbn = odrDetail.SyohoLimitKbn;
                    }
                    if (syohoKbn == 0 && settingValues["autoSetSyohoKbnKohatuDrug"] == 2 && !string.IsNullOrEmpty(ipnName))
                    {
                        syohoKbn = settingValues["autoSetSyohoKbnKohatuDrug"] + 1;
                    }
                    break;
                case 2:
                    // 後発品のある先発品
                    if (settingValues["autoSetSenpatu"] == 0)
                    {
                        //マスタ設定に準じる
                        syohoKbn = settingValues["autoSetSyohoKbnSenpatuDrug"] + 1;
                        syohoLimitKbn = settingValues["autoSetSyohoLimitSenpatuDrug"];
                    }
                    else
                    {
                        //各セットの設定に準じる
                        syohoKbn = odrDetail.SyohoKbn;
                        syohoLimitKbn = odrDetail.SyohoLimitKbn;
                    }
                    if (syohoKbn == 0 && settingValues["autoSetSyohoKbnSenpatuDrug"] == 2 && !string.IsNullOrEmpty(ipnName))
                    {
                        syohoKbn = settingValues["autoSetSyohoKbnSenpatuDrug"] + 1;
                    }
                    break;
            }

            if (tenMst != null && syohoKbn == 3 && string.IsNullOrEmpty(ipnName))
            {
                // 一般名マスタに登録がない
                syohoKbn = 2;
            }
        }
        return (syohoKbn, syohoLimitKbn);
    }

    private double GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int defaultValue = 0, bool fromLastestDb = false)
    {
        SystemConf? systemConf;
        if (!fromLastestDb)
        {
            systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
        }
        else
        {
            systemConf = NoTrackingDataContext.SystemConfs.Where(p =>
                p.HpId == hpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault();
        }
        return systemConf != null ? systemConf.Val : defaultValue;
    }

    private Dictionary<string, int> GetSettingValues(int hpId)
    {
        Dictionary<string, int> result = new()
        {
            { "autoSetSyohoKbnKohatuDrug", (int)GetSettingValue(hpId, 2020, 0) },
            { "autoSetSyohoLimitKohatuDrug", (int)GetSettingValue(hpId, 2020, 1) },
            { "autoSetSyohoKbnSenpatuDrug", (int)GetSettingValue(hpId, 2021, 0) },
            { "autoSetSyohoLimitSenpatuDrug", (int)GetSettingValue(hpId, 2021, 1) },
            { "autoSetKohatu", (int)GetSettingValue(hpId, 2020, 2) },
            { "autoSetSenpatu", (int)GetSettingValue(hpId, 2021, 2) }
        };

        return result;
    }
    private static double CorrectTermVal(int unitSbt, TenMst tenMst, double originTermVal)
    {
        if (tenMst == null) return 0;
        double termVal = originTermVal;
        if (unitSbt == UnitSbtConst.BASIC)
        {
            termVal = tenMst.OdrTermVal;
        }
        else if (unitSbt == UnitSbtConst.CONVERT)
        {
            termVal = tenMst.CnvTermVal;
        }
        return termVal;
    }

    public bool CheckExistSupperSetDetail(int hpId, int setCd)
    {
        return NoTrackingDataContext.SetMsts.Any(item => item.HpId == hpId && item.SetCd == setCd && item.IsDeleted == 0);
    }

    public long GetLastSeqNo(int hpId, int setCd)
    {
        var lastItem = NoTrackingDataContext.SetKarteImgInf.Where(item => item.HpId == hpId && item.SetCd == setCd).ToList()?.MaxBy(item => item.SeqNo);
        return lastItem != null ? lastItem.SeqNo : 0;
    }

    public bool SaveListSetKarteFile(int hpId, int setCd, string host, List<SetFileInfModel> listFiles, bool saveTempFile)
    {
        if (saveTempFile)
        {
            var listInsert = ConvertListAddNewFiles(hpId, host, listFiles);
            if (listInsert.Any())
            {
                TrackingDataContext.SetKarteImgInf.AddRange(listInsert);
            }
        }
        else
        {
            UpdateSeqNoSetFile(hpId, setCd, listFiles.Select(item => new SetFileInfModel(item.IsSchema, item.LinkFile.Replace(host, string.Empty))).ToList());
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    private void UpdateSeqNoSetFile(int hpId, int setCd, List<SetFileInfModel> fileInfModelList)
    {
        var listFileName = fileInfModelList.Select(item => item.LinkFile).Distinct().ToList();
        int position = 1;
        var lastSeqNo = GetLastSeqNo(hpId, setCd);
        var listOldFile = TrackingDataContext.SetKarteImgInf.Where(item =>
                                                                item.HpId == hpId
                                                                && item.SeqNo == lastSeqNo
                                                                && item.SetCd == setCd
                                                                && item.FileName != null
                                                                && listFileName.Contains(item.FileName)
                                                            ).OrderBy(item => item.Position)
                                                            .ToList();

        var listUpdateFiles = TrackingDataContext.SetKarteImgInf.Where(item =>
                                                                item.HpId == hpId
                                                                && item.SeqNo == 0
                                                                && item.SetCd == 0
                                                                && item.FileName != null
                                                                && listFileName.Contains(item.FileName)
                                                            ).ToList();

        foreach (var fileItem in fileInfModelList)
        {
            var oldItemConvert = listOldFile.FirstOrDefault(item => item.SeqNo == lastSeqNo
                                                                    && item.SetCd == setCd
                                                                    && item.FileName != null
                                                                    && item.FileName == fileItem.LinkFile);

            if (oldItemConvert != null)
            {
                SetKarteImgInf newFile = oldItemConvert;
                newFile.Id = 0;
                newFile.SeqNo = lastSeqNo + 1;
                newFile.Position = position;
                TrackingDataContext.SetKarteImgInf.Add(newFile);
                position++;
                continue;
            }

            var oldItemUpdateSeqNo = listUpdateFiles.FirstOrDefault(item => item.SetCd == 0
                                                                            && item.SeqNo == 0
                                                                            && item.FileName != null
                                                                            && item.FileName == fileItem.LinkFile);
            if (oldItemUpdateSeqNo != null)
            {
                oldItemUpdateSeqNo.SetCd = setCd;
                oldItemUpdateSeqNo.SeqNo = lastSeqNo + 1;
                oldItemUpdateSeqNo.Position = position;
                position++;
                continue;
            }

            SetKarteImgInf newItem = new();
            newItem.Id = 0;
            newItem.HpId = hpId;
            newItem.FileName = fileItem.LinkFile;
            newItem.SeqNo = lastSeqNo + 1;
            newItem.Position = position;
            newItem.KarteKbn = fileItem.IsSchema ? 1 : 0;
            newItem.SetCd = setCd;
            TrackingDataContext.SetKarteImgInf.Add(newItem);
            position++;
        }

        if (listFileName.Any(item => item == string.Empty))
        {
            SetKarteImgInf newFile = new();
            newFile.FileName = string.Empty;
            newFile.Id = 0;
            newFile.HpId = hpId;
            newFile.SeqNo = lastSeqNo + 1;
            newFile.Position = 1;
            newFile.SetCd = setCd;
            newFile.KarteKbn = 0;
            TrackingDataContext.SetKarteImgInf.Add(newFile);
        }
    }

    private List<SetKarteImgInf> ConvertListAddNewFiles(int hpId, string host, List<SetFileInfModel> listFiles)
    {
        List<SetKarteImgInf> result = new();
        int position = 1;
        foreach (var item in listFiles)
        {
            result.Add(new SetKarteImgInf()
            {
                Id = 0,
                HpId = hpId,
                SeqNo = 0,
                SetCd = 0,
                KarteKbn = item.IsSchema ? 1 : 0,
                Position = position,
                FileName = item.LinkFile.Replace(host, string.Empty)
            });
            position += 1;
        }
        return result;
    }

    private static List<YohoSetMstModel> GetListYohoSetMstModelByUserID(List<YohoSetMst> listYohoSetMst, List<TenMst> listTenMst)
    {
        var query = from yoho in listYohoSetMst
                    join ten in listTenMst on yoho.ItemCd?.Trim() equals ten.ItemCd.Trim()
                    select new
                    {
                        Yoho = yoho,
                        ItemName = ten.Name,
                        ten.YohoKbn
                    };

        return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
    }

    public bool ClearTempData(int hpId, List<string> listFileNames)
    {
        var listDeletes = TrackingDataContext.SetKarteImgInf.Where(item => item.HpId == hpId
                                                                && item.SeqNo == 0
                                                                && item.SetCd == 0
                                                                && item.FileName != null
                                                                && listFileNames.Contains(item.FileName)
                                                            ).ToList();
        TrackingDataContext.SetKarteImgInf.RemoveRange(listDeletes);
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<ConversionItemInfModel> GetConversionItem(int hpId, string itemCd, int sinDate)
    {
        var conversionItemInfRepo = NoTrackingDataContext.ConversionItemInfs.Where(item => item.HpId == hpId
                                                                                           && item.SourceItemCd == itemCd
                                                                                           && item.IsDeleted == 0);

        var tenMstRepo = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                     && item.StartDate <= sinDate
                                                                     && item.EndDate >= sinDate
                                                                     && item.IsDeleted == DeleteTypes.None);

        var query = from conversionItemInf in conversionItemInfRepo
                    join tenMst in tenMstRepo on conversionItemInf.DestItemCd equals tenMst.ItemCd
                    select new
                    {
                        conversionItemInf,
                        tenMst,
                    };

        return query.AsEnumerable()
                    .Select(item => new ConversionItemInfModel(
                                        item.conversionItemInf.SourceItemCd,
                                        item.conversionItemInf.DestItemCd,
                                        item.conversionItemInf.SortNo,
                                        item.tenMst.ItemCd,
                                        item.tenMst.Name ?? string.Empty,
                                        item.tenMst.DefaultVal,
                                        item.tenMst.Ten,
                                        item.tenMst.HandanGrpKbn,
                                        item.tenMst.MasterSbt ?? string.Empty,
                                        item.tenMst.EndDate,
                                        item.tenMst.KensaItemCd ?? string.Empty,
                                        item.tenMst.KensaItemSeqNo,
                                        item.tenMst.IpnNameCd ?? string.Empty,
                                        item.tenMst.OdrUnitName ?? string.Empty,
                                        item.tenMst.CnvUnitName ?? string.Empty))
                    .OrderBy(item => item.SortNo)
                    .ToList();
    }

    public bool SaveConversionItemInf(int hpId, int userId, string conversionItemCd, string sourceItemCd, List<string> deleteConversionItemCdList)
    {
        var conversionItemInfDBList = TrackingDataContext.ConversionItemInfs.Where(item => item.HpId == hpId
                                                                                           && item.SourceItemCd == sourceItemCd
                                                                                           && item.IsDeleted == 0)
                                                                            .ToList();
        // Delete Item
        foreach (var itemCd in deleteConversionItemCdList)
        {
            var conversionItem = conversionItemInfDBList.FirstOrDefault(item => item.DestItemCd == itemCd);
            if (conversionItem == null)
            {
                continue;
            }

            TrackingDataContext.RemoveRange(conversionItem);
        }
        conversionItemInfDBList = conversionItemInfDBList.Where(item => !deleteConversionItemCdList.Contains(item.DestItemCd)).ToList();

        int sortNo = 1;
        var updateItem = conversionItemInfDBList.FirstOrDefault(item => item.DestItemCd == conversionItemCd);
        bool isAddNew = false;
        if (updateItem == null)
        {
            updateItem = new ConversionItemInf()
            {
                HpId = hpId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                SourceItemCd = sourceItemCd,
                DestItemCd = conversionItemCd,
                IsDeleted = 0,
            };
            isAddNew = true;
        }
        updateItem.SortNo = sortNo;
        updateItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
        updateItem.UpdateId = userId;
        conversionItemInfDBList = conversionItemInfDBList.Where(item => !deleteConversionItemCdList.Contains(item.DestItemCd) && updateItem.DestItemCd != item.DestItemCd).ToList();
        foreach (var conversionItem in conversionItemInfDBList)
        {
            sortNo += 1;
            conversionItem.UpdateId = userId;
            conversionItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            conversionItem.SortNo = sortNo;
        }
        if (isAddNew)
        {
            TrackingDataContext.ConversionItemInfs.Add(updateItem);
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<OdrSetNameModel> GetOdrSetName(int hpId, SetCheckBoxStatusModel checkBoxStatus, int generationId, int timeExpired, string itemName)
    {
        var listSetKbn = GetListSetKbn(checkBoxStatus);

        if (listSetKbn.Count <= 0 || !CheckTargetSetOdrInfDetail(checkBoxStatus))
        {
            return new();
        }

        // Input expired time and check to free comment only => just ignore all free comment
        if (CheckFreeCommentOnly(checkBoxStatus) && timeExpired != 0)
        {
            return new();
        }

        var setMstRepo = NoTrackingDataContext.SetMsts
                        .Where(item => item.HpId == hpId
                                       && item.IsDeleted == 0
                                       && item.GenerationId == generationId
                                       && listSetKbn.Contains(item.SetKbn))
                        .ToList();
        var setCdList = setMstRepo.Select(item => item.SetCd).Distinct().ToList();

        var setOdrInfRepo = NoTrackingDataContext.SetOdrInf.Where(item => item.HpId == hpId
                                                                          && item.IsDeleted == 0
                                                                          && setCdList.Contains(item.SetCd))
                                                           .ToList();

        // Free comment only
        if (CheckFreeCommentOnly(checkBoxStatus))
        {
            var listDetailFreeCommentOnly = GetOdrSetNameFreeComment(hpId, checkBoxStatus, generationId, itemName, timeExpired == 0);

            var listSetMstFreeComment = listDetailFreeCommentOnly.DistinctBy(item => item.SetCd).ToList();

            var querySetFreeComment = (from setMstDetail in listSetMstFreeComment
                                       from setMst in setMstRepo.Where(item =>
                                       (item.SetKbn == setMstDetail.SetKbn && item.SetKbnEdaNo == setMstDetail.SetKbnEdaNo && item.Level1 == setMstDetail.Level1 && item.Level2 == 0 && item.Level3 == 0) ||
                                       (item.SetKbn == setMstDetail.SetKbn && item.SetKbnEdaNo == setMstDetail.SetKbnEdaNo && item.Level1 == setMstDetail.Level1 && item.Level2 == setMstDetail.Level2 && item.Level3 == 0) ||
                                       (item.SetKbn == setMstDetail.SetKbn && item.SetKbnEdaNo == setMstDetail.SetKbnEdaNo && item.Level1 == setMstDetail.Level1 && item.Level2 == setMstDetail.Level2 && item.Level3 == setMstDetail.Level3))
                                       join setOdrInf in setOdrInfRepo on
                                       setMst.SetCd equals setOdrInf.SetCd into setOdrInfList
                                       from setOdrInfItem in setOdrInfList.DefaultIfEmpty()
                                       select new
                                       {
                                           SetMst = setMst,
                                           SetOdrInf = setOdrInfItem,
                                       })
                                      .ToList();

            var listSetNameFreeComment = querySetFreeComment
                                        .Select(item => ConvertToOdrSetNameModel(item.SetMst, item.SetOdrInf, null, null, 0))
                                        .DistinctBy(item => new { item.SetCd, item.SetOrdInfId });

            return listSetNameFreeComment
                   .Union(listDetailFreeCommentOnly)
                   .OrderBy(item => item.SetKbn)
                   .ThenBy(item => item.SetKbnEdaNo)
                   .ThenBy(item => item.Level1)
                   .ThenBy(item => item.Level2)
                   .ThenBy(item => item.Level3)
                   .ThenBy(item => item.SetCd)
                   .ThenBy(item => item.SortNo)
                   .ThenBy(item => item.RowNo)
                   .ToList();
        }

        // For all other case
        var setOdrInfDetailRepo = NoTrackingDataContext.SetOdrInfDetail.Where(item => item.HpId == hpId
                                                                                      && item.ItemCd != null
                                                                                      && item.ItemCd.Replace("　", string.Empty).Replace(" ", string.Empty) != string.Empty
                                                                                      && setCdList.Contains(item.SetCd));

        if (!string.IsNullOrWhiteSpace(itemName))
        {
            setOdrInfDetailRepo = setOdrInfDetailRepo.Where(item => item.ItemName != null && item.ItemName.Contains(itemName));
        }

        var setOdrInfDetailList = setOdrInfDetailRepo.ToList();

        bool isQueryAll = timeExpired == 0;
        if (timeExpired == 0)
        {
            timeExpired = CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();
        }

        var itemCdList = setOdrInfDetailList.Select(item => item.ItemCd).Distinct().ToList();
        var tenMstRepo = NoTrackingDataContext.TenMsts
                         .Where(item => item.HpId == hpId
                                        && item.StartDate <= timeExpired
                                        && item.EndDate >= timeExpired
                                        && itemCdList.Contains(item.ItemCd)
                                        && item.IsDeleted == DeleteTypes.None)
                         .ToList();

        var detailResult = (from setMst in setMstRepo
                            join setOdrInf in setOdrInfRepo on
                                setMst.SetCd equals setOdrInf.SetCd
                            join setOdrInfDetail in setOdrInfDetailList on
                                new { setOdrInf.SetCd, setOdrInf.RpNo, setOdrInf.RpEdaNo } equals
                                new { setOdrInfDetail.SetCd, setOdrInfDetail.RpNo, setOdrInfDetail.RpEdaNo }
                            join tenMst in tenMstRepo on
                                setOdrInfDetail.ItemCd equals tenMst.ItemCd into tenMstList
                            from tenMstItem in tenMstList.DefaultIfEmpty()
                            where tenMstItem == null
                            select new
                            {
                                SetMst = setMst,
                                SetOdrInf = setOdrInf,
                                SetOdrInfDetail = setOdrInfDetail,
                            })
                            .ToList();

        var listItemCd = detailResult.Select(item => item.SetOdrInfDetail.ItemCd).ToList();

        var tenMstRepoResult = NoTrackingDataContext.TenMsts
                               .Where(item => item.HpId == hpId
                                              && listItemCd.Contains(item.ItemCd)
                                              && item.IsDeleted == DeleteTypes.None)
                               .OrderByDescending(item => item.EndDate)
                               .GroupBy(item => item.ItemCd)
                               .Select(item => item.First())
                               .ToList();

        var listDetail = from detail in detailResult
                         join tenMst in tenMstRepoResult on
                           detail.SetOdrInfDetail.ItemCd equals tenMst.ItemCd
                         select ConvertToOdrSetNameModel(detail.SetMst, detail.SetOdrInf, detail.SetOdrInfDetail, tenMst, 0);

        if (isQueryAll)
        {
            var tenMstAllRepo = NoTrackingDataContext.TenMsts
                                .Where(item => item.HpId == hpId
                                               && item.IsDeleted == DeleteTypes.None
                                               && itemCdList.Contains(item.ItemCd))
                                .ToList();

            var tenMstMaxRepo = (from tenMst in tenMstAllRepo
                                 group tenMst by tenMst.ItemCd into grp
                                 select new
                                 {
                                     ItemCd = grp.Key,
                                     EndDate = grp.Max(tenItem => tenItem.EndDate)
                                 }).ToList();

            var queryDetailAll = (from setMst in setMstRepo
                                  join setOdrInf in setOdrInfRepo on
                                      setMst.SetCd equals setOdrInf.SetCd
                                  join setOdrInfDetail in setOdrInfDetailList on
                                      new { setOdrInf.SetCd, setOdrInf.RpNo, setOdrInf.RpEdaNo } equals
                                      new { setOdrInfDetail.SetCd, setOdrInfDetail.RpNo, setOdrInfDetail.RpEdaNo }
                                  join tenMst in tenMstRepo on
                                      setOdrInfDetail.ItemCd equals tenMst.ItemCd
                                  join tenMstMax in tenMstMaxRepo on
                                      setOdrInfDetail.ItemCd equals tenMstMax.ItemCd
                                  select new
                                  {
                                      SetMst = setMst,
                                      SetOdrInf = setOdrInf,
                                      SetOdrInfDetail = setOdrInfDetail,
                                      TenMst = tenMst,
                                      LastEndDate = tenMstMax.EndDate
                                  })
                .ToList();

            var listDetailAll = queryDetailAll.Select(item => ConvertToOdrSetNameModel(item.SetMst, item.SetOdrInf, item.SetOdrInfDetail, item.TenMst, item.LastEndDate));

            listDetail = listDetail.Union(listDetailAll);
        }

        IEnumerable<OdrSetNameModel>? listDetailResult = null;
        // Check conditions
        if (checkBoxStatus.JihiChecked)
        {
            var jihi = listDetail.Where(item => item.ItemCd.StartsWith("J"));
            listDetailResult = jihi;
        }
        if (checkBoxStatus.TokuChecked)
        {
            var toku = listDetail.Where(item => item.MasterSbt == "T");
            listDetailResult = listDetailResult == null ? toku : listDetailResult.Union(toku);
        }
        if (checkBoxStatus.YohoChecked)
        {
            var yoho = listDetail.Where(item => item.YohoKbn > 0);
            listDetailResult = listDetailResult == null ? yoho : listDetailResult.Union(yoho);
        }
        if (checkBoxStatus.BuiChecked)
        {
            var bui = listDetail.Where(item => item.BuiKbn > 0);
            listDetailResult = listDetailResult == null ? bui : listDetailResult.Union(bui);
        }
        if (checkBoxStatus.KihonChecked)
        {
            var Kihon = listDetail.Where(item => !item.ItemCd.StartsWith("J")
                                                 && (item.MasterSbt != "T")
                                                 && (item.YohoKbn <= 0)
                                                 && (item.BuiKbn <= 0));
            listDetailResult = listDetailResult == null ? Kihon : listDetailResult.Union(Kihon);
        }

        if (listDetailResult == null)
        {
            // Free comment only, but we cover this case above, just return new list
            return new List<OdrSetNameModel>();
        }

        var listDetailFreeComment = GetOdrSetNameFreeComment(hpId, checkBoxStatus, generationId, itemName, isQueryAll);

        var listSetMst = listDetailResult.Union(listDetailFreeComment).DistinctBy(item => item.SetCd).ToList();

        var querySet = (from setMstDetail in listSetMst
                        from setMst in setMstRepo.Where(item =>
                        (item.SetKbn == setMstDetail.SetKbn && item.SetKbnEdaNo == setMstDetail.SetKbnEdaNo && item.Level1 == setMstDetail.Level1 && item.Level2 == 0 && item.Level3 == 0) ||
                        (item.SetKbn == setMstDetail.SetKbn && item.SetKbnEdaNo == setMstDetail.SetKbnEdaNo && item.Level1 == setMstDetail.Level1 && item.Level2 == setMstDetail.Level2 && item.Level3 == 0) ||
                        (item.SetKbn == setMstDetail.SetKbn && item.SetKbnEdaNo == setMstDetail.SetKbnEdaNo && item.Level1 == setMstDetail.Level1 && item.Level2 == setMstDetail.Level2 && item.Level3 == setMstDetail.Level3))
                        join setOdrInf in setOdrInfRepo on
                        setMst.SetCd equals setOdrInf.SetCd into setOdrInfList
                        from setOdrInfItem in setOdrInfList.DefaultIfEmpty()
                        select new
                        {
                            SetMst = setMst,
                            SetOdrInf = setOdrInfItem,
                        })
                        .ToList();

        var listSetName = querySet
                          .Select(item => ConvertToOdrSetNameModel(item.SetMst, item.SetOdrInf, null, null, 0))
                          .DistinctBy(item => new { item.SetCd, item.SetOrdInfId });

        var result = listSetName
                    .Union(listDetailResult)
                    .Union(listDetailFreeComment)
                    .OrderBy(item => item.SetKbn)
                    .ThenBy(item => item.SetKbnEdaNo)
                    .ThenBy(item => item.Level1)
                    .ThenBy(item => item.Level2)
                    .ThenBy(item => item.Level3)
                    .ThenBy(item => item.SetCd)
                    .ThenBy(item => item.SortNo)
                    .ThenBy(item => item.RowNo)
                    .ToList();
        return result;
    }

    public (bool SaveSuccess, List<SetMstModel> SetMstUpdateList) SaveOdrSet(int hpId, int userId, int sinDate, List<OdrSetNameModel> setNameModelList, List<OdrSetNameModel> updateSetNameList)
    {
        var rowNoList = setNameModelList.Select(item => item.RowNo).Distinct().ToList();
        var setCdList = setNameModelList.Select(item => item.SetCd).ToList();
        setCdList.AddRange(updateSetNameList.Select(item => item.SetCd));
        setCdList = setCdList.Distinct().ToList();
        var itemCdList = setNameModelList.Select(item => item.ItemCd).Distinct().ToList();

        var odrInfDbList = TrackingDataContext.SetOdrInf.Where(item => item.HpId == hpId
                                                                       && item.IsDeleted == 0
                                                                       && setCdList.Contains(item.SetCd))
                                                          .ToList();

        var tenMstDBList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                       && itemCdList.Contains(item.ItemCd)
                                                                       && item.StartDate <= sinDate
                                                                       && sinDate <= item.EndDate)
                                                        .ToList();

        var rpNoList = odrInfDbList.Select(item => item.RpNo).Distinct().ToList();
        var rpEdaNoList = odrInfDbList.Select(item => item.RpEdaNo).Distinct().ToList();
        var ipnNameCdList = tenMstDBList.Select(item => item.IpnNameCd).Distinct().ToList();
        var odrInfDetailDbList = TrackingDataContext.SetOdrInfDetail.Where(item => item.HpId == hpId
                                                                                   && setCdList.Contains(item.SetCd)
                                                                                   && rpNoList.Contains(item.RpNo)
                                                                                   && rowNoList.Contains(item.RowNo)
                                                                                   && rpEdaNoList.Contains(item.RpEdaNo))
                                                                     .ToList();
        var ipnNameMstDBList = NoTrackingDataContext.IpnNameMsts.Where(item => item.StartDate <= sinDate
                                                                               && item.EndDate >= sinDate
                                                                               && ipnNameCdList.Contains(item.IpnNameCd))
                                                                .ToList();
        #region update setNameOdr
        foreach (var model in setNameModelList)
        {
            var odrInf = odrInfDbList.FirstOrDefault(item => item.Id == model.SetOrdInfId
                                                             && item.SetCd == model.SetCd);
            if (odrInf == null)
            {
                continue;
            }
            var odrInfDetail = odrInfDetailDbList.FirstOrDefault(item => item.SetCd == model.SetCd
                                                                         && item.RpNo == odrInf.RpNo
                                                                         && item.RpEdaNo == odrInf.RpEdaNo
                                                                         && item.RowNo == model.RowNo);
            if (odrInfDetail == null)
            {
                continue;
            }
            var tenMst = tenMstDBList.FirstOrDefault(item => item.ItemCd == model.ItemCd
                                                             && item.StartDate <= sinDate
                                                             && sinDate <= item.EndDate);
            if (tenMst == null)
            {
                continue;
            }

            odrInfDetail.ItemCd = model.ItemCd;
            odrInfDetail.CmtOpt = model.CmtOpt;
            odrInfDetail.Suryo = model.Quantity;
            odrInfDetail.IpnName = ipnNameMstDBList.FirstOrDefault(item => item.IpnNameCd == tenMst.IpnNameCd)?.IpnName ?? string.Empty;

            if (model.IsCommentMaster)
            {
                odrInfDetail.CmtName = tenMst.Name;
            }
            else
            {
                odrInfDetail.CmtName = string.Empty;
            }

            if (!string.IsNullOrEmpty(tenMst.OdrUnitName))
            {
                odrInfDetail.UnitSbt = 1;
                odrInfDetail.UnitName = tenMst.OdrUnitName;
                odrInfDetail.OdrTermVal = tenMst.OdrTermVal;
            }
            else if (!string.IsNullOrEmpty(tenMst.CnvUnitName))
            {
                odrInfDetail.UnitSbt = 2;
                odrInfDetail.UnitName = tenMst.CnvUnitName;
                odrInfDetail.OdrTermVal = tenMst.CnvTermVal;
            }
            else
            {
                odrInfDetail.UnitSbt = 0;
                odrInfDetail.UnitName = string.Empty;
                odrInfDetail.OdrTermVal = 0;
                odrInfDetail.Suryo = 0;
            }

            odrInfDetail.KohatuKbn = tenMst.KohatuKbn;
            odrInfDetail.YohoKbn = tenMst.YohoKbn;
            odrInfDetail.IpnCd = tenMst.IpnNameCd;
            odrInfDetail.DrugKbn = tenMst.DrugKbn;
            odrInfDetail.ItemName = tenMst.Name;

            if (odrInfDetail.SinKouiKbn == 20 && odrInfDetail.DrugKbn > 0)
            {
                switch (odrInfDetail.KohatuKbn)
                {
                    case 0:
                        // 先発品
                        odrInfDetail.SyohoKbn = 0;
                        odrInfDetail.SyohoLimitKbn = 0;
                        break;
                    // Keep old SyohoKbn and SyohoKbnLimit set from previous step
                    case 1:
                    // 後発品
                    case 2:
                        // 後発品のある先発品
                        break;
                }
                if (odrInfDetail.SyohoKbn == 3 && string.IsNullOrEmpty(odrInfDetail.IpnName))
                {
                    // 一般名マスタに登録がない
                    odrInfDetail.SyohoKbn = 2;
                }
            }
            else
            {
                odrInfDetail.SyohoKbn = 0;
                odrInfDetail.SyohoLimitKbn = 0;
            }
        }
        #endregion

        #region update setName
        List<SetMstModel> setMstUpdateList = new();
        setCdList = updateSetNameList.Select(item => item.SetCd).Distinct().ToList();
        var setMstDBList = TrackingDataContext.SetMsts.Where(item => item.HpId == hpId
                                                                     && item.IsDeleted == 0
                                                                     && setCdList.Contains(item.SetCd))
                                                      .ToList();
        List<int> generationIdList = new();
        foreach (var model in updateSetNameList)
        {
            var setMst = setMstDBList.FirstOrDefault(item => item.SetCd == model.SetCd);
            if (setMst == null)
            {
                continue;
            }
            setMst.SetName = model.SetName;
            setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            setMst.UpdateId = userId;
            generationIdList.Add(setMst.GenerationId);

            // update rpName
            var setOdrInfUpdateRpNameList = odrInfDbList.Where(item => item.SetCd == model.SetCd).ToList();
            foreach (var setOdrInf in setOdrInfUpdateRpNameList)
            {
                setOdrInf.RpName = setMst.SetName;
                setOdrInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                setOdrInf.UpdateId = userId;
            }
        }
        #endregion
        var saveSuccess = TrackingDataContext.SaveChanges() > 0;
        if (saveSuccess)
        {
            foreach (var generationId in generationIdList)
            {
                var setMstByGenarationId = _setMstRepository.ReloadCache(hpId, generationId).ToList();
                foreach (var model in updateSetNameList)
                {
                    var rootSet = setMstDBList.FirstOrDefault(item => item.SetCd == model.SetCd);
                    if (rootSet == null)
                    {
                        continue;
                    }
                    var itemSetList = setMstByGenarationId.Where(item => item.SetKbn == rootSet.SetKbn && item.SetKbnEdaNo == rootSet.SetKbnEdaNo && item.GenerationId == rootSet.GenerationId && (rootSet.Level1 == 0 || (rootSet.Level1 > 0 && item.Level1 == rootSet.Level1))).ToList();
                    setMstUpdateList.AddRange(itemSetList);
                }
            }
        }
        setMstUpdateList = setMstUpdateList.Distinct().ToList();
        return (saveSuccess, setMstUpdateList);
    }

    private List<OdrSetNameModel> GetOdrSetNameFreeComment(int hpId, SetCheckBoxStatusModel checkBoxStatus, int generationId, string itemName, bool isQueryAll)
    {
        if (!checkBoxStatus.FreeCommentChecked)
        {
            return new List<OdrSetNameModel>();
        }
        if (!isQueryAll)
        {
            // If isn't query all, because free comment has EndDate = 99999999 by default => just ignore all free comment
            return new();
        }
        var listSetKbn = GetListSetKbn(checkBoxStatus);

        if (listSetKbn.Count <= 0 || !CheckTargetSetOdrInfDetail(checkBoxStatus))
        {
            return new();
        }

        var setMstRepo = NoTrackingDataContext.SetMsts
                         .Where(item => item.HpId == hpId
                                        && item.IsDeleted == 0
                                        && item.GenerationId == generationId
                                        && listSetKbn.Contains(item.SetKbn));

        var setOdrInfRepo = NoTrackingDataContext.SetOdrInf.Where(item => item.HpId == hpId && item.IsDeleted == 0);

        var setOdrInfDetailRepo = NoTrackingDataContext.SetOdrInfDetail
                                 .Where(item => item.HpId == hpId
                                                && (item.ItemCd == null || item.ItemCd.Replace("　", string.Empty).Replace(" ", string.Empty) == string.Empty));

        if (!string.IsNullOrWhiteSpace(itemName))
        {
            setOdrInfDetailRepo = setOdrInfDetailRepo.Where(item => item.ItemName != null && item.ItemName.Contains(itemName));
        }

        var detailResult = (from setMst in setMstRepo
                            join setOdrInf in setOdrInfRepo on
                                setMst.SetCd equals setOdrInf.SetCd
                            join setOdrInfDetail in setOdrInfDetailRepo on
                                new { setOdrInf.SetCd, setOdrInf.RpNo, setOdrInf.RpEdaNo } equals
                                new { setOdrInfDetail.SetCd, setOdrInfDetail.RpNo, setOdrInfDetail.RpEdaNo }
                            select new
                            {
                                SetMst = setMst,
                                SetOdrInf = setOdrInf,
                                SetOdrInfDetail = setOdrInfDetail,
                            })
                            .ToList();
        return detailResult
               .Select(d => ConvertToOdrSetNameModel(d.SetMst, d.SetOdrInf, d.SetOdrInfDetail, null, 0))
               .ToList();
    }

    private OdrSetNameModel ConvertToOdrSetNameModel(SetMst setMst, SetOdrInf setOdrInf, SetOdrInfDetail? setOdrInfDetail, TenMst? tenMst, int lastEndDate)
    {
        return new OdrSetNameModel(
               setMst.SetCd,
               setMst.SetKbn,
               setMst.SetKbnEdaNo,
               setMst.GenerationId,
               setMst.Level1,
               setMst.Level2,
               setMst.Level3,
               setMst.SetName ?? string.Empty,
               setOdrInfDetail?.RowNo ?? 0,
               setOdrInf?.SortNo ?? 0,
               setOdrInf?.RpName ?? string.Empty,
               setOdrInfDetail?.ItemCd ?? string.Empty,
               setOdrInfDetail?.ItemName ?? string.Empty,
               setOdrInfDetail?.CmtName ?? string.Empty,
               setOdrInfDetail?.CmtOpt ?? string.Empty,
               setOdrInfDetail?.Suryo ?? 0,
               setOdrInfDetail?.UnitName ?? string.Empty,
               setOdrInfDetail?.UnitSbt ?? 0,
               setOdrInfDetail?.OdrTermVal ?? 0,
               setOdrInfDetail?.KohatuKbn ?? 0,
               setOdrInfDetail?.IpnCd ?? string.Empty,
               setOdrInfDetail?.IpnName ?? string.Empty,
               setOdrInfDetail?.DrugKbn ?? 0,
               setOdrInfDetail?.SinKouiKbn ?? 0,
               setOdrInfDetail?.SyohoKbn ?? 0,
               setOdrInfDetail?.SyohoLimitKbn ?? 0,
               tenMst?.Ten ?? 0,
               tenMst?.HandanGrpKbn ?? 0,
               tenMst?.MasterSbt ?? string.Empty,
               tenMst?.StartDate ?? 0,
               lastEndDate > 0 ? lastEndDate : tenMst?.EndDate ?? 99999999,
               setOdrInfDetail?.YohoKbn ?? 0,
               setOdrInf?.OdrKouiKbn ?? 0,
               tenMst?.BuiKbn ?? 0,
               setOdrInf?.Id ?? 0
            );
    }

    private List<int> GetListSetKbn(SetCheckBoxStatusModel checkBoxStatus)
    {
        List<int> listSetKbn = new();
        if (checkBoxStatus.SetKbnChecked1)
        {
            listSetKbn.Add(SetNameConst.SetKbn1);
        }
        if (checkBoxStatus.SetKbnChecked2)
        {
            listSetKbn.Add(SetNameConst.SetKbn2);
        }
        if (checkBoxStatus.SetKbnChecked3)
        {
            listSetKbn.Add(SetNameConst.SetKbn3);
        }
        if (checkBoxStatus.SetKbnChecked4)
        {
            listSetKbn.Add(SetNameConst.SetKbn4);
        }
        if (checkBoxStatus.SetKbnChecked5)
        {
            listSetKbn.Add(SetNameConst.SetKbn5);
        }
        if (checkBoxStatus.SetKbnChecked6)
        {
            listSetKbn.Add(SetNameConst.SetKbn6);
        }
        if (checkBoxStatus.SetKbnChecked7)
        {
            listSetKbn.Add(SetNameConst.SetKbn7);
        }
        if (checkBoxStatus.SetKbnChecked8)
        {
            listSetKbn.Add(SetNameConst.SetKbn8);
        }
        if (checkBoxStatus.SetKbnChecked9)
        {
            listSetKbn.Add(SetNameConst.SetKbn9);
        }
        if (checkBoxStatus.SetKbnChecked10)
        {
            listSetKbn.Add(SetNameConst.SetKbn10);
        }
        return listSetKbn;
    }

    private bool CheckTargetSetOdrInfDetail(SetCheckBoxStatusModel checkBoxStatus)
    {
        if (checkBoxStatus.JihiChecked)
        {
            return true;
        }
        else if (checkBoxStatus.KihonChecked)
        {
            return true;
        }
        else if (checkBoxStatus.TokuChecked)
        {
            return true;
        }
        else if (checkBoxStatus.YohoChecked)
        {
            return true;
        }
        else if (checkBoxStatus.BuiChecked)
        {
            return true;
        }
        else if (checkBoxStatus.FreeCommentChecked)
        {
            return true;
        }
        return false;
    }

    private bool CheckFreeCommentOnly(SetCheckBoxStatusModel checkBoxStatus)
    {
        return checkBoxStatus.FreeCommentChecked &&
               !checkBoxStatus.JihiChecked &&
               !checkBoxStatus.KihonChecked &&
               !checkBoxStatus.TokuChecked &&
               !checkBoxStatus.YohoChecked &&
               !checkBoxStatus.BuiChecked;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
