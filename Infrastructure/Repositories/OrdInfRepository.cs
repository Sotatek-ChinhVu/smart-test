using Domain.Models.InputItem;
using Domain.Models.OrdInf;
using Domain.Constant;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using Helper.Constants;

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

        public IEnumerable<OrdInfModel> GetList(int hpId, long ptId, long raiinNo, int sinDate, bool isDeleted)
        {
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && (isDeleted || odr.IsDeleted == 0))?.ToList();

            var result = ConvertEntityToListOrdInfModel(allOdrInf, allOdrInfDetails, hpId);
            return result;
        }

        public IEnumerable<OrdInfModel> GetList(long ptId, int hpId, long raiinNo)
        {
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.HpId == hpId && o.RaiinNo == raiinNo)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && odr.RaiinNo == raiinNo)?.ToList();

            return ConvertEntityToListOrdInfModel(allOdrInf, allOdrInfDetails, hpId);
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

        private List<OrdInfModel> ConvertEntityToListOrdInfModel(List<OdrInf>? allOdrInf, List<OdrInfDetail>? allOdrInfDetails, int hpId)
        {
            var result = new List<OrdInfModel>();
            var tenMsts = _tenantDataContext.TenMsts.Where(t => t.HpId == hpId);
            var kensaMsts = _tenantDataContext.KensaMsts.Where(t => t.HpId == hpId);
            var yakkas = _tenantDataContext.IpnMinYakkaMsts.Where(t => t.HpId == hpId);
            var ipnKasanExcludes = _tenantDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId);
            var ipnKasanExcludeItems = _tenantDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId);

            var checkKensaIrai = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
            var kensaIrai = checkKensaIrai?.Val ?? 0;
            var checkKensaIraiCondition = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
            var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;


            if (!(allOdrInf?.Count > 0))
            {
                return result;
            }

            foreach (var rpOdrInf in allOdrInf)
            {
                var odrDetailModels = new List<OrdInfDetailModel>();

                var createName = _tenantDataContext.UserMsts.FirstOrDefault(u => u.UserId == rpOdrInf.CreateId && u.HpId == hpId)?.Sname ?? string.Empty;
                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();

                if (odrInfDetails?.Count > 0)
                {
                    int count = 0;
                    var usage = odrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);

                    foreach (var odrInfDetail in odrInfDetails)
                    {
                        var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd && t.StartDate <= odrInfDetail.SinDate && t.EndDate >= odrInfDetail.SinDate);
                        var ten = tenMst?.Ten ?? 0;

                        var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);

                        var alternationIndex = count % 2;
                        var bunkatuKoui = 0;
                        if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                        {
                                bunkatuKoui = usage?.SinKouiKbn ?? 0;
                        }

                        var yakka = yakkas.FirstOrDefault(p => p.StartDate <= odrInfDetail.SinDate && p.EndDate >= odrInfDetail.SinDate && p.IpnNameCd == odrInfDetail.IpnCd)?.Yakka ?? 0;

                        var isGetPriceInYakka = IsGetPriceInYakka(tenMst, odrInfDetail.HpId, odrInfDetail.SinDate, ipnKasanExcludes, ipnKasanExcludeItems);

                        int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst, rpOdrInf.InoutKbn, rpOdrInf.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                        var odrInfDetailModel = ConvertToDetailModel(odrInfDetail, yakka, ten, isGetPriceInYakka, kensaGaichu, bunkatuKoui, rpOdrInf.InoutKbn, alternationIndex, tenMst?.OdrTermVal ?? 0, tenMst?.CnvTermVal ?? 0, tenMst?.YjCd ?? string.Empty, tenMst?.MasterSbt ?? string.Empty);
                        odrDetailModels.Add(odrInfDetailModel);
                        count++;
                    }
                }
                var ordInf = ConvertToModel(rpOdrInf, createName);
                ordInf.OrdInfDetails.AddRange(odrDetailModels);
                result.Add(ordInf);
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

        private OrdInfDetailModel ConvertToDetailModel(OdrInfDetail ordInfDetail, double yakka, double ten, bool isGetPriceInYakka, int kensaGaichu, int bunkatuKoui, int inOutKbn, int alternationIndex, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt)
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
                            yjCd
                );
        }

        private bool IsGetPriceInYakka(TenMst? tenMst, int hpId, int sinDate, IQueryable<IpnKasanExclude> ipnKasanExcludes, IQueryable<IpnKasanExcludeItem> ipnKasanExcludeItems)
        {
            if (tenMst == null) return false;

            var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.HpId == hpId && u.IpnNameCd == tenMst.IpnNameCd && u.StartDate <= sinDate && u.EndDate >= sinDate);

            var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.HpId == hpId && u.ItemCd == tenMst.ItemCd && u.StartDate <= sinDate && u.EndDate >= sinDate);

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
