using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.TodayOdr;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using UseCase.OrdInfs.CheckedSpecialItem;

namespace Interactor.MedicalExamination
{
    public class CheckedSpecialItemInteractor : ICheckedSpecialItemInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        private List<string> _syosinls =
            new List<string>
            {
                    ItemCdConst.Syosin,
                    ItemCdConst.SyosinCorona,
                    ItemCdConst.SyosinJouhou,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi
            };

        public CheckedSpecialItemInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;

        }

        public CheckedSpecialItemOutputData Handle(CheckedSpecialItemInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidSinDate);
                }
                if (inputData.IBirthDay <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidIBirthDay);
                }
                if (inputData.CheckAge < 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidCheckAge);
                }
                if (inputData.CheckAge < 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidCheckAge);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidRaiinNo);
                }
                if (inputData.OdrInfDetails.Count == 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.InvalidOdrInfDetail);
                }


            }
            catch
            {
                return new CheckedSpecialItemOutputData(new List<CheckSpecialItemModel>(), CheckedSpecialItemStatus.Failed);
            }
        }

        /// <summary>
        /// 年齢チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> AgeLimitCheck(int sinDate, int iBirthDay, int checkAge, TenItemModel tenMstItem, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();

            int iYear = 0;
            int iMonth = 0;
            int iDay = 0;
            bool needCheckMaxAge, needCheckMinAge;
            CIUtil.SDateToDecodeAge(iBirthDay, sinDate, ref iYear, ref iMonth, ref iDay);

            // Total day from birthday to sindate
            int iDays = 0;
            if (iBirthDay < sinDate)
            {
                iDays = CIUtil.DaysBetween(CIUtil.StrToDate(CIUtil.SDateToShowSDate(iBirthDay)), CIUtil.StrToDate(CIUtil.SDateToShowSDate(sinDate)));
            }

            if (checkAge == 0)
            {
                return checkSpecialItemList;
            }

            List<string> checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                if (checkedItem.Contains(detail.ItemCd))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(detail.ItemCd))
                {
                    continue;
                }
                if (tenMstItem == null)
                {
                    continue;
                }

                needCheckMaxAge = !string.IsNullOrEmpty(tenMstItem.MaxAge) && tenMstItem.MaxAge != "00" && tenMstItem.MaxAge != "0";
                needCheckMinAge = !string.IsNullOrEmpty(tenMstItem.MinAge) && tenMstItem.MinAge != "00" && tenMstItem.MinAge != "0";
                string msg = string.Empty;
                if (needCheckMinAge && needCheckMaxAge && CheckAge(tenMstItem.MaxAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge)}、{FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge)}）\"は上限年齢以上のため、算定できません。";
                }
                else if (needCheckMinAge && needCheckMaxAge && !CheckAge(tenMstItem.MinAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge)}、{FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge)}）\"は下限年齢未満のため、算定できません。";
                }
                if (needCheckMaxAge && CheckAge(tenMstItem.MaxAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge)}）\"は上限年齢以上のため、算定できません。";
                }
                else if (needCheckMinAge && !CheckAge(tenMstItem.MinAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge)}）\"は下限年齢未満のため、算定できません。";
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.AgeLimit, string.Empty, msg, detail.ItemCd);


                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        private bool CheckAge(string tenMstAgeCheck, int iDays, int sinDate, int iBirthDay, int iYear)
        {
            bool subResult = false;

            if (tenMstAgeCheck == "AA")
            {
                // 生後２８日
                subResult = (iDays >= 28);
            }
            else if (tenMstAgeCheck == "B3")
            {
                //３歳に達した日の翌月の１日
                subResult = CheckInBirthMonth(iYear, iBirthDay, sinDate, 3);
            }
            else if (tenMstAgeCheck == "B6")
            {
                //６歳に達した日の翌月の１日
                subResult = CheckInBirthMonth(iYear, iBirthDay, sinDate, 6);
            }
            else if (tenMstAgeCheck == "BF")
            {
                //１５歳に達した日の翌月の１日（現状入院項目のみ）
                subResult = CheckInBirthMonth(iYear, iBirthDay, sinDate, 15);
            }
            else if (tenMstAgeCheck == "BK")
            {
                //２０歳に達した日の翌月の１日（現状入院項目のみ）
                subResult = CheckInBirthMonth(iYear, iBirthDay, sinDate, 20);
            }
            else if (tenMstAgeCheck == "AE")
            {
                //生後９０日
                subResult = (iDays >= 90);
            }
            else if (tenMstAgeCheck == "MG")
            {
                //未就学
                subResult = CIUtil.IsStudent(iBirthDay, sinDate);
            }
            else
            {
                subResult = iYear >= CIUtil.StrToIntDef(tenMstAgeCheck, 0);
            }
            return subResult;
        }

        private bool CheckInBirthMonth(int iYear, int iBirthDay, int sinDate, int tenMstAgeCheck)
        {
            return (iYear > tenMstAgeCheck) ||
                   ((iYear == tenMstAgeCheck) && ((iBirthDay % 10000 / 100) < (sinDate % 10000 / 100)));
        }

        private string FormatDisplayMessage(string tenMstAgeCheck, CheckAgeType checkAgeType)
        {
            string formatedCheckKbn = string.Empty;

            if (tenMstAgeCheck == "AA")
            {
                // 生後２８日
                formatedCheckKbn = "生後２８日";
            }
            else if (tenMstAgeCheck == "B3")
            {
                //３歳に達した日の翌月の１日
                formatedCheckKbn = "３歳に達した日の翌月の１日";
            }
            else if (tenMstAgeCheck == "B6")
            {
                //６歳に達した日の翌月の１日
                formatedCheckKbn = "６歳に達した日の翌月の１日";
            }
            else if (tenMstAgeCheck == "BF")
            {
                //１５歳に達した日の翌月の１日（現状入院項目のみ）
                formatedCheckKbn = "１５歳に達した日の翌月の１日";
            }
            else if (tenMstAgeCheck == "BK")
            {
                //２０歳に達した日の翌月の１日（現状入院項目のみ）
                formatedCheckKbn = "２０歳に達した日の翌月の１日";
            }
            else if (tenMstAgeCheck == "AE")
            {
                //生後９０日
                formatedCheckKbn = "生後９０日";
            }
            else if (tenMstAgeCheck == "MG")
            {
                //未就学
                formatedCheckKbn = "未就学";
            }
            else
            {
                formatedCheckKbn = CIUtil.StrToIntDef(tenMstAgeCheck, 0) + "歳";
                if (checkAgeType == CheckAgeType.MaxAge)
                {
                    formatedCheckKbn += "未満";
                }
                else if (checkAgeType == CheckAgeType.MinAge)
                {
                    formatedCheckKbn += "以上";
                }
            }
            return formatedCheckKbn;
        }

        private string FormatDisplayMessage(string itemName, int dateCheck, bool isCheckStartDate)
        {
            string dateString = CIUtil.SDateToShowSDate(dateCheck);

            if (isCheckStartDate)
            {
                return $"\"{itemName}\"は{dateString}から使用可能です。";
            }
            else
            {
                return $"\"{itemName}\"は{dateString}まで使用可能です。";
            }
        }
        /// <summary>
        /// 有効期限チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> ExpiredCheck(int sinDate, List<TenItemModel> tenMstItemList, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();

            List<string> checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                if (checkedItem.Contains(detail.ItemCd))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(detail.ItemCd))
                {
                    continue;
                }
                if (tenMstItemList.Count == 0)
                {
                    continue;
                }
                int minStartDate = tenMstItemList.Min(item => item.StartDate);

                if (minStartDate > sinDate)
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.Expiration, string.Empty, FormatDisplayMessage(detail.DisplayItemName, minStartDate, true), detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                int maxEndDate = tenMstItemList.Max(item => item.EndDate);

                if (maxEndDate < sinDate)
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.Expiration, string.Empty, FormatDisplayMessage(detail.DisplayItemName, maxEndDate, false), detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }



        /// <summary>
        /// 重複チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> DuplicateCheck(int sinDate, List<TenItemModel> tenMstItems, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();
            List<string> checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                // ｺﾒﾝﾄや用法,特材、分割処方は対象外
                if (string.IsNullOrEmpty(detail.ItemCd) // ｺﾒﾝﾄ
                    || detail.ItemCd.StartsWith("Y") // 用法
                    || detail.ItemCd.StartsWith("Z") // 特材
                    || detail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu // 分割処方
                    || detail.ItemCd == ItemCdConst.Con_Refill) //リフィル
                {
                    continue;
                }

                if (checkedItem.Contains(detail.ItemCd))
                {
                    continue;
                }

                var tenMstItem = tenMstItems.FirstOrDefault(t => t.ItemCd == detail.ItemCd);
                if (tenMstItem == null)
                {
                    continue;
                }

                if (tenMstItem.MasterSbt == "C" || tenMstItem.BuiKbn > 0)
                {
                    continue;
                }

                var itemCount = allOdrInfDetail.Where(d => d.ItemCd == detail.ItemCd).Count();

                if (itemCount > 1)
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.Duplicate, string.Empty, $"\"{detail.DisplayItemName}\"", detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        /// <summary>
        /// 項目コメント
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> ItemCommentCheck(Dictionary<string, string> items, List<ItemCmtModel> allCmtCheckMst, List<KarteInfModel> karteInfs)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();
            foreach (var item in items)
            {
                if (checkSpecialItemList.Any(p => p.ItemCd == item.Key)) continue;

                if (IsShowCommentCheckMst(item.Key, allCmtCheckMst, karteInfs))
                {
                    checkSpecialItemList.Add(new CheckSpecialItemModel(CheckSpecialType.ItemComment, string.Empty, $"\"{item.Value}\"に対するコメントがありません。", item.Key));
                }
            }
            return checkSpecialItemList;
        }

        private bool IsShowCommentCheckMst(string itemCd, List<ItemCmtModel> allCmtCheckMst, List<KarteInfModel> karteInfs)
        {
            var itemCmtModels = allCmtCheckMst.FindAll(p => p.ItemCd == itemCd).OrderBy(p => p.SortNo).ToList();
            if (itemCmtModels.Count == 0) return false;

            foreach (var itemCmtModel in itemCmtModels)
            {
                if (karteInfs.Exists(p => p.KarteKbn == itemCmtModel.KarteKbn && p.Text.AsString().Contains(itemCmtModel.Comment)))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 算定回数チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> CalculationCountCheck(int hpId, int sinDate, long raiinNo, long ptId, int hokenKbn, int hokensyuHandling, int syosinDate, TenItemModel santeiTenMst, List<DensiSanteiKaisuModel> densiSanteiKaisuModels, TenItemModel tenMst, List<OrdInfDetailModel> allOdrInfDetail, List<ItemGrpMstModel> itemGrpMsts, List<(long rpno, long edano, int hokenId)> hokenIds)
        {
            #region Sub function
            int WeeksBefore(int baseDate, int term)
            {
                return CIUtil.WeeksBefore(baseDate, term);
            }

            int MonthsBefore(int baseDate, int term)
            {
                return CIUtil.MonthsBefore(baseDate, term);
            }

            int YearsBefore(int baseDate, int term)
            {
                return CIUtil.YearsBefore(baseDate, term);
            }

            int DaysBefore(int baseDate, int term)
            {
                return CIUtil.DaysBefore(baseDate, term);
            }

            int MonthsAfter(int baseDate, int term)
            {
                return CIUtil.MonthsAfter(baseDate, term);
            }
            int GetPtHokenKbn(long rpno, long edano)
            {
                int? ret = 0;
                if (hokenIds.Any(p => p.rpno == rpno && p.edano == edano))
                {
                    ret = hokenKbn;
                }

                return ret == null ? 0 : ret.Value;
            }

            int GetHokenKbn(int odrHokenKbn)
            {
                int hokenKbn = 0;

                if (new int[] { 0 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 4;
                }
                else if (new int[] { 1, 2 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 0;
                }
                else if (new int[] { 11, 12 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 1;
                }
                else if (new int[] { 13 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 2;
                }
                else if (new int[] { 14 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 3;
                }

                return hokenKbn;
            }
            /// <summary>
            /// チェック用保険区分を返す
            /// 健保、労災、自賠の場合、オプションにより、同一扱いにするか別扱いにするか決定
            /// 自費の場合、健保と自費を対象にする
            /// </summary>
            /// <param name="hokenKbn">
            /// 0-健保、1-労災、2-アフターケア、3-自賠、4-自費
            /// </param>
            /// <returns></returns>
            List<int> GetCheckHokenKbns(int odrHokenKbn)
            {
                List<int> results = new List<int>();

                int hokenKbn = GetHokenKbn(odrHokenKbn);

                if (hokensyuHandling == 0)
                {
                    // 同一に考える
                    if (hokenKbn <= 3)
                    {
                        results.AddRange(new List<int> { 0, 1, 2, 3 });
                    }
                    else
                    {
                        results.Add(hokenKbn);
                    }
                }
                else if (hokensyuHandling == 1)
                {
                    // すべて同一に考える
                    results.AddRange(new List<int> { 0, 1, 2, 3, 4 });
                }
                else
                {
                    // 別に考える
                    results.Add(hokenKbn);
                }

                if (hokenKbn == 4)
                {
                    results.Add(0);
                }

                return results;
            }

            List<int> GetCheckSanteiKbns(int odrHokenKbn)
            {
                List<int> results = new List<int> { 0 };

                int hokenKbn = GetHokenKbn(odrHokenKbn);

                if (hokensyuHandling == 0)
                {
                    // 同一に考える
                    if (hokenKbn == 4)
                    {
                        //results.Add(2);
                    }
                }
                else if (hokensyuHandling == 1)
                {
                    // すべて同一に考える
                    results.Add(2);
                }
                else
                {
                    // 別に考える
                }

                return results;
            }
            #endregion

            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();
            int endDate = sinDate;
            // MAX_COUNT>1の場合は注意扱いする単位のコード

            List<string> checkedItem = new List<string>();
            foreach (var odrDetail in allOdrInfDetail)
            {
                if (string.IsNullOrEmpty(odrDetail.ItemCd))
                {
                    continue;
                }
                if (checkedItem.Contains(odrDetail.ItemCd))
                {
                    continue;
                }

                string santeiItemCd = odrDetail.ItemCd;
                string itemName = odrDetail.DisplayItemName;
                if (tenMst != null)
                {
                    if (!string.IsNullOrEmpty(tenMst.ItemCd)
                        && !string.IsNullOrEmpty(tenMst.SanteiItemCd)
                        && tenMst.ItemCd != tenMst.SanteiItemCd
                        && tenMst.SanteiItemCd != ItemCdConst.NoSantei
                        && !tenMst.ItemCd.StartsWith("Z"))
                    {
                        santeiItemCd = tenMst.SanteiItemCd;
                        if (santeiTenMst != null)
                        {
                            itemName = santeiTenMst.Name;
                        }
                    }
                }

                double suryo = 0;
                var allSameDetail = allOdrInfDetail.Where(d => d.ItemCd == odrDetail.ItemCd);
                foreach (var item in allSameDetail)
                {
                    suryo += (item.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(odrDetail.ItemCd)) ? 1 : item.Suryo;
                }

                //if (odrDetail.ItemCd != santeiItemCd)
                //{
                //    foreach (var odrInfDetail in allOdrInfDetail)
                //    {
                //        if (odrInfDetail.ItemCd == santeiItemCd)
                //        {
                //            suryo += (odrInfDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(odrInfDetail.ItemCd)) ? 1 : odrInfDetail.Suryo;
                //        }
                //    }
                //}

                densiSanteiKaisuModels = densiSanteiKaisuModels.Where(d => d.ItemCd == santeiItemCd).ToList();
                // チェック期間と表記を取得する
                foreach (var densiSanteiKaisu in densiSanteiKaisuModels)
                {
                    string sTerm = string.Empty;
                    int startDate = 0;

                    List<int> checkHokenKbnTmp = new List<int>();
                    checkHokenKbnTmp.AddRange(GetCheckHokenKbns(GetPtHokenKbn(odrDetail.RpNo, odrDetail.RpEdaNo)));

                    if (densiSanteiKaisu.TargetKbn == 1)
                    {
                        // 健保のみ対象の場合はすべて対象
                    }
                    else if (densiSanteiKaisu.TargetKbn == 2)
                    {
                        // 労災のみ対象の場合、健保は抜く
                        checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                    }

                    List<int> checkSanteiKbnTmp = new List<int>();
                    checkSanteiKbnTmp.AddRange(GetCheckSanteiKbns(GetPtHokenKbn(odrDetail.RpNo, odrDetail.RpEdaNo)));

                    switch (densiSanteiKaisu.UnitCd)
                    {
                        case 53:    //患者あたり
                            sTerm = "患者あたり";
                            break;
                        case 121:   //1日
                            startDate = sinDate;
                            sTerm = "日";
                            break;
                        case 131:   //1月
                            startDate = sinDate / 100 * 100 + 1;
                            sTerm = "月";
                            break;
                        case 138:   //1週
                            startDate = WeeksBefore(sinDate, 1);
                            sTerm = "週";
                            break;
                        case 141:   //一連
                            startDate = -1;
                            sTerm = "一連";
                            break;
                        case 142:   //2週
                            startDate = WeeksBefore(sinDate, 2);
                            sTerm = "2週";
                            break;
                        case 143:   //2月
                            startDate = MonthsBefore(sinDate, 1);
                            sTerm = "2月";
                            break;
                        case 144:   //3月
                            startDate = MonthsBefore(sinDate, 2);
                            sTerm = "3月";
                            break;
                        case 145:   //4月
                            startDate = MonthsBefore(sinDate, 3);
                            sTerm = "4月";
                            break;
                        case 146:   //6月
                            startDate = MonthsBefore(sinDate, 5);
                            sTerm = "6月";
                            break;
                        case 147:   //12月
                            startDate = MonthsBefore(sinDate, 11);
                            sTerm = "12月";
                            break;
                        case 148:   //5年
                            startDate = YearsBefore(sinDate, 5);
                            sTerm = "5年";
                            break;
                        case 997:   //初診から1カ月（休日除く）
                            if (allOdrInfDetail.Where(d => d != odrDetail).Count(p => _syosinls.Contains(p.ItemCd)) > 0)
                            {
                                // 初診関連項目を算定している場合、算定不可
                                endDate = 99999999;
                            }
                            else
                            {
                                // 直近の初診日から１か月後を取得する（休日除く）
                                endDate = syosinDate;
                                endDate = _todayOdrRepository.MonthsAfterExcludeHoliday(hpId, endDate, 1);
                            }
                            break;
                        case 998:   //初診から1カ月
                            if (allOdrInfDetail.Where(d => d != odrDetail).Count(p => _syosinls.Contains(p.ItemCd)) > 0)
                            {
                                // 初診関連項目を算定している場合、算定不可
                                endDate = 99999999;
                            }
                            else
                            {
                                // 直近の初診日から１か月後を取得する（休日除く）
                                endDate = syosinDate;
                                endDate = MonthsAfter(endDate, 1);
                            }
                            break;
                        case 999:   //カスタム
                            if (densiSanteiKaisu.TermSbt == 2)
                            {
                                //日
                                startDate = DaysBefore(sinDate, densiSanteiKaisu.TermCount);
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "日";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "日";
                                }
                            }
                            else if (densiSanteiKaisu.TermSbt == 3)
                            {
                                //週
                                startDate = WeeksBefore(sinDate, densiSanteiKaisu.TermCount);
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "週";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "週";
                                }
                            }
                            else if (densiSanteiKaisu.TermSbt == 4)
                            {
                                //月
                                startDate = MonthsBefore(sinDate, densiSanteiKaisu.TermCount);
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "月";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "月";
                                }
                            }
                            else if (densiSanteiKaisu.TermSbt == 5)
                            {
                                //年
                                startDate = (sinDate / 10000 - (densiSanteiKaisu.TermCount - 1)) * 10000 + 101;
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "年間";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "年間";
                                }
                            }
                            break;
                        default:
                            startDate = -1;
                            break;
                    }

                    if (densiSanteiKaisu.UnitCd == 997 || densiSanteiKaisu.UnitCd == 998)
                    {
                        //初診から1カ月
                        if (endDate > sinDate)
                        {
                            string conditionMsg = string.Empty;
                            //算定不可
                            if (densiSanteiKaisu.SpJyoken == 1)
                            {
                                conditionMsg = "算定できない可能性があります。";
                            }
                            else
                            {
                                conditionMsg = "算定できません。";
                            }

                            string errMsg = string.Format("'{0}' は、初診から1カ月以内のため、" + conditionMsg, odrDetail.ItemName);
                            CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.CalculationCount, string.Empty, errMsg, odrDetail.ItemCd);

                            checkSpecialItemList.Add(checkSpecialItem);
                        }
                    }
                    else
                    {
                        double count = 0;
                        if (startDate >= 0)
                        {
                            List<string> itemCds = new List<string>();

                            if (densiSanteiKaisu.ItemGrpCd > 0)
                            {
                                // 項目グループの設定がある場合
                                itemGrpMsts = itemGrpMsts?.Where(i => i.ItemGrpCd == densiSanteiKaisu.ItemGrpCd).ToList() ?? new List<ItemGrpMstModel>();
                            }

                            if (itemGrpMsts != null && itemGrpMsts.Any())
                            {
                                // 項目グループの設定がある場合
                                itemCds.AddRange(itemGrpMsts.Select(x => x.ItemCd));
                            }
                            else
                            {
                                itemCds.Add(odrDetail.ItemCd);
                            }

                            count = _todayOdrRepository.SanteiCount(hpId, ptId, startDate, endDate, sinDate, raiinNo, itemCds, checkSanteiKbnTmp, checkHokenKbnTmp);
                        }
                        if (densiSanteiKaisu.MaxCount <= count // 上限値を超えるかチェックする
                        || densiSanteiKaisu.MaxCount < count + suryo) // 今回分を足すと超えてしまう場合は注意（MaxCount = count + konkaiSuryoはセーフ）
                        {
                            string errMsg = $"\"{itemName}\" {sTerm}{count + suryo}回算定({densiSanteiKaisu.MaxCount}回まで)";
                            CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.CalculationCount, string.Empty, errMsg, odrDetail.ItemCd);

                            checkSpecialItemList.Add(checkSpecialItem);
                        }
                    }
                }
                checkedItem.Add(odrDetail.ItemCd);
            }

            return checkSpecialItemList;
        }
    }
}
