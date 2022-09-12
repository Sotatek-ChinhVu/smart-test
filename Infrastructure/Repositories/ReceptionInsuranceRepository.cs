using Domain.Constant;
using Domain.Models.Insurance;
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

        public bool CheckPatternExpried(int hpId, long ptId, int sinDate, int hokenId)
        {
            var result = false;
            //hungdm - gán
            var hokenPid = 0;
            var hokenInf = _tenantDataContext.PtHokenInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == hokenId);
            if (hokenInf != null)
            {
                var ptRousaiTenkis = _tenantDataContext.PtRousaiTenkis.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.HokenId == hokenInf.HokenId);
                var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(h => h.HokenNo == hokenInf.HokenNo && h.HokenEdaNo == hokenInf.HokenEdaNo);
                var dataHokenCheck = _tenantDataContext.PtHokenChecks.FirstOrDefault(x => x.HpId == hpId && x.PtID == ptId && x.IsDeleted == DeleteStatus.None && x.HokenId == hokenInf.HokenId);
                string houbetu = string.Empty;
                int futanRate = 0;
                int futanKbn = 0;
                int isHaveHokenMst = 0;
                int hokenMstSubNumber = 0;
                if (hokenMst != null)
                {
                    houbetu = hokenMst.Houbetu;
                    futanRate = hokenMst.FutanRate;
                    futanKbn = hokenMst.FutanKbn;
                    isHaveHokenMst = 1;
                    hokenMstSubNumber = hokenMst.HokenSbtKbn;
                }

                var tenkiSenkei = 0;
                var tenkiTenki = 0;
                var tenkiEndDate = 0;
                if (ptRousaiTenkis != null)
                {
                    tenkiSenkei = ptRousaiTenkis.Sinkei;
                    tenkiTenki = ptRousaiTenkis.Tenki;
                    tenkiEndDate = ptRousaiTenkis.EndDate;
                }
                var itemSelected = new HokenInfModel(
                                        hpId,
                                        ptId,
                                        hokenInf.HokenId,
                                        hokenInf.SeqNo,
                                        hokenInf.HokenNo,
                                        hokenInf.HokenEdaNo,
                                        hokenInf.HokenKbn,
                                        hokenInf.HokensyaNo ?? string.Empty,
                                        hokenInf.Kigo ?? string.Empty,
                                        hokenInf.Bango ?? string.Empty,
                                        hokenInf.EdaNo ?? string.Empty,
                                        hokenInf.HonkeKbn,
                                        hokenInf.StartDate,
                                        hokenInf.EndDate,
                                        hokenInf.SikakuDate,
                                        hokenInf.KofuDate,
                                        GetConfirmDateHokenInf(dataHokenCheck),
                                        hokenInf.KogakuKbn,
                                        hokenInf.TasukaiYm,
                                        hokenInf.TokureiYm1,
                                        hokenInf.TokureiYm2,
                                        hokenInf.GenmenKbn,
                                        hokenInf.GenmenRate,
                                        hokenInf.GenmenGaku,
                                        hokenInf.SyokumuKbn,
                                        hokenInf.KeizokuKbn,
                                        hokenInf.Tokki1 ?? string.Empty,
                                        hokenInf.Tokki2 ?? string.Empty,
                                        hokenInf.Tokki3 ?? string.Empty,
                                        hokenInf.Tokki4 ?? string.Empty,
                                        hokenInf.Tokki5 ?? string.Empty,
                                        hokenInf.RousaiKofuNo ?? string.Empty,
                                        nenkinBango: NenkinBango(hokenInf.RousaiKofuNo),
                                        hokenInf.RousaiRoudouCd ?? string.Empty,
                                        hokenInf.RousaiKofuNo ?? string.Empty,
                                        hokenInf.RousaiSaigaiKbn,
                                        hokenInf.RousaiKantokuCd ?? string.Empty,
                                        hokenInf.RousaiSyobyoDate,
                                        hokenInf.RyoyoStartDate,
                                        hokenInf.RyoyoEndDate,
                                        hokenInf.RousaiSyobyoCd ?? string.Empty,
                                        hokenInf.RousaiJigyosyoName ?? string.Empty,
                                        hokenInf.RousaiPrefName ?? string.Empty,
                                        hokenInf.RousaiCityName ?? string.Empty,
                                        hokenInf.RousaiReceCount,
                                        tenkiSenkei,
                                        tenkiTenki,
                                        tenkiEndDate,
                                        houbetu,
                                        futanRate,
                                        futanKbn,
                                        sinDate,
                                        hokenInf.JibaiHokenName ?? string.Empty,
                                        hokenInf.JibaiHokenTanto ?? string.Empty,
                                        hokenInf.JibaiHokenTel ?? string.Empty,
                                        hokenInf.JibaiJyusyouDate,
                                        isHaveHokenMst,
                                        hokenMstSubNumber,
                                        hokenInf.Houbetu ?? string.Empty
                                        );
                // Check
                if (itemSelected != null)
                {
                    var isValidExpiredPattern = itemSelected.IsExpirated;
                    switch (itemSelected.HokenKbn)
                    {
                        // 自費
                        case 0:
                            // ignore
                            break;
                        // 社保
                        case 1:
                        // 国保
                        case 2:
                            if (itemSelected != null)
                            {
                                if (!IsValidAgeCheck(sinDate, hokenPid))
                                {
                                    return false;
                                }
                                if (!IsValidConfirmDateHoken())
                                {
                                    return false;
                                }

                                if (!IsValidHokenMstDate())
                                {
                                    return false;
                                }
                            }
                            if (SelectedKohi1 != null && !SelectedKohi1.IsEmptyModel)
                            {
                                if (!IsValidConfirmDateKohi1())
                                {
                                    return false;
                                }

                                if (!IsValidMasterDateKohi1())
                                {
                                    return false;
                                }
                            }
                            if (SelectedKohi2 != null && !SelectedKohi2.IsEmptyModel)
                            {
                                if (!IsValidConfirmDateKohi2())
                                {
                                    return false;
                                }

                                if (!IsValidMasterDateKohi2())
                                {
                                    return false;
                                }
                            }
                            if (SelectedKohi3 != null && !SelectedKohi3.IsEmptyModel)
                            {
                                if (!IsValidConfirmDateKohi3())
                                {
                                    return false;
                                }

                                if (!IsValidMasterDateKohi3())
                                {
                                    return false;
                                }
                            }
                            if (SelectedKohi4 != null && !SelectedKohi4.IsEmptyModel)
                            {
                                if (!IsValidConfirmDateKohi4())
                                {
                                    return false;
                                }

                                if (!IsValidMasterDateKohi4())
                                {
                                    return false;
                                }
                            }
                            if (!_isValidExpiredPattern())
                            {
                                return false;
                            }
                            return HasElderHoken();
                        // 労災(短期給付)
                        case 11:
                        // 労災(傷病年金)
                        case 12:
                        // アフターケア
                        case 13:
                        // 自賠責
                        case 14:
                            if (!_isValidExpiredPattern())
                            {
                                return false;
                            }
                            if (!AutoConfirmRousaiJibai())
                            {
                                return false;
                            }
                            break;
                    }
                }




            }
            return result;

        }

        private bool IsValidHokenMstDate(int startDate, int endDate, int sinDate)
        {
            int HokenStartDate = startDate;
            int HokenEndDate = endDate;
            // 期限切れﾁｪｯｸ(有効保険の場合のみ)
            if ((HokenStartDate <= sinDate || HokenStartDate == 0)
                && (HokenEndDate >= sinDate || HokenEndDate == 0))
            {
                if (startDate > sinDate)
                {
                    return false;
                }
                if (endDate < sinDate)
                {
                    return false;
                }
            }

            return true;
        }


        private bool IsValidAgeCheck(int sinDate, int hokenPid)
        {
            var validPattern = _tenantDataContext.PtHokenPatterns.Where(pattern => pattern.IsDeleted == DeleteTypes.None &&
                                                                (pattern.StartDate <= sinDate && pattern.EndDate >= sinDate) &&
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
                if (SelectedHokenInf.IsAddNew || (SelectedHokenInf.IsAddHokenCheck && SelectedHokenInf.HokenChecks.Count <= 0))
                {
                    PtHokenCheckModel hokenCheck = CreateHokenCheckModel(1, SelectedHokenInf.HokenId);
                    SelectedHokenInf.AddConfirmDate(hokenCheck);
                }
                else
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
