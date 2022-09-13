using Domain.Models.InputItem;
using Domain.Models.OrdInf;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class OrdInfRepository : IOrdInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public OrdInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public void Create(OrdInfModel ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ordId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfModel> GetList(int hpId, long ptId, int userId, long raiinNo, int sinDate, bool isDeleted)
        {
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && (isDeleted || odr.IsDeleted == 0))?.ToList();

            var result = ConvertEntityToListOrdInfModel(allOdrInf, allOdrInfDetails, hpId, sinDate, sinDate, userId, false);
            return result;
        }

        public IEnumerable<OrdInfModel> GetList(long ptId, int hpId, int userId, long raiinNo)
        {
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.HpId == hpId && o.RaiinNo == raiinNo)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && odr.RaiinNo == raiinNo)?.ToList();
            var sindateMin = allOdrInf?.Min(o => o.SinDate) ?? 0;
            var sindateMax = allOdrInf?.Max(o => o.SinDate) ?? 0;

            return ConvertEntityToListOrdInfModel(allOdrInf, allOdrInfDetails, hpId, sindateMin, sindateMax, userId);
        }

        public bool CheckExistOrder(long rpNo, long rpEdaNo)
        {
            var check = _tenantDataContext.OdrInfs.Any(o => o.RpNo == rpNo && o.RpEdaNo == rpEdaNo);
            return check;
        }

        public bool CheckIsGetYakkaPrice(int hpId, InputItemModel? tenMst, int sinDate)
        {
            if (tenMst == null) return false;
            var ipnKasanExclude = _tenantDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.IpnNameCd == tenMst.IpnNameCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();

            var ipnKasanExcludeItem = _tenantDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.ItemCd == tenMst.ItemCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();
            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        public IpnMinYakkaMstModel FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate)
        {
            var yakkaMst = _tenantDataContext.IpnMinYakkaMsts.Where(p =>
                  p.HpId == hpId &&
                  p.StartDate <= sinDate &&
                  p.EndDate >= sinDate &&
                  p.IpnNameCd == ipnNameCd)
              .FirstOrDefault();

            if (yakkaMst != null)
            {
                return new IpnMinYakkaMstModel(
                        yakkaMst.Id,
                        yakkaMst.HpId,
                        yakkaMst.IpnNameCd,
                        yakkaMst.StartDate,
                        yakkaMst.EndDate,
                        yakkaMst.Yakka,
                        yakkaMst.SeqNo,
                        yakkaMst.IsDeleted
                    );
            }

            return new IpnMinYakkaMstModel(0, 0, string.Empty, 0, 0, 0, 0, 0);
        }

        private List<OrdInfModel> ConvertEntityToListOrdInfModel(List<OdrInf>? allOdrInf, List<OdrInfDetail>? allOdrInfDetails, int hpId, int sinDateMin, int sinDateMax, int userId, bool isHistory = true)
        {
            var result = new List<OrdInfModel>();
            var itemCds = allOdrInfDetails?.Select(od => od.ItemCd ?? string.Empty);
            var ipnCds = allOdrInfDetails?.Select(od => od.IpnCd ?? string.Empty);
            var sinKouiKbns = allOdrInfDetails?.Select(od => od.SinKouiKbn);
            var tenMsts = _tenantDataContext.TenMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax) && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
            var kensaMsts = _tenantDataContext.KensaMsts.Where(t => t.HpId == hpId).ToList();
            var yakkas = _tenantDataContext.IpnMinYakkaMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMax && t.EndDate >= sinDateMax) && (ipnCds != null && ipnCds.Contains(t.IpnNameCd))).ToList();
            var ipnKasanExcludes = _tenantDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax)).ToList();
            var ipnKasanExcludeItems = _tenantDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId && (t.StartDate <= sinDateMin && t.EndDate >= sinDateMax)).ToList();
            var ipnNameMsts = isHistory ? null : _tenantDataContext.IpnNameMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDateMin && ipn.EndDate >= sinDateMax).ToList();
            var ipnKansanMsts = isHistory ? null : _tenantDataContext.IpnKasanMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDateMin && ipn.IsDeleted == 0).ToList();
            var listYohoSets = isHistory ? null : _tenantDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
            var itemCdYohos = isHistory ? null : listYohoSets?.Select(od => od.ItemCd ?? string.Empty);

            var tenMstYohos = isHistory ? null : _tenantDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sinDateMin && t.EndDate >= sinDateMax && (sinKouiKbns != null && sinKouiKbns.Contains(t.SinKouiKbn)) && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

            var checkKensaIrai = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
            var kensaIrai = checkKensaIrai?.Val ?? 0;
            var checkKensaIraiCondition = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
            var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;


            if (!(allOdrInf?.Count > 0))
            {
                return result;
            }

            var odrInfs = from odrInf in allOdrInf
                          join user in _tenantDataContext.UserMsts.Where(u => u.HpId == hpId)
                          on odrInf.CreateId equals user.UserId into odrUsers
                          from odrUser in odrUsers.DefaultIfEmpty()
                          select ConvertToModel(odrInf, odrUser?.Sname ?? string.Empty);

            foreach (var rpOdrInf in odrInfs)
            {
                var odrDetailModels = new List<OrdInfDetailModel>();

                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();

                if (odrInfDetails?.Count > 0)
                {
                    int count = 0;
                    var usage = odrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);

                    foreach (var odrInfDetail in odrInfDetails)
                    {
                        var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd);
                        var ten = tenMst?.Ten ?? 0;
                        if (tenMst != null && string.IsNullOrEmpty(odrInfDetail.IpnCd)) odrInfDetail.IpnCd = tenMst.IpnNameCd;

                        var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);

                        var ipnNameMst = ipnNameMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);
                        if (tenMst != null && string.IsNullOrEmpty(odrInfDetail.IpnCd) && ipnNameMst != null) odrInfDetail.IpnName = ipnNameMst.IpnName;

                        var kasan = ipnKansanMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);

                        var alternationIndex = count % 2;
                        var bunkatuKoui = 0;
                        if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                        {
                            bunkatuKoui = usage?.SinKouiKbn ?? 0;
                        }

                        var yakka = yakkas.FirstOrDefault(p => p.IpnNameCd == odrInfDetail.IpnCd)?.Yakka ?? 0;

                        var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);

                        int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst, rpOdrInf.InoutKbn, rpOdrInf.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                        var odrInfDetailModel = ConvertToDetailModel(odrInfDetail, yakka, ten, isGetPriceInYakka, kensaGaichu, bunkatuKoui, rpOdrInf.InoutKbn, alternationIndex, tenMst?.OdrTermVal ?? 0, tenMst?.CnvTermVal ?? 0, tenMst?.YjCd ?? string.Empty, tenMst?.MasterSbt ?? string.Empty, isHistory ? new List<YohoSetMstModel>() : GetListYohoSetMstModelByUserID(listYohoSets ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrInfDetail.SinKouiKbn).ToList() ?? new List<TenMst>()), kasan?.Kasan1 ?? 0, kasan?.Kasan2 ?? 0);
                        odrDetailModels.Add(odrInfDetailModel);
                        count++;
                    }
                }
                rpOdrInf.OrdInfDetails.AddRange(odrDetailModels);
                result.Add(rpOdrInf);
            }

            return result;
        }


        private OrdInfModel ConvertToModel(OdrInf ordInf, string createName = "")
        {
            return new OrdInfModel(ordInf.HpId,
                        ordInf.RaiinNo,
                        ordInf.RpNo,
                        ordInf.RpEdaNo,
                        ordInf.PtId,
                        ordInf.SinDate,
                        ordInf.HokenPid,
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
                        ordInf.Id,
                        new List<OrdInfDetailModel>(),
                        ordInf.CreateDate,
                        ordInf.CreateId,
                        createName
                   );
        }

        private OrdInfDetailModel ConvertToDetailModel(OdrInfDetail ordInfDetail, double yakka, double ten, bool isGetPriceInYakka, int kensaGaichu, int bunkatuKoui, int inOutKbn, int alternationIndex, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2)
        {
            return new OrdInfDetailModel(
                            ordInfDetail.HpId,
                            ordInfDetail.RaiinNo,
                            ordInfDetail.RpNo,
                            ordInfDetail.RpEdaNo,
                            ordInfDetail.RowNo,
                            ordInfDetail.PtId,
                            ordInfDetail.SinDate,
                            ordInfDetail.SinKouiKbn,
                            ordInfDetail.ItemCd ?? string.Empty,
                            ordInfDetail.ItemName ?? string.Empty,
                            ordInfDetail.Suryo,
                            ordInfDetail.UnitName ?? string.Empty,
                            ordInfDetail.UnitSBT,
                            ordInfDetail.TermVal,
                            ordInfDetail.KohatuKbn,
                            ordInfDetail.SyohoKbn,
                            ordInfDetail.SyohoLimitKbn,
                            ordInfDetail.DrugKbn,
                            ordInfDetail.YohoKbn,
                            ordInfDetail.Kokuji1 ?? string.Empty,
                            ordInfDetail.Kokiji2 ?? string.Empty,
                            ordInfDetail.IsNodspRece,
                            ordInfDetail.IpnCd ?? string.Empty,
                            ordInfDetail.IpnName ?? string.Empty,
                            ordInfDetail.JissiKbn,
                            ordInfDetail.JissiDate ?? DateTime.MinValue,
                            ordInfDetail.JissiId,
                            ordInfDetail.JissiMachine ?? string.Empty,
                            ordInfDetail.ReqCd ?? string.Empty,
                            ordInfDetail.Bunkatu ?? string.Empty,
                            ordInfDetail.CmtName ?? string.Empty,
                            ordInfDetail.CmtOpt ?? string.Empty,
                            ordInfDetail.FontColor ?? string.Empty,
                            ordInfDetail.CommentNewline,
                            masterSbt,
                            inOutKbn,
                            yakka,
                            isGetPriceInYakka,
                            0,
                            0,
                            ten,
                            bunkatuKoui,
                            alternationIndex,
                            kensaGaichu,
                            odrTermVal,
                            cnvTermVal,
                            yjCd,
                            yohoSets,
                            kasan1,
                            kasan2
                );
        }

        private bool IsGetPriceInYakka(TenMst? tenMst, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems)
        {
            if (tenMst == null) return false;

            var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.IpnNameCd == tenMst.IpnNameCd);

            var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.ItemCd == tenMst.ItemCd);

            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        private int GetKensaGaichu(OdrInfDetail? odrInfDetail, TenMst? tenMst, int inOutKbn, int odrKouiKbn, KensaMst? kensaMst, int kensaIraiCondition, int kensaIrai)
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

        private List<YohoSetMstModel> GetListYohoSetMstModelByUserID(List<YohoSetMst> listYohoSetMst, List<TenMst> listTenMst)
        {
            var query = from yoho in listYohoSetMst
                        join ten in listTenMst on yoho.ItemCd.Trim() equals ten.ItemCd.Trim()
                        select new
                        {
                            Yoho = yoho,
                            ItemName = ten.Name,
                            YohoKbn = ten.YohoKbn
                        };
            return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
        }

        public OrdInfModel Read(int ordId)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfModel ord)
        {
            throw new NotImplementedException();
        }
    }
}
