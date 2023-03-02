using Domain.Models.Insurance;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using UseCase.MedicalExamination.UpsertTodayOrd;
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
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IReceptionRepository _receptionRepository;

        public CheckedSpecialItemInteractor(ITodayOdrRepository todayOdrRepository, IMstItemRepository mstItemRepository, IInsuranceRepository insuranceRepository, ISystemConfRepository systemConfRepository, IReceptionRepository receptionRepository)
        {
            _todayOdrRepository = todayOdrRepository;
            _mstItemRepository = mstItemRepository;
            _insuranceRepository = insuranceRepository;
            _systemConfRepository = systemConfRepository;
            _receptionRepository = receptionRepository;
        }

        public CheckedSpecialItemOutputData Handle(CheckedSpecialItemInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidSinDate);
                }
                if (inputData.IBirthDay <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidIBirthDay);
                }
                if (inputData.CheckAge < 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidCheckAge);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidRaiinNo);
                }
                if (inputData.OdrInfs.Count == 0 && (inputData.KarteInf.HpId == 0 && inputData.KarteInf.PtId == 0 && inputData.KarteInf.RaiinNo == 0 && inputData.KarteInf.SinDate == 0))
                {
                    return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.InvalidOdrInfDetail);
                }

                #region Param data
                var allOdrInfDetail = new List<OdrInfDetailItemInputData>();

                foreach (var item in inputData.OdrInfs)
                {
                    allOdrInfDetail.AddRange(item.OdrDetails);
                }
                var allOdrInfDetailModel = allOdrInfDetail.Select(o => new OrdInfDetailModel(
                                o.HpId,
                                o.RaiinNo,
                                o.RpNo,
                                o.RpEdaNo,
                                o.RowNo,
                                o.PtId,
                                o.SinDate,
                                o.SinKouiKbn,
                                o.ItemCd,
                                o.ItemName,
                                o.Suryo,
                                o.UnitName,
                                o.UnitSbt,
                                o.TermVal,
                                o.KohatuKbn,
                                o.SyohoKbn,
                                o.SyohoLimitKbn,
                                o.DrugKbn,
                                o.YohoKbn,
                                o.Kokuji1,
                                o.Kokuji2,
                                o.IsNodspRece,
                                o.IpnCd,
                                o.IpnName,
                                o.JissiKbn,
                                o.JissiDate,
                                o.JissiId,
                                o.JissiMachine,
                                o.ReqCd,
                                o.Bunkatu,
                                o.CmtName,
                                o.CmtOpt,
                                o.FontColor,
                                o.CommentNewline,
                                String.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                ""
                            )).ToList() ?? new List<OrdInfDetailModel>();

                var hokenPids = new List<(long rpno, long edano, int hokenPid)>();

                foreach (var odrInf in inputData.OdrInfs)
                {
                    hokenPids.Add((odrInf.RpNo, odrInf.RpEdaNo, odrInf.HokenPid));
                }
                #endregion
                var checkSpecialItemList = new List<CheckedSpecialItem>();
                var itemCdCs = allOdrInfDetailModel.Select(x => x.ItemCd).Distinct().ToList();
                var minSinDate = !(allOdrInfDetailModel.Count > 0) ? 0 : allOdrInfDetailModel.Min(o => o.SinDate);
                var maxSinDate = !(allOdrInfDetailModel.Count > 0) ? 0 : allOdrInfDetailModel.Max(o => o.SinDate);
                var tenMsts = _mstItemRepository.FindTenMst(inputData.HpId, itemCdCs, minSinDate, maxSinDate) ?? new();
                var santeiItemCds = tenMsts?.Select(t => t.SanteiItemCd).ToList() ?? new();
                var santeiTenMsts = _mstItemRepository.FindTenMst(inputData.HpId, santeiItemCds, minSinDate, maxSinDate) ?? new();
                var densiSanteiKaisuModels = _todayOdrRepository.FindDensiSanteiKaisuList(inputData.HpId, itemCdCs, minSinDate, maxSinDate) ?? new();
                var itemGrpMsts = _mstItemRepository.FindItemGrpMst(inputData.HpId, inputData.SinDate, 1, densiSanteiKaisuModels?.Select(d => d.ItemGrpCd).ToList() ?? new());

                //enable or disable Expired Check and SanteiCount Check
                if (inputData.EnabledInputCheck)
                {
                    checkSpecialItemList.AddRange(AgeLimitCheck(inputData.SinDate, inputData.IBirthDay, inputData.CheckAge, tenMsts ?? new(), allOdrInfDetailModel));
                    checkSpecialItemList.AddRange(ExpiredCheck(inputData.SinDate, tenMsts ?? new(), allOdrInfDetailModel));
                    checkSpecialItemList.AddRange(CalculationCountCheck(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.PtId, santeiTenMsts, densiSanteiKaisuModels ?? new(), tenMsts ?? new(), allOdrInfDetailModel, itemGrpMsts, hokenPids));
                    checkSpecialItemList.AddRange(DuplicateCheck(tenMsts ?? new(), allOdrInfDetailModel));
                }

                //enable or disable Comment Check
                if (inputData.EnabledCommentCheck)
                {
                    var items = new Dictionary<string, string>();
                    var itemCds = new List<string>();
                    foreach (var odrInfDetail in allOdrInfDetail)
                    {
                        if (string.IsNullOrEmpty(odrInfDetail.ItemCd)) continue;

                        if (!items.ContainsKey(odrInfDetail.ItemCd))
                        {
                            itemCds.Add(odrInfDetail.ItemCd);
                            items.Add(odrInfDetail.ItemCd, odrInfDetail.ItemName);
                        }
                    }
                    foreach (var odrDetail in inputData.CheckedOrderModels)
                    {
                        if (string.IsNullOrEmpty(odrDetail.ItemCd)) continue;

                        if (!items.ContainsKey(odrDetail.ItemCd))
                        {
                            itemCds.Add(odrDetail.ItemCd);
                            if (odrDetail.Santei)
                            {
                                items.Add(odrDetail.ItemCd, odrDetail.ItemName);
                            }
                        }
                    }
                    var allCmtCheckMst = _mstItemRepository.GetCmtCheckMsts(inputData.HpId, inputData.UserId, itemCds);
                    checkSpecialItemList.AddRange(ItemCommentCheck(items, allCmtCheckMst, new KarteInfModel(
                                 inputData.KarteInf.HpId,
                                 inputData.KarteInf.RaiinNo,
                                 1,
                                 0,
                                 inputData.KarteInf.PtId,
                                 inputData.KarteInf.SinDate,
                                 inputData.KarteInf.Text,
                                 0,
                                 inputData.KarteInf.RichText,
                                 DateTime.MinValue,
                                 DateTime.MinValue,
                                 string.Empty
                        )));
                }

                return new CheckedSpecialItemOutputData(checkSpecialItemList, CheckedSpecialItemStatus.Successed);
            }
            catch
            {
                return new CheckedSpecialItemOutputData(new List<CheckedSpecialItem>(), CheckedSpecialItemStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
                _insuranceRepository.ReleaseResource();
                _receptionRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _todayOdrRepository.ReleaseResource();
            }
        }

        /// <summary>
        /// 年齢チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        public List<CheckedSpecialItem> AgeLimitCheck(int sinDate, int iBirthDay, int checkAge, List<TenItemModel> tenMstItems, List<OrdInfDetailModel> allOdrInfDetail)
        {
            var checkSpecialItemList = new List<CheckedSpecialItem>();
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

            var checkedItem = new List<string>();
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
                var tenMstItem = tenMstItems.FirstOrDefault(t => t.ItemCd == detail.ItemCd && t.StartDate <= detail.SinDate && t.EndDate >= detail.SinDate);
                if (tenMstItem == null)
                {
                    continue;
                }

                needCheckMaxAge = !string.IsNullOrEmpty(tenMstItem.MaxAge) && tenMstItem.MaxAge != "00" && tenMstItem.MaxAge != "0";
                needCheckMinAge = !string.IsNullOrEmpty(tenMstItem.MinAge) && tenMstItem.MinAge != "00" && tenMstItem.MinAge != "0";
                string msg = string.Empty;
                if (needCheckMinAge && needCheckMaxAge && CheckAge(tenMstItem.MaxAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = AgeLimitMessage(tenMstItem.Name, FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge), FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge));
                }
                else if (needCheckMinAge && needCheckMaxAge && !CheckAge(tenMstItem.MinAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = AgeLimitMessage(tenMstItem.Name, FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge), FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge));
                }
                if (needCheckMaxAge && CheckAge(tenMstItem.MaxAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = AgeLimitMessage(tenMstItem.Name, FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge));
                }
                else if (needCheckMinAge && !CheckAge(tenMstItem.MinAge, iDays, sinDate, iBirthDay, iYear))
                {
                    msg = AgeLimitMessage(tenMstItem.Name, FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge));
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    var checkSpecialItem = new CheckedSpecialItem(CheckSpecialType.AgeLimit, string.Empty, msg, detail.ItemCd);


                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        private string AgeLimitMessage(string name, string firstMessage, string secondMessage = "")
        {
            return !string.IsNullOrEmpty(secondMessage) ? $"\"{name} ({firstMessage}、{secondMessage}) \"は下限年齢未満のため、算定できません。" : $"\"{name} ({firstMessage}) \"は下限年齢未満のため、算定できません。";
        }

        public bool CheckAge(string tenMstAgeCheck, int iDays, int sinDate, int iBirthDay, int iYear)
        {
            bool subResult;

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
            return (iYear > tenMstAgeCheck) || ((iYear == tenMstAgeCheck) && ((iBirthDay % 10000 / 100) < (sinDate % 10000 / 100)));
        }

        private string FormatDisplayMessage(string tenMstAgeCheck, CheckAgeType checkAgeType)
        {
            var formatedCheckKbn = string.Empty;

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

        public List<CheckedSpecialItem> ExpiredCheck(int sinDate, List<TenItemModel> tenMstItemList, List<OrdInfDetailModel> allOdrInfDetail)
        {
            var checkSpecialItemList = new List<CheckedSpecialItem>();

            var checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                var tenMsts = tenMstItemList.Where(t => t.ItemCd == detail.ItemCd);
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
                int minStartDate = tenMsts.Min(item => item.StartDate);

                if (minStartDate > sinDate)
                {
                    var checkSpecialItem = new CheckedSpecialItem(CheckSpecialType.Expiration, string.Empty, FormatDisplayMessage(detail.DisplayItemName, minStartDate, true), detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                int maxEndDate = tenMsts.Max(item => item.EndDate);

                if (maxEndDate < sinDate)
                {
                    var checkSpecialItem = new CheckedSpecialItem(CheckSpecialType.Expiration, string.Empty, FormatDisplayMessage(detail.DisplayItemName, maxEndDate, false), detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        public List<CheckedSpecialItem> DuplicateCheck(List<TenItemModel> tenMstItems, List<OrdInfDetailModel> allOdrInfDetail)
        {
            var checkSpecialItemList = new List<CheckedSpecialItem>();
            var checkedItem = new List<string>();
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
                    var checkSpecialItem = new CheckedSpecialItem(CheckSpecialType.Duplicate, string.Empty, $"\"{detail.DisplayItemName}\"", detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        public List<CheckedSpecialItem> ItemCommentCheck(Dictionary<string, string> items, List<ItemCmtModel> allCmtCheckMst, KarteInfModel karteInf)
        {
            var checkSpecialItemList = new List<CheckedSpecialItem>();
            foreach (var item in items)
            {
                if (checkSpecialItemList.Any(p => p.ItemCd == item.Key)) continue;

                if (IsShowCommentCheckMst(item.Key, allCmtCheckMst, karteInf))
                {
                    checkSpecialItemList.Add(new CheckedSpecialItem(CheckSpecialType.ItemComment, string.Empty, $"\"{item.Value}\"に対するコメントがありません。", item.Key));
                }
            }
            return checkSpecialItemList;
        }

        private bool IsShowCommentCheckMst(string itemCd, List<ItemCmtModel> allCmtCheckMst, KarteInfModel karteInf)
        {
            var itemCmtModels = allCmtCheckMst?.FindAll(p => p.ItemCd == itemCd).OrderBy(p => p.SortNo)?.ToList();
            if (!(itemCmtModels?.Count > 0)) return false;

            foreach (var itemCmtModel in itemCmtModels)
            {
                if (karteInf.KarteKbn == KarteConst.KarteKbn && karteInf.Text.AsString().Contains(itemCmtModel.Comment))
                {
                    return false;
                }
            }
            return true;
        }

        public List<CheckedSpecialItem> CalculationCountCheck(int hpId, int sinDate, long raiinNo, long ptId, List<TenItemModel> santeiTenMsts, List<DensiSanteiKaisuModel> densiSanteiKaisuModels, List<TenItemModel> tenMsts, List<OrdInfDetailModel> allOdrInfDetail, List<ItemGrpMstModel> itemGrpMsts, List<(long rpno, long edano, int hokenId)> hokenIds)
        {
            var checkSpecialItemList = new List<CheckedSpecialItem>();
            int endDate = sinDate;
            // MAX_COUNT>1の場合は注意扱いする単位のコード
            var hokensyuHandling = (int)_systemConfRepository.GetSettingValue(3013, 0, hpId);
            var syosinDate = _receptionRepository.GetFirstVisitWithSyosin(hpId, ptId, sinDate);

            var checkedItem = new List<string>();
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
                var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrDetail.ItemCd);
                if (tenMst != null)
                {
                    if (!string.IsNullOrEmpty(tenMst.ItemCd)
                        && !string.IsNullOrEmpty(tenMst.SanteiItemCd)
                        && tenMst.ItemCd != tenMst.SanteiItemCd
                        && tenMst.SanteiItemCd != ItemCdConst.NoSantei
                        && !tenMst.ItemCd.StartsWith("Z"))
                    {
                        santeiItemCd = tenMst.SanteiItemCd;
                        var santeiTenMst = santeiTenMsts.FirstOrDefault(s => s.SanteiItemCd == santeiItemCd && s.StartDate <= odrDetail.SinDate && s.EndDate >= odrDetail.SinDate);
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

                var densiSanteiKaisuModelFilters = densiSanteiKaisuModels.Where(d => d.ItemCd == santeiItemCd).ToList();

                checkSpecialItemList.AddRange(ExecuteDensiSantei(ptId, hpId, endDate, raiinNo, hokensyuHandling, sinDate, syosinDate, suryo, itemName, densiSanteiKaisuModelFilters, odrDetail, hokenIds, allOdrInfDetail, itemGrpMsts));
                // チェック期間と表記を取得する
                checkedItem.Add(odrDetail.ItemCd);
            }

            return checkSpecialItemList;
        }

        public int WeeksBefore(int baseDate, int term)
        {
            return CIUtil.WeeksBefore(baseDate, term);
        }

        public int MonthsBefore(int baseDate, int term)
        {
            return CIUtil.MonthsBefore(baseDate, term);
        }

        public int YearsBefore(int baseDate, int term)
        {
            return CIUtil.YearsBefore(baseDate, term);
        }

        public int DaysBefore(int baseDate, int term)
        {
            return CIUtil.DaysBefore(baseDate, term);
        }

        public int MonthsAfter(int baseDate, int term)
        {
            return CIUtil.MonthsAfter(baseDate, term);
        }

        public int GetPtHokenKbn(int hpId, long ptId, int sinDate, long rpno, long edano, List<(long rpno, long edano, int hokenId)> hokenIds)
        {
            int? ret = 0;
            if (hokenIds.Any(p => p.rpno == rpno && p.edano == edano))
            {
                ret = _insuranceRepository.GetPtHokenInf(hpId, hokenIds.Find(p => p.rpno == rpno && p.edano == edano).hokenId, ptId, sinDate)?.HokenKbn;
            }

            return ret == null ? 0 : ret.Value;
        }

        public int GetHokenKbn(int odrHokenKbn)
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

        public List<int> GetCheckHokenKbns(int odrHokenKbn, int hokensyuHandling)
        {
            var results = new List<int>();

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

        public List<int> GetCheckSanteiKbns(int odrHokenKbn, int hokensyuHandling)
        {
            var results = new List<int> { 0 };

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

        private List<CheckedSpecialItem> ExecuteDensiSantei(long ptId, int hpId, int endDate, long raiinNo, int hokensyuHandling, int sinDate, int syosinDate, double suryo, string itemName, List<DensiSanteiKaisuModel> densiSanteiKaisuModels, OrdInfDetailModel odrDetail, List<(long rpno, long edano, int hokenId)> hokenIds, List<OrdInfDetailModel> allOdrInfDetail, List<ItemGrpMstModel> itemGrpMsts)
        {
            List<CheckedSpecialItem> checkSpecialItemList = new();

            foreach (var densiSanteiKaisu in densiSanteiKaisuModels)
            {
                var sTerm = string.Empty;
                int startDate = 0;

                var checkHokenKbnTmp = new List<int>();
                checkHokenKbnTmp.AddRange(GetCheckHokenKbns(GetPtHokenKbn(hpId, ptId, sinDate, odrDetail.RpNo, odrDetail.RpEdaNo, hokenIds), hokensyuHandling));

                if (densiSanteiKaisu.TargetKbn == 1)
                {
                    // 健保のみ対象の場合はすべて対象
                }
                else if (densiSanteiKaisu.TargetKbn == 2)
                {
                    // 労災のみ対象の場合、健保は抜く
                    checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                }

                var checkSanteiKbnTmp = new List<int>();
                checkSanteiKbnTmp.AddRange(GetCheckSanteiKbns(GetPtHokenKbn(hpId, ptId, sinDate, odrDetail.RpNo, odrDetail.RpEdaNo, hokenIds), hokensyuHandling));

                CommonDensiSantei(hpId, densiSanteiKaisu, odrDetail, allOdrInfDetail, ref startDate, ref endDate, ref sTerm, sinDate, syosinDate);

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
                        var checkSpecialItem = new CheckedSpecialItem(CheckSpecialType.CalculationCount, string.Empty, errMsg, odrDetail.ItemCd);

                        checkSpecialItemList.Add(checkSpecialItem);
                    }
                }
                else
                {
                    double count = 0;
                    if (startDate >= 0)
                    {
                        var itemCds = new List<string>();

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
                        var checkSpecialItem = new CheckedSpecialItem(CheckSpecialType.CalculationCount, string.Empty, errMsg, odrDetail.ItemCd);

                        checkSpecialItemList.Add(checkSpecialItem);
                    }
                }
            }
            return checkSpecialItemList;
        }

        public void CommonDensiSantei(int hpId, DensiSanteiKaisuModel densiSanteiKaisu, OrdInfDetailModel odrDetail, List<OrdInfDetailModel> allOdrInfDetail, ref int startDate, ref int endDate, ref string sTerm, int sinDate, int syosinDate)
        {
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

        }
    }
}
