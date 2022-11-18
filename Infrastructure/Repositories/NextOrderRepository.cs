using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class NextOrderRepository : INextOrderRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        private readonly TenantDataContext _tenantDataContextTracking;
        public NextOrderRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<RsvkrtByomeiModel> GetByomeis(int hpId, long ptId, long rsvkrtNo, int type)
        {
            var byomeis = new List<RsvkrtByomei>();
            if (type == 0)
            {
                byomeis = _tenantDataContext.RsvkrtByomeis.Where(b => b.HpId == hpId && b.PtId == ptId && b.RsvkrtNo == rsvkrtNo && b.IsDeleted == DeleteTypes.None).ToList();
            }
            List<string> codeLists = new();
            foreach (var item in byomeis)
            {
                codeLists.AddRange(GetCodeLists(item));
            }

            var byomeiMstList = _tenantDataContext.ByomeiMsts.Where(b => codeLists.Contains(b.ByomeiCd)).ToList();

            var byomeiModels = byomeis.Select(b => ConvertByomeiToModel(b, byomeiMstList)).ToList();

            return byomeiModels;
        }

        public RsvkrtKarteInfModel GetKarteInf(int hpId, long ptId, long rsvkrtNo)
        {

            var karteInf = _tenantDataContext.RsvkrtKarteInfs.FirstOrDefault(k => k.HpId == hpId && k.PtId == ptId && k.RsvkrtNo == rsvkrtNo && k.IsDeleted == DeleteTypes.None);

            var karteModel = ConvertKarteInfToModel(karteInf ?? new());

            return karteModel;
        }

        public List<RsvkrtOrderInfModel> GetOrderInfs(int hpId, long ptId, long rsvkrtNo, int sinDate, int userId)
        {
            var orderInfs = _tenantDataContext.RsvkrtOdrInfs.Where(o => o.HpId == hpId && o.PtId == ptId && o.RsvkrtNo == rsvkrtNo && o.IsDeleted == DeleteTypes.None).ToList();
            var orderInfDetails = _tenantDataContext.RsvkrtOdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.RsvkrtNo == rsvkrtNo).ToList();

            var itemCds = orderInfDetails.Select(od => od.ItemCd ?? string.Empty);
            var ipnCds = orderInfDetails.Select(od => od.IpnCd ?? string.Empty);
            var sinKouiKbns = orderInfDetails.Select(od => od.SinKouiKbn);
            var tenMsts = _tenantDataContext.TenMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate) && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
            var kensaMsts = _tenantDataContext.KensaMsts.Where(t => t.HpId == hpId).ToList();
            var yakkas = _tenantDataContext.IpnMinYakkaMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate) && (ipnCds != null && ipnCds.Contains(t.IpnNameCd))).ToList();
            var ipnKasanExcludes = _tenantDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate)).ToList();
            var ipnKasanExcludeItems = _tenantDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate)).ToList();
            var ipnNameMsts = _tenantDataContext.IpnNameMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDate && ipn.EndDate >= sinDate).ToList();
            var ipnKansanMsts = _tenantDataContext.IpnKasanMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDate && ipn.IsDeleted == 0).ToList();
            var listYohoSets = _tenantDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
            var itemCdYohos = listYohoSets?.Select(od => od.ItemCd ?? string.Empty);

            var tenMstYohos = _tenantDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sinDate && t.EndDate >= sinDate && (sinKouiKbns != null && sinKouiKbns.Contains(t.SinKouiKbn)) && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

            var checkKensaIrai = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
            var kensaIrai = checkKensaIrai?.Val ?? 0;
            var checkKensaIraiCondition = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
            var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;

            var oderInfModels = orderInfs.Select(o => ConvertOrderInfToModel(o, orderInfDetails, tenMsts, kensaMsts, yakkas, ipnKasanExcludes, ipnKasanExcludeItems, ipnNameMsts, ipnKansanMsts, listYohoSets ?? new(), tenMstYohos, kensaIrai, kensaIraiCondition)).ToList();

            return oderInfModels;
        }

        public void Upsert(List<NextOrderModel> nextOrderModels)
        {
            foreach (var nextOrderModel in nextOrderModels)
            {
            }
        }

        private RsvkrtByomeiModel ConvertByomeiToModel(RsvkrtByomei byomei, List<ByomeiMst> byomeiMsts)
        {

            var codeLists = GetCodeLists(byomei);
            //prefix and suffix
            var byomeiMst = byomeiMsts.FirstOrDefault(item => codeLists.Contains(item.ByomeiCd)) ?? new ByomeiMst();
            return new RsvkrtByomeiModel(
                    byomei.Id,
                    byomei.HpId,
                    byomei.PtId,
                    byomei.RsvkrtNo,
                    byomei.SeqNo,
                    byomei.ByomeiCd ?? string.Empty,
                    byomei.Byomei ?? string.Empty,
                    byomei.SyobyoKbn,
                    byomei.SikkanKbn,
                    byomei.NanbyoCd,
                    byomei.HosokuCmt ?? string.Empty,
                    byomei.IsNodspRece,
                    byomei.IsNodspKarte,
                    byomei.IsDeleted,
                    GetCodeLists(byomei),
                    byomeiMst.Icd101 ?? string.Empty,
                    byomeiMst.Icd1012013 ?? string.Empty,
                    byomeiMst.Icd1012013 ?? string.Empty,
                    byomeiMst.Icd1022013 ?? string.Empty
                );
        }

        private RsvkrtKarteInfModel ConvertKarteInfToModel(RsvkrtKarteInf karteInf)
        {
            return new RsvkrtKarteInfModel(
                    karteInf.HpId,
                    karteInf.PtId,
                    karteInf.RsvDate,
                    karteInf.RsvkrtNo,
                    karteInf.KarteKbn,
                    karteInf.SeqNo,
                    karteInf.Text ?? string.Empty,
                    karteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(karteInf.RichText),
                    karteInf.IsDeleted
                );
        }

        private RsvkrtOrderInfModel ConvertOrderInfToModel(RsvkrtOdrInf odrInf, List<RsvkrtOdrInfDetail> odrInfDetails, List<TenMst> tenMsts, List<KensaMst> kensaMsts, List<IpnMinYakkaMst> yakkas, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems, List<IpnNameMst> ipnNames, List<IpnKasanMst> ipnKasanMsts, List<YohoSetMst> yohoSetMsts, List<TenMst> tenMstYohos, double kensaIrai, double kensaIraiCondition)
        {

            odrInfDetails = odrInfDetails.Where(od => od.RpNo == odrInf.RpNo && od.RpEdaNo == odrInf.RpEdaNo).ToList();
            int index = 0;
            var odrInfDetailModels = new List<RsvKrtOrderInfDetailModel>();
            var obj = new object();
            Parallel.ForEach(odrInfDetails, odrInfDetail =>
            {
                var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd) ?? new();
                var yakka = yakkas.FirstOrDefault(y => y.IpnNameCd == odrInfDetail.IpnCd) ?? new();
                var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);
                var usage = odrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);
                var bunkatuKoui = 0;
                if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    bunkatuKoui = usage?.SinKouiKbn ?? 0;
                }
                var alternationIndex = index % 2;
                var kensaMst = tenMst == null ? new() : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo) ?? new();
                int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst ?? new(), odrInf.InoutKbn, odrInf.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                var yohoSet = GetListYohoSetMstModelByUserID(yohoSetMsts ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrInfDetail.SinKouiKbn)?.ToList() ?? new List<TenMst>());
                var kasan = ipnKasanMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);

                var odrInfDetailModel = new RsvKrtOrderInfDetailModel(
                         odrInfDetail.HpId,
                         odrInfDetail.PtId,
                         odrInfDetail.RsvkrtNo,
                         odrInfDetail.RpNo,
                         odrInfDetail.RpEdaNo,
                         odrInfDetail.RowNo,
                         odrInfDetail.RsvDate,
                         odrInfDetail.SinKouiKbn,
                         odrInfDetail.ItemCd ?? string.Empty,
                         odrInfDetail.ItemName ?? string.Empty,
                         odrInfDetail.Suryo,
                         odrInfDetail.UnitName ?? string.Empty,
                         odrInfDetail.UnitSbt,
                         odrInfDetail.TermVal,
                         odrInfDetail.KohatuKbn,
                         odrInfDetail.SyohoKbn,
                         odrInfDetail.SyohoLimitKbn,
                         odrInfDetail.DrugKbn,
                         odrInfDetail.YohoKbn,
                         odrInfDetail.Kokuji1 ?? string.Empty,
                         odrInfDetail.Kokuji2 ?? string.Empty,
                         odrInfDetail.IsNodspRece,
                         odrInfDetail.IpnCd ?? string.Empty,
                         odrInfDetail.IpnName ?? string.Empty,
                         odrInfDetail.Bunkatu ?? string.Empty,
                         odrInfDetail.CmtName ?? string.Empty,
                         odrInfDetail.CmtOpt ?? string.Empty,
                         odrInfDetail.FontColor ?? string.Empty,
                         odrInfDetail.CommentNewline,
                         tenMst?.MasterSbt ?? string.Empty,
                         odrInf.InoutKbn,
                         yakka.Yakka,
                         isGetPriceInYakka,
                         tenMst?.Ten ?? 0,
                         bunkatuKoui,
                         alternationIndex,
                         kensaGaichu,
                         0,
                         0,
                         tenMst?.OdrTermVal ?? 0,
                         tenMst?.CnvTermVal ?? 0,
                         tenMst?.YjCd ?? string.Empty,
                         yohoSet,
                         kasan?.Kasan1 ?? 0,
                         kasan?.Kasan2 ?? 0
                        );
                lock (obj)
                {
                    odrInfDetailModels.Add(odrInfDetailModel);
                    index++;
                }
            });
            var createName = _tenantDataContext.UserMsts.FirstOrDefault(u => u.UserId == odrInf.CreateId && u.HpId == odrInf.HpId)?.Sname ?? string.Empty;

            return new RsvkrtOrderInfModel(
                    odrInf.HpId,
                    odrInf.PtId,
                    odrInf.RsvDate,
                    odrInf.RsvkrtNo,
                    odrInf.RpNo,
                    odrInf.RpEdaNo,
                    odrInf.Id,
                    odrInf.HokenPid,
                    odrInf.OdrKouiKbn,
                    odrInf.RpName ?? string.Empty,
                    odrInf.InoutKbn,
                    odrInf.SikyuKbn,
                    odrInf.SyohoSbt,
                    odrInf.SanteiKbn,
                    odrInf.TosekiKbn,
                    odrInf.DaysCnt,
                    odrInf.IsDeleted,
                    odrInf.SortNo,
                    odrInf.CreateDate,
                    odrInf.CreateId,
                    createName,
                    odrInfDetailModels
                );
        }

        private static List<YohoSetMstModel> GetListYohoSetMstModelByUserID(List<YohoSetMst> listYohoSetMst, List<TenMst> listTenMst)
        {
            var query = from yoho in listYohoSetMst
                        join ten in listTenMst on yoho.ItemCd.Trim() equals ten.ItemCd.Trim()
                        select new
                        {
                            Yoho = yoho,
                            ItemName = ten.Name,
                            ten.YohoKbn
                        };

            return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
        }

        private static bool IsGetPriceInYakka(TenMst tenMst, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems)
        {
            if (tenMst == null) return false;

            var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.IpnNameCd == tenMst.IpnNameCd);

            var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.ItemCd == tenMst.ItemCd);

            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        private static int GetKensaGaichu(RsvkrtOdrInfDetail odrInfDetail, TenMst tenMst, int inOutKbn, int odrKouiKbn, KensaMst kensaMst, int kensaIraiCondition, int kensaIrai)
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

        private List<string> GetCodeLists(RsvkrtByomei mst)
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

        public List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted)
        {
            var allRsvkrtMst = _tenantDataContext.RsvkrtMsts.Where(rsv => rsv.HpId == hpId && rsv.PtId == ptId && rsv.RsvkrtKbn == rsvkrtKbn && (isDeleted || rsv.IsDeleted == 0))?.AsEnumerable();

            return allRsvkrtMst?.Select(rsv => ConvertToModel(rsv)).ToList() ?? new List<NextOrderModel>();
        }

        private static NextOrderModel ConvertToModel(RsvkrtMst rsvkrtMst)
        {
            return new NextOrderModel(
                        rsvkrtMst.HpId,
                        rsvkrtMst.PtId,
                        rsvkrtMst.RsvkrtNo,
                        rsvkrtMst.RsvkrtKbn,
                        rsvkrtMst.RsvDate,
                        rsvkrtMst.RsvName ?? string.Empty,
                        rsvkrtMst.IsDeleted,
                        rsvkrtMst.SortNo,
                        new(),
                        new(),
                        new()
                   );
        }

        private static RsvkrtByomei ConvertModelToRsvkrtByomei(int userId, RsvkrtByomeiModel rsvkrtByomeiModel)
        {
            return new RsvkrtByomei
            {
                Id = rsvkrtByomeiModel.Id,
                HpId = rsvkrtByomeiModel.HpId,
                PtId = rsvkrtByomeiModel.PtId,
                RsvkrtNo = rsvkrtByomeiModel.RsvkrtNo,
                SeqNo = rsvkrtByomeiModel.SeqNo,
                ByomeiCd = rsvkrtByomeiModel.ByomeiCd,
                SyusyokuCd1 = rsvkrtByomeiModel.PrefixSuffixList.FirstOrDefault(),
                SyusyokuCd2 = rsvkrtByomeiModel.PrefixSuffixList.Skip(1).FirstOrDefault(),
                SyusyokuCd3 = rsvkrtByomeiModel.PrefixSuffixList.Skip(2).FirstOrDefault(),
                SyusyokuCd4 = rsvkrtByomeiModel.PrefixSuffixList.Skip(3).FirstOrDefault(),
                SyusyokuCd5 = rsvkrtByomeiModel.PrefixSuffixList.Skip(4).FirstOrDefault(),
                SyusyokuCd6 = rsvkrtByomeiModel.PrefixSuffixList.Skip(5).FirstOrDefault(),
                SyusyokuCd7 = rsvkrtByomeiModel.PrefixSuffixList.Skip(6).FirstOrDefault(),
                SyusyokuCd8 = rsvkrtByomeiModel.PrefixSuffixList.Skip(7).FirstOrDefault(),
                SyusyokuCd9 = rsvkrtByomeiModel.PrefixSuffixList.Skip(8).FirstOrDefault(),
                SyusyokuCd10 = rsvkrtByomeiModel.PrefixSuffixList.Skip(9).FirstOrDefault(),
                SyusyokuCd11 = rsvkrtByomeiModel.PrefixSuffixList.Skip(10).FirstOrDefault(),
                SyusyokuCd12 = rsvkrtByomeiModel.PrefixSuffixList.Skip(11).FirstOrDefault(),
                SyusyokuCd13 = rsvkrtByomeiModel.PrefixSuffixList.Skip(12).FirstOrDefault(),
                SyusyokuCd14 = rsvkrtByomeiModel.PrefixSuffixList.Skip(13).FirstOrDefault(),
                SyusyokuCd15 = rsvkrtByomeiModel.PrefixSuffixList.Skip(14).FirstOrDefault(),
                SyusyokuCd16 = rsvkrtByomeiModel.PrefixSuffixList.Skip(15).FirstOrDefault(),
                SyusyokuCd17 = rsvkrtByomeiModel.PrefixSuffixList.Skip(16).FirstOrDefault(),
                SyusyokuCd18 = rsvkrtByomeiModel.PrefixSuffixList.Skip(17).FirstOrDefault(),
                SyusyokuCd19 = rsvkrtByomeiModel.PrefixSuffixList.Skip(18).FirstOrDefault(),
                SyusyokuCd20 = rsvkrtByomeiModel.PrefixSuffixList.Skip(19).FirstOrDefault(),
                SyusyokuCd21 = rsvkrtByomeiModel.PrefixSuffixList.Skip(20).FirstOrDefault(),
                Byomei = rsvkrtByomeiModel.Byomei,
                SyobyoKbn = rsvkrtByomeiModel.SyobyoKbn,
                SikkanKbn = rsvkrtByomeiModel.SikkanKbn,
                NanbyoCd = rsvkrtByomeiModel.NanbyoCd,
                HosokuCmt = rsvkrtByomeiModel.HosokuCmt,
                IsNodspKarte = rsvkrtByomeiModel.IsNodspKarte,
                IsNodspRece = rsvkrtByomeiModel.IsNodspRece,
                IsDeleted = rsvkrtByomeiModel.IsDeleted,
                CreateDate = DateTime.UtcNow,
                CreateId = userId,
                UpdateDate = DateTime.UtcNow,
                UpdateId = userId
            };
        }

        private static RsvkrtKarteInf ConvertModelToRsvkrtKarteInf(int userId, RsvkrtKarteInfModel rsvkrtKarteInfModel)
        {
            return new RsvkrtKarteInf
            {
                HpId = rsvkrtKarteInfModel.HpId,
                PtId = rsvkrtKarteInfModel.PtId,
                RsvDate = rsvkrtKarteInfModel.SinDate,
                RsvkrtNo = rsvkrtKarteInfModel.RaiinNo,
                KarteKbn = rsvkrtKarteInfModel.KarteKbn,
                SeqNo = rsvkrtKarteInfModel.SeqNo,
                Text = rsvkrtKarteInfModel.Text,
                RichText = Encoding.UTF8.GetBytes(rsvkrtKarteInfModel.RichText),
                IsDeleted = rsvkrtKarteInfModel.IsDeleted,
                CreateDate = DateTime.UtcNow,
                CreateId = userId,
                UpdateDate = DateTime.UtcNow,
                UpdateId = userId
            };
        }
    }
}
