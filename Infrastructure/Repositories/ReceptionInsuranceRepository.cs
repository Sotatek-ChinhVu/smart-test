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

        public bool CheckPatternExpried(InsuranceModel itemInsurance)
        {
            var result = false;

            if (itemInsurance != null)
            {
                switch (itemInsurance.HokenKbn)
                {
                    // 自費
                    case 0:
                        // ignore
                        break;
                    // 社保
                    case 1:
                    // 国保
                    case 2:
                        if (!IsValidAgeCheck(itemInsurance.SinDate, itemInsurance.HokenPid))
                        {
                            return false;
                        }
                        if (!IsValidConfirmDateHoken(itemInsurance.SinDate, itemInsurance.IsExpirated, itemInsurance.IsJihi, itemInsurance.IsNoHoken, itemInsurance.IsExpirated, itemInsurance.ConfirmDate))
                        {
                            return false;
                        }

                        if (!IsValidHokenMstDate(itemInsurance.HokenInfStartDate, itemInsurance.HokenInfEndDate, itemInsurance.SinDate, itemInsurance.IsHaveHokenMst, itemInsurance.HokenMstStartDate, itemInsurance.HokenMstEndDate))
                        {
                            return false;
                        }
                        if (!itemInsurance.IsEmptyKohi1)
                        {
                            if (!IsValidConfirmDateKohi(itemInsurance.Kohi1.ConfirmDate, itemInsurance.SinDate, itemInsurance.IsExpirated))
                            {
                                return false;
                            }

                            if (!IsValidMasterDateKohi1(itemInsurance.Kohi1, itemInsurance.SinDate))
                            {
                                return false;
                            }
                        }
                        if (!itemInsurance.IsEmptyKohi2)
                        {
                            if (!IsValidConfirmDateKohi(itemInsurance.Kohi2.ConfirmDate, itemInsurance.SinDate, itemInsurance.IsExpirated))
                            {
                                return false;
                            }

                            if (!IsValidMasterDateKohi1(itemInsurance.Kohi2, itemInsurance.SinDate))
                            {
                                return false;
                            }
                        }
                        if (!itemInsurance.IsEmptyKohi3)
                        {
                            if (!IsValidConfirmDateKohi(itemInsurance.Kohi3.ConfirmDate, itemInsurance.SinDate, itemInsurance.IsExpirated))
                            {
                                return false;
                            }

                            if (!IsValidMasterDateKohi1(itemInsurance.Kohi3, itemInsurance.SinDate))
                            {
                                return false;
                            }
                        }
                        if (!itemInsurance.IsEmptyKohi4)
                        {
                            if (!IsValidConfirmDateKohi(itemInsurance.Kohi4.ConfirmDate, itemInsurance.SinDate, itemInsurance.IsExpirated))
                            {
                                return false;
                            }

                            if (!IsValidMasterDateKohi1(itemInsurance.Kohi4, itemInsurance.SinDate))
                            {
                                return false;
                            }
                        }
                        if (!itemInsurance.IsExpirated)
                        {
                            return false;
                        }
                        return HasElderHoken(itemInsurance.SinDate, itemInsurance.HpId, itemInsurance.PtId, itemInsurance.PatientInfBirthday);
                    // 労災(短期給付)
                    case 11:
                    // 労災(傷病年金)
                    case 12:
                    // アフターケア
                    case 13:
                    // 自賠責
                    case 14:
                        if (!itemInsurance.IsExpirated)
                        {
                            return false;
                        }
                        break;
                }
            }
            return result;
        }


        private bool HasElderHoken(int sinDate, int hpId, long ptId, int ptInfBirthday)
        {
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
                            return false;
                        }
                        else if (age < 65 && elderHokenQuery.Any())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        private bool IsValidConfirmDateKohi(int kohiConfirmDate, int sinDate, bool patternIsExpirated)
        {
            int Kouhi1ConfirmDate = kohiConfirmDate;
            int ConfirmKohi1YM = Int32.Parse(CIUtil.Copy(Kouhi1ConfirmDate.ToString(), 1, 6));
            int SinYM = Int32.Parse(CIUtil.Copy(sinDate.ToString(), 1, 6));
            if (Kouhi1ConfirmDate == 0
                || SinYM != ConfirmKohi1YM)
            {

                if (patternIsExpirated)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool IsValidMasterDateKohi1(KohiInfModel itemKohi, int sinDate)
        {
            if (!itemKohi.IsHaveKohiMst) return true;
            if (itemKohi.HokenMstModel.StartDate > sinDate)
            {
                return false;
            }
            if (itemKohi.HokenMstModel.EndDate < sinDate)
            {
                return false;
            }
            return true;
        }




        private bool IsValidAgeCheck(int sinDate, int hokenPid)
        {
            var validPattern = _tenantDataContext.PtHokenPatterns.Where(pattern => pattern.IsDeleted == DeleteTypes.None &&
                                                                (pattern.StartDate <= sinDate && pattern.EndDate >= sinDate)
                                                                    //&&
                                                                    //pattern.HokenInf.IsShahoOrKokuho &&
                                                                    //!(pattern.HokenInf.HokensyaNo.Length == 8
                                                                    // && (pattern.HokenInf.HokensyaNo.StartsWith("109") || pattern.HokenInf.HokensyaNo.StartsWith("99")))
                                                                    );


            validPattern = validPattern.Where(pattern => pattern.HokenPid == hokenPid);

            if (validPattern == null || validPattern.Count() == 0)
            {
                return true;
            }

            string checkParam = GetSettingParam(1005);
            var splittedParam = checkParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int invalidAgeCheck = 0;
            //PatientInf.Birthday
            int patientInfBirthDay = 0;

            foreach (var param in splittedParam)
            {
                int ageCheck = Int32.Parse(param.Trim());
                if (ageCheck == 0) continue;

                foreach (var pattern in validPattern)
                {
                    //confirmDate =pattern.HokenInf.ConfirmDate
                    int confirmDate = 0;
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
                return false;
            }
            return true;
        }

        private bool IsValidConfirmDateHoken(int sinDate, bool isExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, bool hokenInfIsExpirated, int hokenInfConfirmDate)
        {
            if (isExpirated)
            {
                return true;
            }
            if (hokenInfIsJihi || hokenInfIsNoHoken || hokenInfIsExpirated)
            {
                return true;
            }
            // 主保険・保険証確認日ﾁｪｯｸ(有効保険・新規保険の場合のみ)
            // check main hoken, apply for new hoken only
            int HokenConfirmDate = hokenInfConfirmDate;
            int ConfirmHokenYM = Int32.Parse(CIUtil.Copy(HokenConfirmDate.ToString(), 1, 6));
            int SinYM = Int32.Parse(CIUtil.Copy(sinDate.ToString(), 1, 6));
            if (HokenConfirmDate == 0
                || SinYM != ConfirmHokenYM)
            {
                return false;
            }
            return true;
        }

        private bool IsValidHokenMstDate(int startDate, int endDate, int sinDate, bool isHaveHokenMst, int hokenMstStartDate, int hokenMstEndDate)
        {
            int HokenStartDate = startDate;
            int HokenEndDate = endDate;
            // 期限切れﾁｪｯｸ(有効保険の場合のみ)
            if ((HokenStartDate <= sinDate || HokenStartDate == 0)
                && (HokenEndDate >= sinDate || HokenEndDate == 0))
            {
                if (!isHaveHokenMst)
                {
                    return true;
                }
                if (hokenMstStartDate > sinDate)
                {
                    return false;
                }
                if (hokenMstEndDate < sinDate)
                {

                    return false;
                }
            }

            return true;
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

        private int GetConfirmDateHokenInf(PtHokenCheck? ptHokenCheck)
        {
            return ptHokenCheck is null ? 0 : DateTimeToInt(ptHokenCheck.CheckDate);
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
