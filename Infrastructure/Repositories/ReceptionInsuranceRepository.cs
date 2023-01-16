using Domain.Constant;
using Domain.Models.ReceptionInsurance;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class ReceptionInsuranceRepository : RepositoryBase, IReceptionInsuranceRepository
    {
        public ReceptionInsuranceRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IEnumerable<ReceptionInsuranceModel> GetReceptionInsurance(int hpId, long ptId, int sinDate, bool isShowExpiredReception)
        {
            var listData = new List<ReceptionInsuranceModel>();
            var listhokenInf = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == DeleteStatus.None)
                           .OrderBy(x => !(x.StartDate <= sinDate && x.EndDate >= sinDate))
                           .ThenByDescending(x => x.HokenId)
                           .ToList();
            var listKohi = NoTrackingDataContext.PtKohis.Where(entity => entity.HpId == hpId && entity.PtId == ptId)
                          .OrderBy(x => !(x.StartDate <= sinDate && x.EndDate >= sinDate))
                          .ThenByDescending(x => x.HokenId).ToList();

            if (listhokenInf.Count > 0)
            {
                foreach (var item in listhokenInf)
                {
                    var HokenMasterModel = NoTrackingDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
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
                    var HokenMasterModel = NoTrackingDataContext.HokenMsts.Where(hoken => hoken.HokenNo == item.HokenNo && hoken.HokenEdaNo == item.HokenEdaNo).FirstOrDefault();
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

        public string HasElderHoken(int sinDate, int hpId, long ptId, int ptInfBirthday)
        {
            var result = "";
            if (sinDate >= 20080401)
            {
                var listHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();

                if (listHokenPatterns != null && listHokenPatterns.Count > 0)
                {
                    var patternHokenOnly = listHokenPatterns.Where(pattern => pattern.IsDeleted == 0 && (pattern.StartDate <= sinDate && pattern.EndDate >= sinDate));
                    var listHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
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
      
        public string IsValidAgeCheck(int sinDate, int hokenPid, int hpId, long ptId, int ptInfBirthday)
        {
            var checkResult = "";
            // pattern
            var listPattern = NoTrackingDataContext.PtHokenPatterns.Where(pattern => pattern.IsDeleted == DeleteTypes.None &&
                                                                (pattern.StartDate <= sinDate && pattern.EndDate >= sinDate)
                                                                && pattern.HpId == hpId && pattern.PtId == ptId
                                                                    );

            var listHokenInf = NoTrackingDataContext.PtHokenInfs.Where(x => x.IsDeleted == DeleteTypes.None
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
                    var ptCheck = NoTrackingDataContext.PtHokenChecks.Where(x => x.HokenId == pattern.ptHokenPattern.HokenId && x.HokenGrp == HokenGroupConstant.HokenGroupHokenPattern)
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

        private string GetSettingParam(int groupCd, int grpEdaNo = 0, string defaultParam = "")
        {
            var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
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
            var validHokenCheck = NoTrackingDataContext.PtHokenChecks.Where(x => x.IsDeleted == 0 && x.HokenId == hokenId && x.HokenGrp == typeHokenGroup)
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
                var hokenChecks = NoTrackingDataContext.PtHokenChecks
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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
