using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.ReceptionInsurance;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidKohi;

namespace Interactor.Insurance
{
    public class ValidateKohiInteractor : IValidKohiInputPort
    {
        private readonly IReceptionInsuranceRepository _insuranceResponsitory;
        public ValidateKohiInteractor(IReceptionInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public ValidKohiOutputData Handle(ValidKohiInputData inputData)
        {
            if (inputData.SinDate < 0)
            {
                return new ValidKohiOutputData(false, string.Empty, ValidKohiStatus.InvalidSindate);
            }
            if (inputData.PtBirthday < 0)
            {
                return new ValidKohiOutputData(false, string.Empty, ValidKohiStatus.InvalidPtBirthday);
            }
            //IsValidKohi1
            var checkMessageKohi1 = IsValidKohi(inputData.IsKohiEmptyModel1, inputData.IsSelectedKohiMst1, inputData.SelectedKohiFutansyaNo1, inputData.SelectedKohiJyukyusyaNo1, inputData.SelectedKohiTokusyuNo1, inputData.SelectedKohiStartDate1, inputData.SelectedKohiEndDate1, inputData.SelectedKohiConfirmDate1, inputData.SelectedKohiMstFutansyaCheckFlag1, inputData.SelectedKohiMstJyukyusyaCheckFlag1, inputData.SelectedKohiMst1TokusyuCheckFlag1, inputData.SelectedKohiMstStartDate1, inputData.SelectedKohiMstEndDate1, inputData.SelectedKohiMstDisplayText1, 1, inputData.SinDate, inputData.SelectedKohiIsAddNew1);
            if (!checkMessageKohi1.Result)
            {
                return checkMessageKohi1;
            }
            // check Kohi No Function1
            var checkMessageKohiNoFnc1 = IsValidKohiNo_Fnc(inputData.IsKohiEmptyModel1, inputData.IsSelectedKohiMst1, inputData.SelectedKohiHokenNo1, inputData.SelectedKohiFutansyaNo1, inputData.SelectedKohiTokusyuNo1, inputData.SelectedKohiMstJyukyusyaCheckFlag1, inputData.SelectedKohiMstFutansyaCheckFlag1, inputData.SelectedKohiMstJyuKyuCheckDigit1, inputData.SelectedKohiMstCheckDigit1, inputData.SelectedKohiMstHoubetu1, inputData.SelectedKohiJyukyusyaNo1, inputData.SelectedKohiMstAgeStart1, inputData.SelectedKohiMstAgeEnd1, 1, inputData.PtBirthday);
            if (!checkMessageKohiNoFnc1.Result)
            {
                return checkMessageKohiNoFnc1;
            }
            //IsValidKohi2
            var checkMessageKohi2 = IsValidKohi(inputData.IsKohiEmptyModel2, inputData.IsSelectedKohiMst2, inputData.SelectedKohiFutansyaNo2, inputData.SelectedKohiJyukyusyaNo2, inputData.SelectedKohiTokusyuNo2, inputData.SelectedKohiStartDate2, inputData.SelectedKohiEndDate2, inputData.SelectedKohiConfirmDate2, inputData.SelectedKohiMstFutansyaCheckFlag2, inputData.SelectedKohiMstJyukyusyaCheckFlag2, inputData.SelectedKohiMst2TokusyuCheckFlag2, inputData.SelectedKohiMstStartDate2, inputData.SelectedKohiMstEndDate2, inputData.SelectedKohiMstDisplayText2, 2, inputData.SinDate, inputData.SelectedKohiIsAddNew2);
            if (!checkMessageKohi2.Result)
            {
                return checkMessageKohi2;
            }
            // check Kohi No Function 2
            var checkMessageKohiNoFnc2 = IsValidKohiNo_Fnc(inputData.IsKohiEmptyModel2, inputData.IsSelectedKohiMst2, inputData.SelectedKohiHokenNo2, inputData.SelectedKohiFutansyaNo2, inputData.SelectedKohiTokusyuNo2, inputData.SelectedKohiMstJyukyusyaCheckFlag2, inputData.SelectedKohiMstFutansyaCheckFlag2, inputData.SelectedKohiMstJyuKyuCheckDigit2, inputData.SelectedKohiMstCheckDigit2, inputData.SelectedKohiMstHoubetu2, inputData.SelectedKohiJyukyusyaNo2, inputData.SelectedKohiMstAgeStart2, inputData.SelectedKohiMstAgeEnd2, 2, inputData.PtBirthday);
            if (!checkMessageKohiNoFnc2.Result)
            {
                return checkMessageKohiNoFnc2;
            }
            //IsValidKohi3
            var checkMessageKohi3 = IsValidKohi(inputData.IsKohiEmptyModel3, inputData.IsSelectedKohiMst3, inputData.SelectedKohiFutansyaNo3, inputData.SelectedKohiJyukyusyaNo3, inputData.SelectedKohiTokusyuNo3, inputData.SelectedKohiStartDate3, inputData.SelectedKohiEndDate3, inputData.SelectedKohiConfirmDate3, inputData.SelectedKohiMstFutansyaCheckFlag3, inputData.SelectedKohiMstJyukyusyaCheckFlag3, inputData.SelectedKohiMst3TokusyuCheckFlag3, inputData.SelectedKohiMstStartDate3, inputData.SelectedKohiMstEndDate3, inputData.SelectedKohiMstDisplayText3, 3, inputData.SinDate, inputData.SelectedKohiIsAddNew3);
            if (!checkMessageKohi3.Result)
            {
                return checkMessageKohi3;
            }
            // check Kohi No Function 3
            var checkMessageKohiNoFnc3 = IsValidKohiNo_Fnc(inputData.IsKohiEmptyModel3, inputData.IsSelectedKohiMst3, inputData.SelectedKohiHokenNo3, inputData.SelectedKohiFutansyaNo3, inputData.SelectedKohiTokusyuNo3, inputData.SelectedKohiMstJyukyusyaCheckFlag3, inputData.SelectedKohiMstFutansyaCheckFlag3, inputData.SelectedKohiMstJyuKyuCheckDigit3, inputData.SelectedKohiMstCheckDigit3, inputData.SelectedKohiMstHoubetu3, inputData.SelectedKohiJyukyusyaNo3, inputData.SelectedKohiMstAgeStart3, inputData.SelectedKohiMstAgeEnd3, 3, inputData.PtBirthday);
            if (!checkMessageKohiNoFnc3.Result)
            {
                return checkMessageKohiNoFnc3;
            }
            //IsValidKohi4
            var checkMessageKohi4 = IsValidKohi(inputData.IsKohiEmptyModel4, inputData.IsSelectedKohiMst4, inputData.SelectedKohiFutansyaNo4, inputData.SelectedKohiJyukyusyaNo4, inputData.SelectedKohiTokusyuNo4, inputData.SelectedKohiStartDate4, inputData.SelectedKohiEndDate4, inputData.SelectedKohiConfirmDate4, inputData.SelectedKohiMstFutansyaCheckFlag4, inputData.SelectedKohiMstJyukyusyaCheckFlag4, inputData.SelectedKohiMst4TokusyuCheckFlag4, inputData.SelectedKohiMstStartDate4, inputData.SelectedKohiMstEndDate4, inputData.SelectedKohiMstDisplayText4, 4, inputData.SinDate, inputData.SelectedKohiIsAddNew4);
            if (!checkMessageKohi4.Result)
            {
                return checkMessageKohi4;
            }
            // check Kohi No Function 4
            var checkMessageKohiNoFnc4 = IsValidKohiNo_Fnc(inputData.IsKohiEmptyModel4, inputData.IsSelectedKohiMst4, inputData.SelectedKohiHokenNo4, inputData.SelectedKohiFutansyaNo4, inputData.SelectedKohiTokusyuNo4, inputData.SelectedKohiMstJyukyusyaCheckFlag4, inputData.SelectedKohiMstFutansyaCheckFlag4, inputData.SelectedKohiMstJyuKyuCheckDigit4, inputData.SelectedKohiMstCheckDigit4, inputData.SelectedKohiMstHoubetu4, inputData.SelectedKohiJyukyusyaNo4, inputData.SelectedKohiMstAgeStart4, inputData.SelectedKohiMstAgeEnd4, 4, inputData.PtBirthday);
            if (!checkMessageKohiNoFnc4.Result)
            {
                return checkMessageKohiNoFnc4;
            }
            var checkMessageKohiAll = IsvalidKohiAll(inputData.IsKohiEmptyModel1, inputData.IsKohiEmptyModel2, inputData.IsKohiEmptyModel3, inputData.IsKohiEmptyModel4,
                                                     inputData.SelectedKohiFutansyaNo1, inputData.SelectedKohiJyukyusyaNo1, inputData.SelectedKohiMstDisplayText1, inputData.SelectedKohiMstStartDate1, inputData.SelectedKohiEndDate1, inputData.SelectedKohiConfirmDate1,
                                                     inputData.SelectedKohiFutansyaNo2, inputData.SelectedKohiJyukyusyaNo2, inputData.SelectedKohiMstDisplayText2, inputData.SelectedKohiMstStartDate2, inputData.SelectedKohiEndDate2, inputData.SelectedKohiConfirmDate2,
                                                     inputData.SelectedKohiFutansyaNo3, inputData.SelectedKohiJyukyusyaNo3, inputData.SelectedKohiMstDisplayText3, inputData.SelectedKohiMstStartDate3, inputData.SelectedKohiEndDate3, inputData.SelectedKohiConfirmDate3,
                                                     inputData.SelectedKohiFutansyaNo4, inputData.SelectedKohiJyukyusyaNo4, inputData.SelectedKohiMstDisplayText4, inputData.SelectedKohiMstStartDate4, inputData.SelectedKohiEndDate4, inputData.SelectedKohiConfirmDate4);
            if (!checkMessageKohiAll.Result)
            {
                return checkMessageKohiAll;
            }
            return new ValidKohiOutputData(true, string.Empty, ValidKohiStatus.InvalidSuccess);
        }

        private ValidKohiOutputData IsValidKohi(bool isKohiModdel, bool isHokenMstModel, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenMstIsFutansyaCheckFlag, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsTokusyuCheckFlag, int hokenMstModelStartDate, int hokenMstModelEndDate, string hokenMstDisplayText, int numberKohi, int sinDate, bool isAddNew)
        {
            var message = "";
            var numberMessage = "";

            if (numberKohi == 1)
            {
                numberMessage = "１";
            }
            else if (numberKohi == 2)
            {
                numberMessage = "２";
            }
            else if (numberKohi == 3)
            {
                numberMessage = "３";
            }
            else
            {
                numberMessage = "４";
            }

            if (isKohiModdel)
            {
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel4);
                }
            }


