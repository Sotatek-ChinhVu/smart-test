using Domain.Models.OrdInf;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Entity.Tenant;
using Helper.Constants;

namespace Infrastructure.Converter
{
    internal class Order
    {
        protected Order()
        {

        }

        public static OrdInfModel CreateBy(OdrInf odrInf, List<OdrInfDetail> odrInfDetailList, List<TenMst> tenMstList, List<KensaMst> kensaMstList, List<IpnNameMst> ipnNameMstList, string createName, string updateName, int odrKouiKbn, int kensaIrai, int kensaIraiCondition, IQueryable<IpnMinYakkaMst> ipnMinYakkaMstQuery, IQueryable<IpnKasanExclude> ipnKasanExcludeQuery, IQueryable<IpnKasanExcludeItem> ipnKasanExcludeItemQuery)
        {
            int inOutKbn = odrInf.InoutKbn;

            List<OrdInfDetailModel> odrInfDetailModelList = new List<OrdInfDetailModel>();

            foreach (OdrInfDetail detail in odrInfDetailList)
            {
                if (detail.ItemCd == null)
                {
                    continue;
                }
                string itemCd = detail.ItemCd;
                string ipnCd = detail.IpnCd ?? string.Empty;
                double ten = 0;
                string masterSbt = string.Empty;
                double odrTermVal = 0;
                double cnvTermVal = 0;
                string yjCd = string.Empty;
                IpnMinYakkaMstModel ipnMinYakkaMstModel = new IpnMinYakkaMstModel();
                bool isGetYakka = false;

                var tenMst = tenMstList
                     .Where(t => t.ItemCd == itemCd && t.StartDate <= detail.SinDate && detail.SinDate <= t.EndDate)
                     .OrderByDescending(t => t.StartDate)
                     .FirstOrDefault();

                var ipnMinYakka = ipnMinYakkaMstQuery.Where(ipnYakka => ipnYakka.IpnNameCd == detail.IpnCd && ipnYakka.StartDate <= detail.SinDate && ipnYakka.EndDate >= detail.SinDate).OrderByDescending(ipnYakka => ipnYakka.EndDate).FirstOrDefault();
                if (ipnMinYakka != null)
                {
                    ipnMinYakkaMstModel = new IpnMinYakkaMstModel(ipnMinYakka.Id, ipnMinYakka.HpId, ipnMinYakka.IpnNameCd,
                        ipnMinYakka.StartDate, ipnMinYakka.EndDate, ipnMinYakka.Yakka, ipnMinYakka.SeqNo, ipnMinYakka.IsDeleted, false);
                }

                isGetYakka = ipnKasanExcludeQuery.FirstOrDefault(ipnKasan => ipnKasan.IpnNameCd == detail.IpnCd && ipnKasan.StartDate <= detail.SinDate && ipnKasan.EndDate >= detail.SinDate) == null && ipnKasanExcludeItemQuery.FirstOrDefault(ipnKasan => ipnKasan.ItemCd == detail.ItemCd && ipnKasan.StartDate <= detail.SinDate && ipnKasan.EndDate >= detail.SinDate) == null;

                if (tenMst != null)
                {
                    ten = tenMst.Ten;
                    masterSbt = tenMst.MasterSbt ?? string.Empty;
                    odrTermVal = tenMst.OdrTermVal;
                    cnvTermVal = tenMst.CnvTermVal;
                    yjCd = tenMst.YjCd ?? string.Empty;
                }

                if (tenMst != null && string.IsNullOrEmpty(ipnCd))
                {
                    detail.IpnCd = tenMst.IpnNameCd;
                }

                var ipnNameMst = ipnNameMstList.FirstOrDefault(ipn => ipn.IpnNameCd == detail.IpnCd);
                if (tenMst != null && string.IsNullOrEmpty(detail.IpnCd) && ipnNameMst != null)
                {
                    detail.IpnName = ipnNameMst.IpnName;
                }
                var kensaMst = tenMst == null ? null : kensaMstList.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);

                var kensaGaichu = GetKensaGaichu(detail, tenMst, inOutKbn, odrKouiKbn, kensaMst, kensaIraiCondition, kensaIrai);
                OrdInfDetailModel odrInfDetailModel =
                    new OrdInfDetailModel(detail.HpId,
                                          detail.RaiinNo,
                                          detail.RpNo,
                                          detail.RpEdaNo,
                                          detail.RowNo,
                                          detail.PtId,
                                          detail.SinDate,
                                          detail.SinKouiKbn,
                                          detail.ItemCd,
                                          detail.ItemName ?? string.Empty,
                                          detail.Suryo,
                                          detail.UnitName ?? string.Empty,
                                          detail.UnitSBT,
                                          detail.TermVal,
                                          detail.KohatuKbn,
                                          detail.SyohoKbn,
                                          detail.SyohoLimitKbn,
                                          detail.DrugKbn,
                                          detail.YohoKbn,
                                          detail.Kokuji1 ?? string.Empty,
                                          detail.Kokiji2 ?? string.Empty,
                                          detail.IsNodspRece,
                                          detail.IpnCd ?? string.Empty,
                                          detail.IpnName ?? string.Empty,
                                          detail.JissiKbn,
                                          detail.JissiDate ?? DateTime.MinValue,
                                          detail.JissiId,
                                          detail.JissiMachine ?? string.Empty,
                                          detail.ReqCd ?? string.Empty,
                                          detail.Bunkatu ?? string.Empty,
                                          detail.CmtName ?? string.Empty,
                                          detail.CmtOpt ?? string.Empty,
                                          detail.FontColor ?? string.Empty,
                                          detail.CommentNewline,
                                          masterSbt ?? string.Empty,
                                          inOutKbn,
                                          0,
                                          false,
                                          0,
                                          tenMst?.CmtCol1 ?? 0,
                                          ten,
                                          0,
                                          0,
                                          kensaGaichu,
                                          odrTermVal,
                                          cnvTermVal,
                                          yjCd,
                                          new List<YohoSetMstModel>(),
                                          0,
                                          0,
                                          string.Empty,
                                          string.Empty,
                                          string.Empty,
                                          string.Empty,
                                          tenMst?.CmtColKeta1 ?? 0,
                                          tenMst?.CmtColKeta2 ?? 0,
                                          tenMst?.CmtColKeta3 ?? 0,
                                          tenMst?.CmtColKeta4 ?? 0,
                                          tenMst?.CmtCol2 ?? 0,
                                          tenMst?.CmtCol3 ?? 0,
                                          tenMst?.CmtCol4 ?? 0,
                                          tenMst?.HandanGrpKbn ?? 0,
                                          kensaMst == null,
                                          ipnMinYakkaMstModel,
                                          isGetYakka,
                                          new(),
                                          odrInf.OdrKouiKbn
                                          );
                odrInfDetailModelList.Add(odrInfDetailModel);
            }

