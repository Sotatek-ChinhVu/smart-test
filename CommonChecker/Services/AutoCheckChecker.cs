using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Types;
using EmrCalculateApi.Implementation.Finder;
using System.Text;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class AutoCheckChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        public List<TOdrInf> CurrentListOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkingOdr"></param>
        /// <returns></returns>
        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            TOdrInf checkingOdr = unitCheckerResult.CheckingData;

            foreach (var detail in checkingOdr.OrdInfDetails)
            {
                if (string.IsNullOrEmpty(detail.ItemCd))
                {
                    continue;
                }
                var santeiGrpDetail = MasterFinder.FindSanteiGrpDetail(detail.ItemCd);
                if (santeiGrpDetail == null)
                {
                    continue;
                }
                var santeiCntCheck = MasterFinder.FindSanteiCntCheck(santeiGrpDetail.SanteiGroupCd, Sinday);
                if (santeiCntCheck == null)
                {
                    continue;
                }
                // Now, check TermCnt = 1 and TermSbt = 4 and CntType = 2 only. In other case, just ignore
                if (santeiCntCheck.TermCnt == 1 && santeiCntCheck.TermSbt == 4 && santeiCntCheck.CntType == 2)
                {
                    double santeiCntInMonth = MasterFinder.GetOdrCountInMonth(PtID, Sinday, detail.ItemCd);
                    double countInCurrentOdr = 0;

                    foreach (var item in CurrentListOrder)
                    {
                        foreach (var itemDetail in item.OrdInfDetails)
                        {
                            if (itemDetail.RpNo != detail.RpNo && itemDetail.ItemCd == detail.ItemCd)
                            {
                                countInCurrentOdr += itemDetail.Suryo;
                            }
                        }
                    }

                    double totalSanteiCount = santeiCntInMonth + countInCurrentOdr;

                    if (totalSanteiCount >= santeiCntCheck.MaxCnt)
                    {
                        var targetItem = MasterFinder.FindTenMst(santeiCntCheck.TargetCd, Sinday);
                        if (targetItem == null)
                        {
                            continue;
                        }

                        StringBuilder stringBuilder = new StringBuilder("");
                        stringBuilder.Append("'");
                        stringBuilder.Append(detail.DisplayItemName);
                        stringBuilder.Append("'");
                        stringBuilder.Append("が");
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append("1ヶ月 ");
                        stringBuilder.Append(santeiCntCheck.MaxCnt);
                        stringBuilder.Append("単位を超えています。");
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append("'");
                        stringBuilder.Append(targetItem.Name);
                        stringBuilder.Append("'に置き換えますか？");

                        string msg = stringBuilder.ToString();
                        EmrDialogMessage msgDlg = new EmrDialogMessage(EmrMessageType.mFree00010,
                              new string[] { msg },
                              new EmrMessageButtons[] { EmrMessageButtons.mbYes, EmrMessageButtons.mbNo }, 0);
                        var messageCallback = Messenger.Default.SendAsync(msgDlg);
                        if (messageCallback.Result.Success)
                        {
                            var dialogCallback = messageCallback.Result.Result;
                            if (dialogCallback.ResultButton == EmrMessageButtons.mbYes)
                            {
                                var grpKouiDetail = CIUtil.GetGroupKoui(detail.SinKouiKbn);
                                var grpKouiTarget = CIUtil.GetGroupKoui(targetItem.SinKouiKbn);

                                if (grpKouiDetail == grpKouiTarget)
                                {
                                    detail.ItemCd = targetItem.ItemCd;
                                    detail.ItemName = targetItem.Name;
                                    detail.SinKouiKbn = targetItem.SinKouiKbn;
                                    detail.KohatuKbn = targetItem.KohatuKbn;
                                    detail.DrugKbn = targetItem.DrugKbn;
                                    string unitNameBefore = detail.UnitName;
                                    if (!string.IsNullOrEmpty(targetItem.OdrUnitName))
                                    {
                                        detail.UnitSBT = 1;
                                        detail.UnitName = targetItem.OdrUnitName;
                                        detail.TermVal = targetItem.OdrTermVal;
                                    }
                                    else if (!string.IsNullOrEmpty(targetItem.CnvUnitName))
                                    {
                                        detail.UnitSBT = 2;
                                        detail.UnitName = targetItem.CnvUnitName;
                                        detail.TermVal = targetItem.CnvTermVal;
                                    }
                                    else
                                    {
                                        detail.UnitSBT = 0;
                                    }
                                    if (detail.UnitName != unitNameBefore)
                                    {
                                        detail.Suryo = 1;
                                    }
                                    detail.KohatuKbn = targetItem.KohatuKbn;
                                    detail.YohoKbn = targetItem.YohoKbn;
                                    detail.DrugKbn = targetItem.DrugKbn;
                                    detail.IpnCd = targetItem.IpnNameCd;
                                    detail.IpnName = MasterFinder.FindIpnNameMst(targetItem.IpnNameCd, Sinday)?.IpnName;

                                    detail.Kokuji1 = targetItem.Kokuji1;
                                    detail.Kokuji2 = targetItem.Kokuji2;

                                    if (detail.SinKouiKbn == 20 && detail.DrugKbn > 0)
                                    {
                                        switch (detail.KohatuKbn)
                                        {
                                            case 0:
                                                // 先発品
                                                detail.SyohoKbn = 0;
                                                detail.SyohoLimitKbn = 0;
                                                break;
                                            case 1:
                                                // 後発品
                                                detail.SyohoKbn = SystemConfig.Instance.AutoSetSyohoKbnKohatuDrug + 1;
                                                detail.SyohoLimitKbn = SystemConfig.Instance.AutoSetSyohoLimitKohatuDrug;
                                                break;
                                            case 2:
                                                // 後発品のある先発品
                                                detail.SyohoKbn = SystemConfig.Instance.AutoSetSyohoKbnSenpatuDrug + 1;
                                                detail.SyohoLimitKbn = SystemConfig.Instance.AutoSetSyohoLimitSenpatuDrug;
                                                break;
                                        }
                                        if (detail.SyohoKbn == 3 && string.IsNullOrEmpty(detail.IpnName))
                                        {
                                            // 一般名マスタに登録がない
                                            detail.SyohoKbn = 2;
                                        }
                                    }
                                }
                                else
                                {
                                    // Difference group koui
                                    if (checkingOdr.OdrInfDetailModels.Count == 1)
                                    {
                                        checkingOdr.OdrKouiKbn = detail.SinKouiKbn;
                                    }
                                    else
                                    {
                                        unitCheckerResult.AdditionData.Add(targetItem.ItemCd);
                                    }
                                }
                            }
                        }
                    }
                    else if (totalSanteiCount + detail.Suryo > santeiCntCheck.MaxCnt)
                    {
                        StringBuilder stringBuilder = new StringBuilder("");
                        stringBuilder.Append("'");
                        stringBuilder.Append(detail.DisplayItemName);
                        stringBuilder.Append("'");
                        stringBuilder.Append("が");
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append("1ヶ月 ");
                        stringBuilder.Append(santeiCntCheck.MaxCnt);
                        stringBuilder.Append("単位を超えます。");
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append("数量を'");
                        stringBuilder.Append(santeiCntCheck.MaxCnt - totalSanteiCount);
                        stringBuilder.Append("'に変更しますか？");

                        string msg = stringBuilder.ToString();
                        EmrDialogMessage msgDlg = new EmrDialogMessage(EmrMessageType.mFree00010,
                              new string[] { msg },
                              new EmrMessageButtons[] { EmrMessageButtons.mbYes, EmrMessageButtons.mbNo }, 0);
                        var messageCallback = Messenger.Default.SendAsync(msgDlg);
                        if (messageCallback.Result.Success)
                        {
                            var dialogCallback = messageCallback.Result.Result;
                            if (dialogCallback.ResultButton == EmrMessageButtons.mbYes)
                            {
                                detail.Suryo = santeiCntCheck.MaxCnt - totalSanteiCount;
                            }
                        }
                    }

                }
            }
            return unitCheckerResult;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            throw new NotImplementedException();
        }
    }
}