            if (isHokenMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty4);
                }
            }
            if (string.IsNullOrEmpty(futansyaNo.Trim())
                && hokenMstIsFutansyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty4);
                }
            }
            else if (string.IsNullOrEmpty(jyukyusyaNo.Trim())
                && hokenMstIsJyukyusyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidJyukyusyaNo1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidJyukyusyaNo2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidJyukyusyaNo3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidJyukyusyaNo4);
                }
            }
            else if (string.IsNullOrEmpty(tokusyuNo.Trim())
                && hokenMstIsTokusyuCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidTokusyuNo1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidTokusyuNo2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidTokusyuNo3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidTokusyuNo4);
                }
            }
            if (!string.IsNullOrEmpty(futansyaNo) && Int32.Parse(futansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNo01);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNo02);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNo03);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNo04);
                }
            }
            int kohiYukoFromDate = startDate;
            int kohiYukoToDate = endDate;
            if (kohiYukoFromDate != 0 && kohiYukoToDate != 0 && kohiYukoFromDate > kohiYukoToDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "有効終了日", "公費" + numberMessage + "有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiYukoDate1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiYukoDate2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiYukoDate3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiYukoDate4);
                }
            }

            // check confirm date kohi
            var checkMessageIsValidConfirmDateKohi = IsValidConfirmDateKohi(confirmDate, numberMessage, sinDate, isAddNew);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateKohi))
            {
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiConfirmDate1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiConfirmDate2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiConfirmDate3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiConfirmDate4);
                }
            }

            // master date kohi IsValidMasterDateKohi
            if (!isHokenMstModel)
            {
                if (hokenMstModelStartDate > sinDate)
                {
                    var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelStartDate) + "～)", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    if (numberKohi == 1)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstStartDate1);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstStartDate2);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstStartDate3);
                    }
                    else
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstStartDate4);
                    }
                }
                if (hokenMstModelEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelEndDate) + "～)", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    if (numberKohi == 1)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEndDate1);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEndDate2);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEndDate3);
                    }
                    else
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEndDate4);
                    }
                }
            }

            return new ValidKohiOutputData(true, message, ValidKohiStatus.InvalidSuccess);
        }

        private string IsValidConfirmDateKohi(int confirmDate, string numberMessage, int sinDate, bool isAddNew)
        {
            var message = "";
            int kouhi1ConfirmDate = confirmDate;
            int confirmKohi1YM = Int32.Parse(CIUtil.Copy(kouhi1ConfirmDate.ToString(), 1, 6));
            if (kouhi1ConfirmDate == 0
                || sinDate != confirmKohi1YM)
            {
                // 公１・保険証確認日ﾁｪｯｸ(有効保険・新規保険の場合のみ)
                if (isAddNew)
                {
                    return message;
                }
                else
                {
                    var paramsMessage = new string[] { "公費" + numberMessage, "受給者証等" };
                    message = String.Format(ErrorMessage.MessageType_mChk00030, paramsMessage);
                    return message;
                }
            }
            return message;
        }

        private ValidKohiOutputData IsvalidKohiAll(bool isKohiEmptyModel1, bool isKohiEmptyModel2, bool isKohiEmptyModel3, bool isKohiEmptyModel4, string futansyaNo1, string jyukyusyaNo1, string displayTextMaster1, int startDate1, int endDate1, int confirmDate1, string futansyaNo2, string jyukyusyaNo2, string displayTextMaster2, int startDate2, int endDate2, int confirmDate2, string futansyaNo3, string jyukyusyaNo3, string displayTextMaster3, int startDate3, int endDate3, int confirmDate3, string futansyaNo4, string jyukyusyaNo4, string displayTextMaster4, int startDate4, int endDate4, int confirmDate4)
        {
            var message = "";
            if (isKohiEmptyModel2 && isKohiEmptyModel1)
            {
                var paramsMessage = new string[] { "公費１" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmpty21);
            }

            if (isKohiEmptyModel3)
            {
                if (isKohiEmptyModel1)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmpty31);
                }

                if (isKohiEmptyModel2)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmpty32);
                }
            }

            if (isKohiEmptyModel4)
            {
                if (isKohiEmptyModel1)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmpty41);
                }

                if (isKohiEmptyModel2)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmpty42);
                }

                if (isKohiEmptyModel3)
                {
                    var paramsMessage = new string[] { "公費３" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmpty43);
                }
            }
            // check duplicate 1
            if (!isKohiEmptyModel1 && ((!isKohiEmptyModel2 && (futansyaNo1 == futansyaNo2 && jyukyusyaNo1 == jyukyusyaNo2 && displayTextMaster1 == displayTextMaster2 && startDate1 == startDate2 && endDate1 == endDate2 && confirmDate1 == confirmDate2))
                   || (!isKohiEmptyModel3 && (futansyaNo1 == futansyaNo3 && jyukyusyaNo1 == jyukyusyaNo3 && displayTextMaster1 == displayTextMaster3 && startDate1 == startDate3 && endDate1 == endDate3 && confirmDate1 == confirmDate3))
                   || (!isKohiEmptyModel4 && (futansyaNo1 == futansyaNo4 && jyukyusyaNo1 == jyukyusyaNo4 && displayTextMaster1 == displayTextMaster4 && startDate1 == startDate4 && endDate1 == endDate4 && confirmDate1 == confirmDate4))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidDuplicateKohi1);
            }
            // check duplicate 2
            if (!isKohiEmptyModel2 && ((!isKohiEmptyModel1 && (futansyaNo1 == futansyaNo2 && jyukyusyaNo1 == jyukyusyaNo2 && displayTextMaster1 == displayTextMaster2 && startDate1 == startDate2 && endDate1 == endDate2 && confirmDate1 == confirmDate2))
                   || (!isKohiEmptyModel3 && (futansyaNo2 == futansyaNo3 && jyukyusyaNo2 == jyukyusyaNo3 && displayTextMaster2 == displayTextMaster3 && startDate2 == startDate3 && endDate2 == endDate3 && confirmDate2 == confirmDate3))
                   || (!isKohiEmptyModel4 && (futansyaNo2 == futansyaNo4 && jyukyusyaNo2 == jyukyusyaNo4 && displayTextMaster2 == displayTextMaster4 && startDate2 == startDate4 && endDate2 == endDate4 && confirmDate2 == confirmDate4))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidDuplicateKohi2);
            }
            // check duplicate 3
            if (!isKohiEmptyModel3 && ((!isKohiEmptyModel1 && (futansyaNo1 == futansyaNo3 && jyukyusyaNo1 == jyukyusyaNo3 && displayTextMaster1 == displayTextMaster3 && startDate1 == startDate3 && endDate1 == endDate3 && confirmDate1 == confirmDate3))
                   || (!isKohiEmptyModel2 && (futansyaNo2 == futansyaNo3 && jyukyusyaNo2 == jyukyusyaNo3 && displayTextMaster2 == displayTextMaster3 && startDate2 == startDate3 && endDate2 == endDate3 && confirmDate2 == confirmDate3))
                   || (!isKohiEmptyModel4 && (futansyaNo3 == futansyaNo4 && jyukyusyaNo3 == jyukyusyaNo4 && displayTextMaster3 == displayTextMaster4 && startDate3 == startDate4 && endDate3 == endDate4 && confirmDate3 == confirmDate4))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidDuplicateKohi3);
            }
            // check duplicate 4
            if (!isKohiEmptyModel4 && ((!isKohiEmptyModel1 && (futansyaNo1 == futansyaNo4 && jyukyusyaNo1 == jyukyusyaNo4 && displayTextMaster1 == displayTextMaster4 && startDate1 == startDate4 && endDate1 == endDate4 && confirmDate1 == confirmDate4))
                   || (!isKohiEmptyModel2 && (futansyaNo2 == futansyaNo4 && jyukyusyaNo2 == jyukyusyaNo4 && displayTextMaster2 == displayTextMaster4 && startDate2 == startDate4 && endDate2 == endDate4 && confirmDate2 == confirmDate4))
                   || (!isKohiEmptyModel3 && (futansyaNo3 == futansyaNo4 && jyukyusyaNo3 == jyukyusyaNo4 && displayTextMaster3 == displayTextMaster4 && startDate3 == startDate4 && endDate3 == endDate4 && confirmDate3 == confirmDate4))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidDuplicateKohi4);
            }

            return new ValidKohiOutputData(true, message, ValidKohiStatus.InvalidSuccess);
        }

        private ValidKohiOutputData IsValidKohiNo_Fnc(bool isKohiModel, bool isKohiMstModel, int hokenNo, string futansyaNo, string tokusyuNo, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsFutansyaCheckFlag, int hokenMstJyukyuCheckDigit, int hokenMstCheckDigit, string hokenMstHoubetu, string jyukyusyaNo, int hokenMstAgeStartDate, int hokenMstAgeEndDate, int numberKohi, int ptBirthday)
        {
            var message = "";
            var numberMessage = "";
            if (numberKohi == 1)
            {
                numberMessage = "１";
            }
            else if (numberKohi == 2)
            {
                numberMessage = "２";
            }
            else if (numberKohi == 3)
            {
                numberMessage = "３";
            }
            else
            {
                numberMessage = "４";
            }

            if (!isKohiModel)
            {
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiEmptyModel4);
                }
            }

            //公費１保険番号
            //公費１負担者番号入力なし
            if (!isKohiMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty3);
                }
                else
                {
                    return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidKohiHokenMstEmpty4);
                }
            }
            if (hokenNo != 0)
            {
                if (string.IsNullOrEmpty(futansyaNo)
                    && hokenMstIsFutansyaCheckFlag == 1)
                {
                    var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    if (numberKohi == 1)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty1);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty2);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty3);
                    }
                    else
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutansyaNoEmpty4);
                    }
                }
                //法別番号のチェック
                if (hokenMstIsFutansyaCheckFlag == 1)
                {
                    string intHBT = string.Empty;
                    if (!string.IsNullOrEmpty(futansyaNo) && futansyaNo.Length == 8)
                    {
                        intHBT = futansyaNo.Substring(0, 2);
                    }
                    //法別番号に一致する保険番号を初期値にセット
                    if (!string.IsNullOrEmpty(intHBT) && intHBT != hokenMstHoubetu)
                    {
                        //その法別のレコードがあるか　あればセット
                        var paramsMessage = new string[] { "公費" + numberMessage + "保険番号", "・公費" + numberMessage + "法別番号が一致しません。" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckHBT1);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckHBT2);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckHBT3);
                        }
                        else
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckHBT4);
                        }
                    }
                    //チェックデジット
                    if (hokenMstCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(futansyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitFutansyaNo1);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitFutansyaNo2);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitFutansyaNo3);
                        }
                        else
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitFutansyaNo4);
                        }
                    }
                    if (hokenMstIsJyukyusyaCheckFlag == 1 && hokenMstJyukyuCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(jyukyusyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo1);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo2);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo3);
                        }
                        else
                        {
                            return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo4);
                        }
                    }

                    //年齢チェック
                    int intAGE = -1;
                    if (ptBirthday != 0)
                    {
                        intAGE = CIUtil.SDateToAge(ptBirthday, Int32.Parse(DateTime.Now.ToString("yyyyMMdd")));
                    }
                    if (intAGE != -1)
                    {
                        int ageStartMst = hokenMstAgeStartDate;
                        int ageEndMst = hokenMstAgeEndDate;
                        if ((ageStartMst != 0 || ageEndMst != 999) && (intAGE < ageStartMst || intAGE > ageEndMst))
                        {
                            var paramsMessage = new string[] { "公費" + numberMessage + "の保険が適用年齢範囲外の", "・この保険の適用年齢範囲は。" + ageStartMst + "歳 〜 " + ageEndMst + "歳 です。" };
                            message = String.Format(ErrorMessage.MessageType_mEnt01041, paramsMessage);
                            if (numberKohi == 1)
                            {
                                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckAge1);
                            }
                            else if (numberKohi == 2)
                            {
                                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckAge2);
                            }
                            else if (numberKohi == 3)
                            {
                                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckAge3);
                            }
                            else
                            {
                                return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidMstCheckAge4);
                            }
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(futansyaNo)
                    || string.IsNullOrEmpty(jyukyusyaNo)
                    || string.IsNullOrEmpty(tokusyuNo))
                {
                    var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    if (numberKohi == 1)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutanJyoTokuNull1);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutanJyoTokuNull2);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutanJyoTokuNull3);
                    }
                    else
                    {
                        return new ValidKohiOutputData(false, message, ValidKohiStatus.InvalidFutanJyoTokuNull4);
                    }
                }
            }
            return new ValidKohiOutputData(true, message, ValidKohiStatus.InvalidSuccess);
        }
    }
}