            OrdInfModel ordInfModel = new OrdInfModel(
                odrInf.HpId,
                odrInf.RaiinNo,
                odrInf.RpNo,
                odrInf.RpEdaNo,
                odrInf.PtId,
                odrInf.SinDate,
                odrInf.HokenPid,
                odrInf.OdrKouiKbn,
                odrInf.RpName ?? string.Empty,
                odrInf.InoutKbn,
                odrInf.SikyuKbn,
                odrInf.SyohoSbt,
                odrInf.SanteiKbn,
                odrInf.TosekiKbn,
                odrInf.DaysCnt,
                odrInf.SortNo,
                odrInf.IsDeleted,
                odrInf.Id,
                odrInfDetailModelList,
                odrInf.CreateDate,
                odrInf.CreateId,
                createName,
                odrInf.UpdateDate,
                odrInf.UpdateId,
                updateName,
                odrInf.CreateMachine ?? string.Empty,
                odrInf.UpdateMachine ?? string.Empty
                );

            return ordInfModel;
        }

        private static int GetKensaGaichu(OdrInfDetail? odrInfDetail, TenMst? tenMst, int inOutKbn, int odrKouiKbn, KensaMst? kensaMst, int kensaIraiCondition, int kensaIrai)
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
                    kensaCondition = (odrInfDetail.SinKouiKbn == 61 || odrInfDetail.SinKouiKbn == 64) && odrInfDetail.Kokiji2 != "7" && odrInfDetail.Kokiji2 != "9";
                }
                else
                {
                    kensaCondition = odrInfDetail.SinKouiKbn == 61 && odrInfDetail.Kokiji2 != "7" && odrInfDetail.Kokiji2 != "9" && (tenMst == null ? 0 : tenMst.HandanGrpKbn) != 6;
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
    }
}
