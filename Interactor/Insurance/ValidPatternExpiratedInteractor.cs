using Domain.Constant;
using Domain.Models.ReceptionInsurance;
using Helper.Common;
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidPatternExpirated;

namespace Interactor.Insurance
{
    public class ValidPatternExpiratedInteractor : IValidPatternExpiratedInputPort
    {
        private readonly IReceptionInsuranceRepository _insuranceResponsitory;
        public ValidPatternExpiratedInteractor(IReceptionInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public ValidPatternExpiratedOutputData Handle(ValidPatternExpiratedInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidHpId);
            }

            if (inputData.PtId < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidPtId);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidSinDate);
            }
            
            if (inputData.PatternHokenPid < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidPatternHokenPid);
            }
            
            if (inputData.PatternConfirmDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidPatternConfirmDate);
            }
            
            if (inputData.HokenInfStartDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidHokenInfStartDate);
            }

            if (inputData.HokenInfEndDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidHokenInfEndDate);
            }

            if (inputData.HokenMstStartDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidHokenMstStartDate);
            }

            if (inputData.HokenMstEndDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidHokenMstEndDate);
            }

            if (inputData.KohiConfirmDate1 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiConfirmDate1);
            }

            if (inputData.KohiHokenMstStartDate1 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate1);
            }

            if (inputData.KohiHokenMstEndDate1 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate1);
            }

            if (inputData.KohiConfirmDate2 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiConfirmDate2);
            }

            if (inputData.KohiHokenMstStartDate2 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate2);
            }

            if (inputData.KohiHokenMstEndDate2 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate2);
            }

            if (inputData.KohiConfirmDate3 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiConfirmDate3);
            }

            if (inputData.KohiHokenMstStartDate3 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate3);
            }

            if (inputData.KohiHokenMstEndDate3 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate3);
            }

            if (inputData.KohiConfirmDate4 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiConfirmDate4);
            }

            if (inputData.KohiHokenMstStartDate4 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate4);
            }

            if (inputData.KohiHokenMstEndDate4 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate4);
            }

            if (inputData.PatientInfBirthday < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidPatientInfBirthday);
            }

            if (inputData.PatternHokenKbn < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidPatternExpiratedStatus.InvalidPatternHokenKbn);
            }

            var result = getMessageCheckValidatePattern(inputData);
            return result;
        }

        private ValidPatternExpiratedOutputData getMessageCheckValidatePattern(ValidPatternExpiratedInputData inputData)
        {
            var result = string.Empty;
            if (inputData.PatternHokenKbn > 0)
            {
                switch (inputData.PatternHokenKbn)
                {
                    // 自費
                    case 0:
                        // ignore
                        break;
                    // 社保
                    case 1:
                    // 国保
                    case 2:
                        var checkValidConfirmDateAgeCheck = _insuranceResponsitory.IsValidAgeCheck(inputData.SinDate, inputData.PatternHokenPid, inputData.HpId, inputData.PtId, inputData.PatientInfBirthday);
                        if (!String.IsNullOrEmpty(checkValidConfirmDateAgeCheck))
                        {
                            return new ValidPatternExpiratedOutputData(false, checkValidConfirmDateAgeCheck, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidConfirmDateAgeCheck);
                        }
                        var checkValidConfirmDateHoken = IsValidConfirmDateHoken(inputData.SinDate, inputData.PatternIsExpirated, inputData.HokenInfIsJihi, inputData.HokenInfIsNoHoken, inputData.PatternIsExpirated, inputData.PatternConfirmDate);
                        if (!String.IsNullOrEmpty(checkValidConfirmDateHoken))
                        {
                            return new ValidPatternExpiratedOutputData(false, checkValidConfirmDateHoken, TypeMessage.TypeMessageConfirmation, ValidPatternExpiratedStatus.InvalidConfirmDateHoken);
                        }
                        var checkValidHokenMstDate = IsValidHokenMstDate(inputData.HokenInfStartDate, inputData.HokenInfEndDate, inputData.SinDate, inputData.IsHaveHokenMst, inputData.HokenMstStartDate, inputData.HokenMstEndDate, inputData.HokenMstDisplayTextMaster);
                        if (!String.IsNullOrEmpty(checkValidHokenMstDate))
                        {
                            return new ValidPatternExpiratedOutputData(false, checkValidHokenMstDate, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidHokenMstDate);
                        }
                        if (!inputData.IsEmptyKohi1)
                        {
                            var checkValidKohi1 = IsValidConfirmDateKohi(inputData.KohiConfirmDate1, inputData.SinDate, inputData.PatternIsExpirated, 1);
                            if (!String.IsNullOrEmpty(checkValidKohi1))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohi1, TypeMessage.TypeMessageConfirmation, ValidPatternExpiratedStatus.InvalidConfirmDateKohi1);
                            }
                            var checkValidKohiMst1 = IsValidMasterDateKohi(inputData.SinDate, 1, inputData.KohiHokenMstDisplayTextMaster1, inputData.KohiHokenMstStartDate1, inputData.KohiHokenMstEndDate1, inputData.IsKohiHaveHokenMst1);
                            if (!String.IsNullOrEmpty(checkValidKohiMst1))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohiMst1, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidMasterDateKohi1);
                            }
                        }
                        if (!inputData.IsEmptyKohi2)
                        {
                            var checkValidKohi2 = IsValidConfirmDateKohi(inputData.KohiConfirmDate2, inputData.SinDate, inputData.PatternIsExpirated, 2);
                            if (!String.IsNullOrEmpty(checkValidKohi2))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohi2, TypeMessage.TypeMessageConfirmation, ValidPatternExpiratedStatus.InvalidConfirmDateKohi2);
                            }
                            var checkValidKohiMst2 = IsValidMasterDateKohi(inputData.SinDate, 2, inputData.KohiHokenMstDisplayTextMaster2, inputData.KohiHokenMstStartDate2, inputData.KohiHokenMstEndDate2, inputData.IsKohiHaveHokenMst2);
                            if (!String.IsNullOrEmpty(checkValidKohiMst2))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohiMst2, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidMasterDateKohi2);
                            }
                        }
                        if (!inputData.IsEmptyKohi3)
                        {
                            var checkValidKohi3 = IsValidConfirmDateKohi(inputData.KohiConfirmDate3, inputData.SinDate, inputData.PatternIsExpirated, 3);
                            if (!String.IsNullOrEmpty(checkValidKohi3))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohi3, TypeMessage.TypeMessageConfirmation, ValidPatternExpiratedStatus.InvalidConfirmDateKohi3);
                            }
                            var checkValidKohiMst3 = IsValidMasterDateKohi(inputData.SinDate, 3, inputData.KohiHokenMstDisplayTextMaster3, inputData.KohiHokenMstStartDate3, inputData.KohiHokenMstEndDate3, inputData.IsKohiHaveHokenMst3);
                            if (!String.IsNullOrEmpty(checkValidKohiMst3))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohiMst3, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidMasterDateKohi3);
                            }
                        }
                        if (!inputData.IsEmptyKohi4)
                        {
                            var checkValidKohi4 = IsValidConfirmDateKohi(inputData.KohiConfirmDate4, inputData.SinDate, inputData.PatternIsExpirated, 4);
                            if (!String.IsNullOrEmpty(checkValidKohi4))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohi4, TypeMessage.TypeMessageConfirmation, ValidPatternExpiratedStatus.InvalidConfirmDateKohi4);
                            }
                            var checkValidKohiMst4 = IsValidMasterDateKohi(inputData.SinDate, 4, inputData.KohiHokenMstDisplayTextMaster4, inputData.KohiHokenMstStartDate4, inputData.KohiHokenMstEndDate4, inputData.IsKohiHaveHokenMst4);
                            if (!String.IsNullOrEmpty(checkValidKohiMst4))
                            {
                                return new ValidPatternExpiratedOutputData(false, checkValidKohiMst4, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidMasterDateKohi4);
                            }
                        }
                        if (inputData.PatternIsExpirated)
                        {
                            var message = new string[] { "保険組合せ" };
                            result = String.Format(ErrorMessage.MessageType_mChk00020, message);
                            return new ValidPatternExpiratedOutputData(false, result, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidPatternIsExpirated);
                        }
                        var checkHasElderHoken = _insuranceResponsitory.HasElderHoken(inputData.SinDate, inputData.HpId, inputData.PtId, inputData.PatientInfBirthday);
                        if (!String.IsNullOrEmpty(checkHasElderHoken))
                        {
                            return new ValidPatternExpiratedOutputData(false, checkHasElderHoken, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidHasElderHoken);
                        }
                        return new ValidPatternExpiratedOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidPatternExpiratedStatus.ValidPatternExpiratedSuccess);
                    // 労災(短期給付)
                    case 11:
                    // 労災(傷病年金)
                    case 12:
                    // アフターケア
                    case 13:
                    // 自賠責
                    case 14:
                        if (inputData.PatternIsExpirated)
                        {
                            var message = new string[] { "保険組合せ" };
                            result = String.Format(ErrorMessage.MessageType_mChk00020, message);
                            return new ValidPatternExpiratedOutputData(false, result, TypeMessage.TypeMessageWarning, ValidPatternExpiratedStatus.InvalidHasElderHoken);
                        }
                        break;
                }
            }
            return new ValidPatternExpiratedOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidPatternExpiratedStatus.ValidPatternExpiratedSuccess);
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

        public string IsValidConfirmDateHoken(int sinDate, bool isExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, bool hokenInfIsExpirated, int hokenInfConfirmDate)
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
    }
}
