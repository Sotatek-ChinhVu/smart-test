using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.ReceptionInsurance;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionInsuranceRepository : IReceptionInsuranceRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public ReceptionInsuranceRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<ReceptionInsuranceModel> GetReceptionInsurance(int hpId, long ptId, int sinDate, bool isShowExpiredReception)
        {
            var listData = new List<ReceptionInsuranceModel>();
            var listhokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None)
                           .OrderBy(x => !(x.StartDate <= sinDate && x.EndDate >= sinDate))
                           .ThenByDescending(x => x.HokenId)
                           .ToList();
            var listKohi = _tenantDataContext.PtKohis.Where(entity => entity.HpId == hpId && entity.PtId == ptId)
                          .OrderBy(x => !(x.StartDate <= sinDate && x.EndDate >= sinDate))
                          .ThenByDescending(x => x.HokenId).ToList();

            if (listhokenInf.Count > 0)
            {
                foreach (var item in listhokenInf)
                {
                    var HokenMasterModel = _tenantDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
                    var isReceKisaiOrNoHoken = false;
                    var isExpirated = IsExpirated(item.StartDate, item.EndDate, sinDate);
                    if (HokenMasterModel != null)
                    {
                        isReceKisaiOrNoHoken = IsReceKisai(HokenMasterModel) || IsNoHoken(HokenMasterModel, item.HokenKbn, item.Houbetu ?? string.Empty);
                    }
                    if (!isReceKisaiOrNoHoken && (isShowExpiredReception || isExpirated))
                    {
                        var newItemHokenInfModel = new ReceptionInsuranceModel(
                                            item.HokenKbn,
                                            item.Kigo ?? string.Empty,
                                            item.Bango ?? string.Empty,
                                            item.StartDate,
                                            item.EndDate,
                                            GetConfirmDate(item.HokenId, HokenGroupConstant.HokenGroupHokenPattern),
                                            item.EdaNo ?? string.Empty,
                                            item.HokensyaNo ?? string.Empty,
                                            item.RousaiKofuNo ?? string.Empty,
                                            sinDate,
                                            1,
                                            0,
                                            "",
                                            "",
                                            item.HokenId,
                                            GetConfirmState(item.HokenKbn, item.Houbetu ?? string.Empty, hpId, ptId, sinDate, item.HokenId, 1, HokenMasterModel)
                                            );

                        listData.Add(newItemHokenInfModel);
                    }
                }
            }

            if (listKohi.Count > 0)
            {
                foreach (var item in listKohi)
                {
                    var HokenMasterModel = _tenantDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
                    var isExpirated = IsExpirated(item.StartDate, item.EndDate, sinDate);
                    if (isShowExpiredReception || isExpirated)
                    {
                        var newItemKohiModel = new ReceptionInsuranceModel(
                                            0,
                                            "",
                                            "",
                                            item.StartDate,
                                            item.EndDate,
                                            GetConfirmDate(item.HokenId, HokenGroupConstant.HokenGroupKohi),
                                            "",
                                            "",
                                            "",
                                            sinDate,
                                            0,
                                            1,
                                            item.FutansyaNo ?? string.Empty,
                                            item.JyukyusyaNo ?? string.Empty,
                                            item.HokenId,
                                            GetConfirmState(0, item.Houbetu ?? string.Empty, hpId, ptId, sinDate, item.HokenId, 2, HokenMasterModel)
                                            );

                        listData.Add(newItemKohiModel);
                    }
                }
            }

            return listData;
        }

        public string CheckPatternExpirated(int hpId, long ptId, int sinDate, int patternHokenPid, bool patternIsExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, int patternConfirmDate, int hokenInfStartDate, int hokenInfEndDate, bool isHaveHokenMst, int hokenMstStartDate, int hokenMstEndDate, string hokenMstDisplayTextMaster, bool isEmptyKohi1, bool isKohiHaveHokenMst1, int kohiConfirmDate1, string kohiHokenMstDisplayTextMaster1, int kohiHokenMstStartDate1, int kohiHokenMstEndDate1, bool isEmptyKohi2, bool isKohiHaveHokenMst2, int kohiConfirmDate2, string kohiHokenMstDisplayTextMaster2, int kohiHokenMstStartDate2, int kohiHokenMstEndDate2, bool isEmptyKohi3, bool isKohiHaveHokenMst3, int kohiConfirmDate3, string kohiHokenMstDisplayTextMaster3, int kohiHokenMstStartDate3, int kohiHokenMstEndDate3, bool isEmptyKohi4, bool isKohiHaveHokenMst4, int kohiConfirmDate4, string kohiHokenMstDisplayTextMaster4, int kohiHokenMstStartDate4, int kohiHokenMstEndDate4, int patientInfBirthday, int patternHokenKbn)
        {
            var result = "";

            if (patternHokenKbn > 0)
            {
                switch (patternHokenKbn)
                {
                    // 自費
                    case 0:
                        // ignore
                        break;
                    // 社保
                    case 1:
                    // 国保
                    case 2:
                        var checkValidAge = IsValidAgeCheck(sinDate, patternHokenPid, hpId, ptId, patientInfBirthday);
                        if (!String.IsNullOrEmpty(checkValidAge))
                        {
                            return checkValidAge;
                        }
                        var checkValidConfirmDateHoken = IsValidConfirmDateHoken(sinDate, patternIsExpirated, hokenInfIsJihi, hokenInfIsNoHoken, patternIsExpirated, patternConfirmDate);
                        if (!String.IsNullOrEmpty(checkValidConfirmDateHoken))
                        {
                            return checkValidConfirmDateHoken;
                        }
                        var checkValidHokenMstDate = IsValidHokenMstDate(hokenInfStartDate, hokenInfEndDate, sinDate, isHaveHokenMst, hokenMstStartDate, hokenMstEndDate, hokenMstDisplayTextMaster);
                        if (!String.IsNullOrEmpty(checkValidHokenMstDate))
                        {
                            return checkValidHokenMstDate;
                        }
                        if (!isEmptyKohi1)
                        {
                            var checkValidKohi1 = IsValidConfirmDateKohi(kohiConfirmDate1, sinDate, patternIsExpirated, 1);
                            if (!String.IsNullOrEmpty(checkValidKohi1))
                            {
                                return result;
                            }
                            var checkValidKohiMst1 = IsValidMasterDateKohi(sinDate, 1, kohiHokenMstDisplayTextMaster1, kohiHokenMstStartDate1, kohiHokenMstEndDate1, isKohiHaveHokenMst1);
                            if (!String.IsNullOrEmpty(checkValidKohiMst1))
                            {
                                return result;
                            }
                        }
                        if (!isEmptyKohi2)
                        {
                            var checkValidKohi2 = IsValidConfirmDateKohi(kohiConfirmDate2, sinDate, patternIsExpirated, 2);
                            if (!String.IsNullOrEmpty(checkValidKohi2))
                            {
                                return result;
                            }
                            var checkValidKohiMst2 = IsValidMasterDateKohi(sinDate, 2, kohiHokenMstDisplayTextMaster2, kohiHokenMstStartDate2, kohiHokenMstEndDate2, isKohiHaveHokenMst2);
                            if (!String.IsNullOrEmpty(checkValidKohiMst2))
                            {
                                return result;
                            }
                        }
                        if (!isEmptyKohi3)
                        {
                            var checkValidKohi3 = IsValidConfirmDateKohi(kohiConfirmDate3, sinDate, patternIsExpirated, 3);
                            if (!String.IsNullOrEmpty(checkValidKohi3))
                            {
                                return result;
                            }
                            var checkValidKohiMst3 = IsValidMasterDateKohi(sinDate, 3, kohiHokenMstDisplayTextMaster3, kohiHokenMstStartDate3, kohiHokenMstEndDate3, isKohiHaveHokenMst3);
                            if (!String.IsNullOrEmpty(checkValidKohiMst3))
                            {
                                return result;
                            }
                        }
                        if (!isEmptyKohi4)
                        {
                            var checkValidKohi4 = IsValidConfirmDateKohi(kohiConfirmDate4, sinDate, patternIsExpirated, 4);
                            if (!String.IsNullOrEmpty(checkValidKohi4))
                            {
                                return result;
                            }
                            var checkValidKohiMst4 = IsValidMasterDateKohi(sinDate, 4, kohiHokenMstDisplayTextMaster4, kohiHokenMstStartDate4, kohiHokenMstEndDate4, isKohiHaveHokenMst4);
                            if (!String.IsNullOrEmpty(checkValidKohiMst4))
                            {
                                return result;
                            }
                        }
                        if (patternIsExpirated)
                        {
                            var message = new string[] { "保険組合せ" };
                            result = String.Format(ErrorMessage.MessageType_mChk00020, message);
                            return result;
                        }
                        return HasElderHoken(sinDate, hpId, ptId, patientInfBirthday);
                    // 労災(短期給付)
                    case 11:
                    // 労災(傷病年金)
                    case 12:
                    // アフターケア
                    case 13:
                    // 自賠責
                    case 14:
                        if (patternIsExpirated)
                        {
                            var message = new string[] { "保険組合せ" };
                            result = String.Format(ErrorMessage.MessageType_mChk00020, message);
                            return result;
                        }
                        break;
                }
            }
            return result;
        }


        private string HasElderHoken(int sinDate, int hpId, long ptId, int ptInfBirthday)
        {
            var result = "";
            if (sinDate >= 20080401)
            {
                var listHokenPatterns = _tenantDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();

                if (listHokenPatterns != null && listHokenPatterns.Count > 0)
                {
                    var patternHokenOnly = listHokenPatterns.Where(pattern => pattern.IsDeleted == 0 && (pattern.StartDate <= sinDate && pattern.EndDate >= sinDate));
                    var listHokenInfs = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
                    int age = CIUtil.SDateToAge(ptInfBirthday, sinDate);

                    // hoken exist in at least 1 pattern
                    var inUsedHokens = listHokenInfs.Where(hoken => hoken.HokenId > 0 && hoken.IsDeleted == 0 && (hoken.StartDate <= sinDate && hoken.EndDate >= sinDate)
                                                                && patternHokenOnly.Any(pattern => pattern.HokenId == hoken.HokenId));

                    var elderHokenQuery = inUsedHokens.Where(hoken => hoken.EndDate >= sinDate
                                                                        && hoken.HokensyaNo != null && hoken.HokensyaNo != ""
                                                                        && hoken.HokensyaNo.Length == 8 && hoken.HokensyaNo.StartsWith("39"));

                    if (elderHokenQuery != null)
                    {
                        if (age >= 75 && !elderHokenQuery.Any())
                        {
                            var stringParams = new string[] { "後期高齢者保険が入力されていません。", "保険者証" };
                            result = string.Format(ErrorMessage.MessageType_mChk00080, stringParams);
                            return result;
                        }
                        else if (age < 65 && elderHokenQuery.Any())
                        {
                            var stringParamsCheck = new string[] { "後期高齢者保険の対象外の患者に、後期高齢者保険が登録されています。", "保険者証" };
                            result = string.Format(ErrorMessage.MessageType_mChk00080, stringParamsCheck);
                            return result;
                        }
                    }
                }
            }
            return result;
        }


        private string IsValidConfirmDateKohi(int kohiConfirmDate, int sinDate, bool patternIsExpirated, int numberKohi)
        {
            int Kouhi1ConfirmDate = kohiConfirmDate;
            int ConfirmKohi1YM = Int32.Parse(CIUtil.Copy(Kouhi1ConfirmDate.ToString(), 1, 6));
            int SinYM = Int32.Parse(CIUtil.Copy(sinDate.ToString(), 1, 6));
            string checkConfirmDateKohi = "";
            if (Kouhi1ConfirmDate == 0
                || SinYM != ConfirmKohi1YM)
            {

                if (patternIsExpirated)
                {
                    return checkConfirmDateKohi;
                }
                switch (numberKohi)
                {
                    case 1:
                        var stringParams1 = new string[] { "公費１", "受給者証等" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00030, stringParams1);
                        break;
                    case 2:
                        var stringParams2 = new string[] { "公費２", "受給者証等" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00030, stringParams2);
                        break;
                    case 3:
                        var stringParams3 = new string[] { "公費３", "受給者証等" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00030, stringParams3);
                        break;
                    case 4:
                        var stringParams4 = new string[] { "公費４", "受給者証等" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00030, stringParams4);
                        break;
                }
                return checkConfirmDateKohi;
            }
            return checkConfirmDateKohi;
        }

        private string IsValidMasterDateKohi(int sinDate, int numberKohi, string kohiHokenMstDisplayTextMaster, int kohiHokenMstStartDate, int kohiHokenMstEndDate, bool isHaveKohiMst)
        {
            string checkConfirmDateKohi = "";
            if (!isHaveKohiMst) return checkConfirmDateKohi;
            if (kohiHokenMstStartDate > sinDate)
            {
                switch (numberKohi)
                {
                    case 1:
                        var stringParams1 = new string[] { "公費１ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstStartDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams1);
                        break;
                    case 2:
                        var stringParams2 = new string[] { "公費２ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstStartDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams2);
                        break;
                    case 3:
                        var stringParams3 = new string[] { "公費３ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstStartDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams3);
                        break;
                    case 4:
                        var stringParams4 = new string[] { "公費４ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstStartDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams4);
                        break;
                }
                return checkConfirmDateKohi;
            }
            if (kohiHokenMstEndDate < sinDate)
            {
                switch (numberKohi)
                {
                    case 1:
                        var stringParams1 = new string[] { "公費１ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstEndDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams1);
                        break;
                    case 2:
                        var stringParams2 = new string[] { "公費２ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstEndDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams2);
                        break;
                    case 3:
                        var stringParams3 = new string[] { "公費３ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstEndDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams3);
                        break;
                    case 4:
                        var stringParams4 = new string[] { "公費４ '" + kohiHokenMstDisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiHokenMstEndDate) + "～)", "保険番号" };
                        checkConfirmDateKohi = string.Format(ErrorMessage.MessageType_mChk00080, stringParams4);
                        break;
                }
                return checkConfirmDateKohi;
            }
            return checkConfirmDateKohi;
        }

        private string IsValidAgeCheck(int sinDate, int hokenPid, int hpId, long ptId, int ptInfBirthday)
        {
            var checkResult = "";
            // pattern
            var listPattern = _tenantDataContext.PtHokenPatterns.Where(pattern => pattern.IsDeleted == DeleteTypes.None &&
                                                                (pattern.StartDate <= sinDate && pattern.EndDate >= sinDate)
                                                                && pattern.HpId == hpId && pattern.PtId == ptId
                                                                    );

            var listHokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.IsDeleted == DeleteTypes.None
                                                                && x.HpId == hpId && x.PtId == ptId
                                                                && ((x.HokenKbn == 1 && x.Houbetu != HokenConstant.HOUBETU_NASHI) || x.HokenKbn == 2)
                                                                && !(!String.IsNullOrEmpty(x.HokensyaNo) && x.HokensyaNo.Length == 8
                                                                        && (x.HokensyaNo.StartsWith("109") || x.HokensyaNo.StartsWith("99"))
                                                                ));
            var dataJoinPatternWithHokenInf = from ptHokenPattern in listPattern
                                              join ptHokenInf in listHokenInf on
                                                  new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                                                  new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
                                                  select new
                                                  {
                                                      ptHokenPattern,
                                                      ptHokenInf
                                                  };
            var validPattern = dataJoinPatternWithHokenInf.Where(pattern => pattern.ptHokenPattern.HokenPid == hokenPid);

            if (validPattern == null || validPattern.Count() == 0)
            {
                return checkResult;
            }

            string checkParam = GetSettingParam(1005);
            var splittedParam = checkParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int invalidAgeCheck = 0;
            //PatientInf.Birthday
            int patientInfBirthDay = ptInfBirthday;

            foreach (var param in splittedParam)
            {
                int ageCheck = Int32.Parse(param.Trim());
                if (ageCheck == 0) continue;

                foreach (var pattern in validPattern)
                {
                    var ptCheck = _tenantDataContext.PtHokenChecks.Where(x => x.HokenId == pattern.ptHokenPattern.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupHokenPattern)
                                    .OrderByDescending(x => x.CheckDate).FirstOrDefault();
                    int confirmDate = GetConfirmDateHokenInf(ptCheck);
                    if (!IsValidAgeCheckConfirm(ageCheck, confirmDate, patientInfBirthDay, sinDate) && invalidAgeCheck <= ageCheck)
                    {
                        invalidAgeCheck = ageCheck;
                    }
                }
            }

            if (invalidAgeCheck != 0)
            {
                string cardName;
                int age = CIUtil.SDateToAge(patientInfBirthDay, sinDate);
                if (age >= 70)
                {
                    cardName = "高齢受給者証";
                }
                else
                {
                    cardName = "保険証";
                }
                var stringParams = new string[] { $"{invalidAgeCheck}歳となりました。", cardName };
                checkResult = string.Format(ErrorMessage.MessageType_mChk00080, stringParams);
                return checkResult;
            }
            return checkResult;
        }

        private int GetConfirmDateHokenInf(PtHokenCheck? ptHokenCheck)
        {
            return ptHokenCheck is null ? 0 : DateTimeToInt(ptHokenCheck.CheckDate);
        }

        private string IsValidConfirmDateHoken(int sinDate, bool isExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, bool hokenInfIsExpirated, int hokenInfConfirmDate)
        {
            var checkComfirmDateHoken = "";
            if (isExpirated)
            {
                return checkComfirmDateHoken;
            }
            if (hokenInfIsJihi || hokenInfIsNoHoken || hokenInfIsExpirated)
            {
                return checkComfirmDateHoken;
            }
            // 主保険・保険証確認日ﾁｪｯｸ(有効保険・新規保険の場合のみ)
            // check main hoken, apply for new hoken only
            int HokenConfirmDate = hokenInfConfirmDate;
            int ConfirmHokenYM = Int32.Parse(CIUtil.Copy(HokenConfirmDate.ToString(), 1, 6));
            int SinYM = Int32.Parse(CIUtil.Copy(sinDate.ToString(), 1, 6));
            if (HokenConfirmDate == 0
                || SinYM != ConfirmHokenYM)
            {
                var stringParams = new string[] { "保険", "保険証" };
                checkComfirmDateHoken = string.Format(ErrorMessage.MessageType_mChk00030, stringParams);
                return checkComfirmDateHoken;
            }
            return checkComfirmDateHoken;
        }

        private string IsValidHokenMstDate(int startDate, int endDate, int sinDate, bool isHaveHokenMst, int hokenMstStartDate, int hokenMstEndDate, string displayTextMaster)
        {
            var checkValidHokenMst = "";
            int HokenStartDate = startDate;
            int HokenEndDate = endDate;
            // 期限切れﾁｪｯｸ(有効保険の場合のみ)
            if ((HokenStartDate <= sinDate || HokenStartDate == 0)
                && (HokenEndDate >= sinDate || HokenEndDate == 0))
            {
                if (!isHaveHokenMst)
                {
                    return checkValidHokenMst;
                }
                if (hokenMstStartDate > sinDate)
                {
                    var stringMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstStartDate) + "～)", "保険番号" };
                    checkValidHokenMst = string.Format(ErrorMessage.MessageType_mChk00080, stringMessage);
                    return checkValidHokenMst;
                }
                if (hokenMstEndDate < sinDate)
                {
                    var stringParams = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(hokenMstEndDate) + ")", "保険番号" };

                    checkValidHokenMst = string.Format(ErrorMessage.MessageType_mChk00080, stringParams);
                    return checkValidHokenMst;
                }
            }

            return checkValidHokenMst;
        }

        private string GetSettingParam(int groupCd, int grpEdaNo = 0, string defaultParam = "")
        {
            var systemConf = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            //Fix comment 894 (duong.vu)
            //Return value in DB if and only if Param is not null or white space
            if (systemConf != null && !string.IsNullOrWhiteSpace(systemConf.Param))
            {
                return systemConf.Param;
            }
            return defaultParam;
        }

        private bool IsValidAgeCheckConfirm(int ageCheck, int confirmDate, int patientInfBirthDay, int sinDate)
        {
            int birthDay = patientInfBirthDay;
            // 但し、2日生まれ以降の場合は翌月１日を誕生日とする。
            if (CIUtil.Copy(birthDay.ToString(), 7, 2) != "01")
            {
                int firstDay = birthDay / 100 * 100 + 1;
                int nextMonth = CIUtil.DateTimeToInt(CIUtil.IntToDate(firstDay).AddMonths(1));
                birthDay = nextMonth;
            }

            if (CIUtil.AgeChk(birthDay, sinDate, ageCheck)
                && !CIUtil.AgeChk(birthDay, confirmDate, ageCheck))
            {
                return false;
            }
            return true;
        }

        private static int DateTimeToInt(DateTime dateTime, string format = "yyyyMMdd")
        {
            int result = 0;
            result = Int32.Parse(dateTime.ToString(format));
            return result;
        }

        private int GetConfirmDate(int hokenId, int typeHokenGroup)
        {
            var validHokenCheck = _tenantDataContext.PtHokenChecks.Where(x => x.IsDeleted == 0 && x.HokenId == hokenId && x.HokenGrp == typeHokenGroup)
                .OrderByDescending(x => x.CheckDate)
                .ToList();
            if (!validHokenCheck.Any())
            {
                return 0;
            }
            return CIUtil.DateTimeToInt(validHokenCheck[0].CheckDate);
        }

        private int GetConfirmState(int hokenKbn, string houbetu, int hpId, long ptId, int sinDate, int hokenId, int hokenMstOrKohi, HokenMst? hokenMaster)
        {
            if (hokenMaster != null)
            {
                if (hokenMstOrKohi == 1)
                {
                    var IsReceKisaiOrNoHoken = IsReceKisai(hokenMaster) || IsNoHoken(hokenMaster, hokenKbn, houbetu);
                    // Jihi 100% or NoHoken
                    if (IsReceKisaiOrNoHoken)
                    {
                        return 1;
                    }
                }

                // HokenChecks
                var hokenChecks = _tenantDataContext.PtHokenChecks
                                    .Where(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == 0
                                                && x.HokenGrp == 1 && x.HokenId == hokenId && x.IsDeleted == 0)
                                    .OrderByDescending(x => x.CheckDate)
                                    .ToList();

                if (hokenChecks.Count == 0)
                {
                    return 0;
                }

                var now = CIUtil.IntToDate(sinDate);
                if (hokenChecks.Any(hk => hk.CheckDate.Year == now.Year && hk.CheckDate.Month == now.Month && hk.CheckDate.Day == now.Day))
                {
                    return 2;
                }
                int SinYM = Int32.Parse(CIUtil.Copy(sinDate.ToString(), 1, 6));
                foreach (var ptHokenCheck in hokenChecks)
                {
                    int currentConfirmYM = Int32.Parse(CIUtil.Copy(CIUtil.DateTimeToInt(ptHokenCheck.CheckDate).ToString(), 1, 6));
                    if (currentConfirmYM == SinYM)
                    {
                        return 3;
                    }
                }
                return 0;

            }
            else
                return 1;
        }

        private bool IsReceKisai(HokenMst HokenMasterModel)
        {

            if (HokenMasterModel != null)
            {
                return HokenMasterModel.ReceKisai == 3;
            }
            return false;

        }

        private bool IsNoHoken(HokenMst HokenMasterModel, int hokenKbn, string houbetu)
        {

            if (HokenMasterModel != null)
            {
                return HokenMasterModel.HokenSbtKbn == 0;
            }
            return hokenKbn == 1 && houbetu == HokenConstant.HOUBETU_NASHI;
        }

        private bool IsExpirated(int startDate, int endDate, int sinDate)
        {
            return !(startDate <= sinDate && endDate >= sinDate);
        }

        private string NenkinBango(string? rousaiKofuNo)
        {
            string nenkinBango = "";
            if (rousaiKofuNo != null && rousaiKofuNo.Length == 9)
            {
                nenkinBango = rousaiKofuNo.Substring(0, 2);
            }
            return nenkinBango;
        }
    }
}
