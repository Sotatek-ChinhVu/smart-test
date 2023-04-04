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

        public static OrdInfModel CreateBy(OdrInf odrInf, List<OdrInfDetail> odrInfDetailList, List<TenMst> tenMstList, List<KensaMst> kensaMstList, List<IpnNameMst> ipnNameMstList, string createName, string updateName, int odrKouiKbn, int kensaIrai, int kensaIraiCondition)
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

                var tenMst = tenMstList.FirstOrDefault(t => t.ItemCd == itemCd);
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
                                          0,
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
                                          string.Empty
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
                updateName
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
    }
}
