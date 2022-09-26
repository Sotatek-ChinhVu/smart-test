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


            return new ValidKohiOutputData(true, string.Empty, ValidKohiStatus.ValidSuccess);
        }

        private string IsValidKohi(bool isKohiModdel, bool isHokenMstModel, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenMstIsFutansyaNoCheck, int hokenMstIsJyukyusyaNoCheck, int hokenMstIsTokusyuNoCheck, int hokenMstModelStartDate, int hokenMstModelEndDate, string hokenMstDisplayText, int numberKohi, int sinDate, bool isAddNew)
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

            if (!isKohiModdel)
            {
                return message;
            }


            if (!isHokenMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return message;
            }
            if (string.IsNullOrEmpty(futansyaNo.Trim())
                && hokenMstIsFutansyaNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            else if (string.IsNullOrEmpty(jyukyusyaNo.Trim())
                && hokenMstIsJyukyusyaNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            else if (string.IsNullOrEmpty(tokusyuNo.Trim())
                && hokenMstIsTokusyuNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            if (!string.IsNullOrEmpty(futansyaNo) && Int32.Parse(futansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return message;
            }
            int KohiYukoFromDate = startDate;
            int KohiYukoToDate = endDate;
            if (KohiYukoFromDate != 0 && KohiYukoToDate != 0 && KohiYukoFromDate > KohiYukoToDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "有効終了日", "公費" + numberMessage + "有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return message;
            }

            var checkMessageIsValidConfirmDateKohi = IsValidConfirmDateKohi(confirmDate, numberMessage, sinDate, isAddNew);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateKohi))
            {
                return checkMessageIsValidConfirmDateKohi;
            }

            var checkMessageIsValidMasterDateKohi = IsValidMasterDateKohi(isHokenMstModel, hokenMstModelStartDate, hokenMstModelEndDate, hokenMstDisplayText, numberMessage, sinDate);
            if (!String.IsNullOrEmpty(checkMessageIsValidMasterDateKohi))
            {
                return checkMessageIsValidMasterDateKohi;
            }

            return message;
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

        private string IsValidMasterDateKohi(bool isHokenMstModel,int hokenMstStartDate, int hokenMstEndDate, string displayText, string numberMessage, int sinDate)
        {
            var message = "";
            if (!isHokenMstModel) return message;
            if (hokenMstStartDate > sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + displayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstStartDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                return message;
            }
            if (hokenMstEndDate < sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + displayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstEndDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                return message;
            }
            return message;
        }

        private string IsvalidKohiAll(KohiInfModel kohiModel1, KohiInfModel kohiModel2, KohiInfModel kohiModel3, KohiInfModel kohiModel4)
        {
            var message = "";
            if (kohiModel2 != null && kohiModel1 == null)
            {
                var paramsMessage = new string[] { "公費１" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }

            if (kohiModel3 != null)
            {
                if (kohiModel1 == null)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }

                if (kohiModel2 == null)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }
            }

            if (kohiModel4 != null)
            {
                if (kohiModel1 == null)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }

                if (kohiModel2 == null)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }

                if (kohiModel3 == null)
                {
                    var paramsMessage = new string[] { "公費３" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }
            }

            var checkMessageIsValidDuplicateKohi1 = IsValidDuplicateKohi1(kohiModel1, kohiModel2, kohiModel3, kohiModel4);
            if (String.IsNullOrEmpty(checkMessageIsValidDuplicateKohi1))
            {
                return checkMessageIsValidDuplicateKohi1;
            }
            var checkMessageIsValidDuplicateKohi2 = IsValidDuplicateKohi2(kohiModel1, kohiModel2, kohiModel3, kohiModel4);
            if (String.IsNullOrEmpty(checkMessageIsValidDuplicateKohi2))
            {
                return checkMessageIsValidDuplicateKohi2;
            }
            var checkMessageIsValidDuplicateKohi3 = IsValidDuplicateKohi3(kohiModel1, kohiModel2, kohiModel3, kohiModel4);
            if (String.IsNullOrEmpty(checkMessageIsValidDuplicateKohi3))
            {
                return checkMessageIsValidDuplicateKohi3;
            }
            var checkMessageIsValidDuplicateKohi4 = IsValidDuplicateKohi4(kohiModel1, kohiModel2, kohiModel3, kohiModel4);
            if (String.IsNullOrEmpty(checkMessageIsValidDuplicateKohi4))
            {
                return checkMessageIsValidDuplicateKohi1;
            }

            return message;
        }

        private string IsValidDuplicateKohi1(KohiInfModel? kohiModel1, KohiInfModel? kohiModel2, KohiInfModel? kohiModel3, KohiInfModel? kohiModel4)
        {
            var message = "";
            if (kohiModel1 != null && (kohiModel2 == kohiModel1
                    || kohiModel3 == kohiModel1
                    || kohiModel4 == kohiModel1))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
            }

            return message;
        }
        private string IsValidDuplicateKohi2(KohiInfModel? kohiModel1, KohiInfModel? kohiModel2, KohiInfModel? kohiModel3, KohiInfModel? kohiModel4)
        {
            var message = "";
            if (kohiModel2 != null && (kohiModel1 == kohiModel2
                    || kohiModel3 == kohiModel2
                    || kohiModel4 == kohiModel2))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
            }
            return message;
        }

        private string IsValidDuplicateKohi3(KohiInfModel? kohiModel1, KohiInfModel? kohiModel2, KohiInfModel? kohiModel3, KohiInfModel? kohiModel4)
        {
            var message = "";
            if (kohiModel3 != null && (kohiModel1 == kohiModel3
                     || kohiModel2 == kohiModel3
                     || kohiModel4 == kohiModel3))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
            }
            return message;
        }

        private string IsValidDuplicateKohi4(KohiInfModel? kohiModel1, KohiInfModel? kohiModel2, KohiInfModel? kohiModel3, KohiInfModel? kohiModel4)
        {
            var message = "";
            if (kohiModel4 != null && (kohiModel1 == kohiModel4
                   || kohiModel2 == kohiModel4
                   || kohiModel3 == kohiModel4))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
            }
            return message;
        }

        private string IsValidKohiNo_Fnc(bool isKohiModel, bool isKohiMstModel, int hokenNo, string futansyaNo, string tokusyuNo, int hokenMstIsJyukyusyaNoCheck, int hokenMstIsFutansyaNoCheck, int hokenMstJyukyuCheckDigit, int hokenMstCheckDigit, string hokenMstHoubetu, string jyukyusyaNo, int hokenMstAgeStartDate, int hokenMstAgeEndDate, int numberKohi, int ptBirthday)
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
                return message;
            }

            //公費１保険番号
            //公費１負担者番号入力なし
            if (!isKohiMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return message;
            }
            if (hokenNo != 0)
            {
                if (string.IsNullOrEmpty(futansyaNo)
                    && hokenMstIsJyukyusyaNoCheck == 1)
                {
                    var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }
                //法別番号のチェック
                if (hokenMstIsFutansyaNoCheck == 1)
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
                        return message;
                    }
                    //チェックデジット
                    if (hokenMstCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(futansyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        return message;
                    }
                    if (hokenMstIsJyukyusyaNoCheck == 1 && hokenMstJyukyuCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(jyukyusyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        return message;
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
                            return message;
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
                    return message;
                }
            }

            return message;
        }
    }
}
