using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Helper.Common;
using UseCase.Insurance.ValidKohi;

namespace Interactor.Insurance
{
    public class ValidateKohiInteractor : IValidKohiInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public ValidateKohiInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public ValidKohiOutputData Handle(ValidKohiInputData inputData)
        {
            var validateDetails = new List<ResultValidateInsurance<ValidKohiStatus>>();
            try
            {
                if (inputData.SinDate < 0)
                    validateDetails.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidSindate, string.Empty, TypeMessage.TypeMessageError));

                if (inputData.PtBirthday < 0)
                    validateDetails.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidPtBirthday, string.Empty, TypeMessage.TypeMessageError));

                // Get HokenMst Kohi1
                var hokenMstKohi = _patientInforRepository.GetHokenMstByInfor(inputData.SelectedKohiHokenNo, inputData.SelectedKohiHokenEdraNo);
                if (hokenMstKohi is null)
                    hokenMstKohi = new HokenMstModel();

                //IsValidKohi1
                IsValidKohi(ref validateDetails, inputData.IsKohiEmptyModel, inputData.IsSelectedKohiMst, inputData.SelectedKohiFutansyaNo, inputData.SelectedKohiJyukyusyaNo, inputData.SelectedKohiTokusyuNo, inputData.SelectedKohiStartDate, inputData.SelectedKohiEndDate, inputData.SelectedKohiConfirmDate, hokenMstKohi.IsFutansyaNoCheck, hokenMstKohi.IsJyukyusyaNoCheck, hokenMstKohi.IsTokusyuNoCheck, hokenMstKohi.StartDate, hokenMstKohi.EndDate, hokenMstKohi.DisplayTextMaster, 1, inputData.SinDate, inputData.SelectedKohiIsAddNew, inputData.SelectedHokenPatternIsExpirated);

                // check Kohi No Function1
                IsValidKohiNo_Fnc(ref validateDetails, inputData.IsKohiEmptyModel, inputData.IsSelectedKohiMst, inputData.SelectedKohiHokenNo, inputData.SelectedKohiFutansyaNo, inputData.SelectedKohiTokusyuNo, hokenMstKohi.IsJyukyusyaNoCheck, hokenMstKohi.IsFutansyaNoCheck, hokenMstKohi.JyuKyuCheckDigit, hokenMstKohi.CheckDigit, hokenMstKohi.Houbetu, inputData.SelectedKohiJyukyusyaNo, hokenMstKohi.AgeStart, hokenMstKohi.AgeEnd, 1, inputData.PtBirthday);
            }
            catch (Exception ex)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFaild, ex.Message, TypeMessage.TypeMessageError));
            }
            return new ValidKohiOutputData(validateDetails);
        }

        private void IsValidKohi(ref List<ResultValidateInsurance<ValidKohiStatus>> result, bool isKohiModdel, bool isHokenMstModel, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenMstIsFutansyaCheckFlag, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsTokusyuCheckFlag, int hokenMstModelStartDate, int hokenMstModelEndDate, string hokenMstDisplayText, int numberKohi, int sinDate, bool isAddNew,bool selectedHokenPatternIsExpirated)
        {
            var message = "";
            var numberMessage = "";

            if (numberKohi == 1)
                numberMessage = "１";
            else if (numberKohi == 2)
                numberMessage = "２";
            else if (numberKohi == 3)
                numberMessage = "３";
            else
                numberMessage = "４";

            if (isKohiModdel)
            {
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel4, message, TypeMessage.TypeMessageError));
                }
            }

            if (!isHokenMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty4, message, TypeMessage.TypeMessageError));
                }
            }

            // check validate data
            CheckValidData(ref result, numberMessage, numberKohi, futansyaNo, hokenMstIsFutansyaCheckFlag, jyukyusyaNo, hokenMstIsJyukyusyaCheckFlag, tokusyuNo, hokenMstIsTokusyuCheckFlag);

            // check kohi date
            CheckKohiDate(ref result, startDate, endDate, numberMessage, numberKohi);

            // check confirm date kohi
            IsValidConfirmDateKohi(ref result, confirmDate, numberMessage, sinDate, isAddNew, numberKohi, selectedHokenPatternIsExpirated);

            // master date kohi IsValidMasterDateKohi
            CheckMasterDateKohi(ref result, hokenMstModelStartDate, hokenMstModelEndDate, sinDate, numberMessage, hokenMstDisplayText, numberKohi);
        }

        private void CheckValidData(ref List<ResultValidateInsurance<ValidKohiStatus>> result, string numberMessage, int numberKohi, string futansyaNo, int hokenMstIsFutansyaCheckFlag, string jyukyusyaNo, int hokenMstIsJyukyusyaCheckFlag, string tokusyuNo, int hokenMstIsTokusyuCheckFlag)
        {
            var message = "";
            if (string.IsNullOrEmpty(futansyaNo.Trim())
                && hokenMstIsFutansyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty4, message, TypeMessage.TypeMessageError));
                }
            }
            else if (string.IsNullOrEmpty(jyukyusyaNo.Trim())
                && hokenMstIsJyukyusyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidJyukyusyaNo1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidJyukyusyaNo2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidJyukyusyaNo3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidJyukyusyaNo4, message, TypeMessage.TypeMessageError));
                }
            }
            else if (string.IsNullOrEmpty(tokusyuNo.Trim())
                && hokenMstIsTokusyuCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidTokusyuNo1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidTokusyuNo2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidTokusyuNo3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidTokusyuNo4, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(futansyaNo) && Int32.Parse(futansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNo01, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNo02, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNo03, message, TypeMessage.TypeMessageWarning));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNo04, message, TypeMessage.TypeMessageWarning));
                }
            }
        }

        private void CheckKohiDate(ref List<ResultValidateInsurance<ValidKohiStatus>> result, int startDate, int endDate, string numberMessage, int numberKohi)
        {
            var message = "";
            int kohiYukoFromDate = startDate;
            int kohiYukoToDate = endDate;
            if (kohiYukoFromDate != 0 && kohiYukoToDate != 0 && kohiYukoFromDate > kohiYukoToDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "有効終了日", "公費" + numberMessage + "有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiYukoDate1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiYukoDate2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiYukoDate3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiYukoDate4, message, TypeMessage.TypeMessageError));
                }
            }
        }

        private void IsValidConfirmDateKohi(ref List<ResultValidateInsurance<ValidKohiStatus>> result, int confirmDate, string numberMessage, int sinDate, bool isAddNew, int numberKohi, bool selectedHokenPatternIsExpirated)
        {
            var message = "";
            int kouhi1ConfirmDate = confirmDate;
            int confirmKohi1YM = Int32.Parse(CIUtil.Copy(kouhi1ConfirmDate.ToString(), 1, 6));
            if (kouhi1ConfirmDate == 0
                || sinDate != confirmKohi1YM)
            {
                // 公１・保険証確認日ﾁｪｯｸ(有効保険・新規保険の場合のみ)
                if (!isAddNew)
                {
                    var paramsMessage = new string[] { "公費" + numberMessage, "受給者証等" };
                    message = String.Format(ErrorMessage.MessageType_mChk00030, paramsMessage);
                }
                else
                {
                    if(!selectedHokenPatternIsExpirated)
                    {
                        var paramsMessage = new string[] { "公費１", "受給者証等" };
                        message = String.Format(ErrorMessage.MessageType_mChk00030, paramsMessage);
                    }
                }
            }
            if (!String.IsNullOrEmpty(message))
            {
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiConfirmDate1, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiConfirmDate2, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiConfirmDate3, message, TypeMessage.TypeMessageConfirmation));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiConfirmDate4, message, TypeMessage.TypeMessageConfirmation));
                }
            }
        }


        private void CheckMasterDateKohi(ref List<ResultValidateInsurance<ValidKohiStatus>> result, int hokenMstModelStartDate, int hokenMstModelEndDate, int sinDate, string numberMessage, string hokenMstDisplayText, int numberKohi)
        {
            var message = "";
            if (hokenMstModelStartDate > sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelStartDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstStartDate1, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstStartDate2, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstStartDate3, message, TypeMessage.TypeMessageWarning));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstStartDate4, message, TypeMessage.TypeMessageWarning));
                }
            }
            if (hokenMstModelEndDate < sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelEndDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEndDate1, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEndDate2, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEndDate3, message, TypeMessage.TypeMessageWarning));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEndDate4, message, TypeMessage.TypeMessageWarning));
                }
            }
        }

        private void IsValidKohiNo_Fnc(ref List<ResultValidateInsurance<ValidKohiStatus>> result, bool isKohiModel, bool isKohiMstModel, int hokenNo, string futansyaNo, string tokusyuNo, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsFutansyaCheckFlag, int hokenMstJyukyuCheckDigit, int hokenMstCheckDigit, string hokenMstHoubetu, string jyukyusyaNo, int hokenMstAgeStartDate, int hokenMstAgeEndDate, int numberKohi, int ptBirthday)
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
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiEmptyModel4, message, TypeMessage.TypeMessageError));
                }
            }

            //公費１保険番号
            //公費１負担者番号入力なし
            if (!isKohiMstModel && !isKohiModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidKohiHokenMstEmpty4, message, TypeMessage.TypeMessageError));
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
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty1, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 2)
                    {
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty2, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 3)
                    {
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty3, message, TypeMessage.TypeMessageError));
                    }
                    else
                    {
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutansyaNoEmpty4, message, TypeMessage.TypeMessageError));
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
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckHBT1, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 2)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckHBT2, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 3)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckHBT3, message, TypeMessage.TypeMessageError));
                        }
                        else
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckHBT4, message, TypeMessage.TypeMessageError));
                        }
                    }
                    //チェックデジット
                    if (hokenMstCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(futansyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitFutansyaNo1, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 2)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitFutansyaNo2, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 3)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitFutansyaNo3, message, TypeMessage.TypeMessageError));
                        }
                        else
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitFutansyaNo4, message, TypeMessage.TypeMessageError));
                        }
                    }
                    if (hokenMstIsJyukyusyaCheckFlag == 1 && hokenMstJyukyuCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(jyukyusyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo1, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 2)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo2, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 3)
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo3, message, TypeMessage.TypeMessageError));
                        }
                        else
                        {
                            result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckDigitJyukyusyaNo4, message, TypeMessage.TypeMessageError));
                        }
                    }

                    //年齢チェック
                    int intAge = -1;
                    if (ptBirthday != 0)
                    {
                        intAge = CIUtil.SDateToAge(ptBirthday, Int32.Parse(DateTime.Now.ToString("yyyyMMdd")));
                    }
                    if (intAge != -1)
                    {
                        int ageStartMst = hokenMstAgeStartDate;
                        int ageEndMst = hokenMstAgeEndDate;
                        if ((ageStartMst != 0 || ageEndMst != 999) && (intAge < ageStartMst || intAge > ageEndMst))
                        {
                            var paramsMessage = new string[] { "公費" + numberMessage + "の保険が適用年齢範囲外の", "・この保険の適用年齢範囲は。" + ageStartMst + "歳 〜 " + ageEndMst + "歳 です。" };
                            message = String.Format(ErrorMessage.MessageType_mEnt01041, paramsMessage);
                            if (numberKohi == 1)
                            {
                                result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckAge1, message, TypeMessage.TypeMessageError));
                            }
                            else if (numberKohi == 2)
                            {
                                result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckAge2, message, TypeMessage.TypeMessageError));
                            }
                            else if (numberKohi == 3)
                            {
                                result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckAge3, message, TypeMessage.TypeMessageError));
                            }
                            else
                            {
                                result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidMstCheckAge4, message, TypeMessage.TypeMessageError));
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
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutanJyoTokuNull1, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 2)
                    {
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutanJyoTokuNull2, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 3)
                    {
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutanJyoTokuNull3, message, TypeMessage.TypeMessageError));
                    }
                    else
                    {
                        result.Add(new ResultValidateInsurance<ValidKohiStatus>(ValidKohiStatus.InvalidFutanJyoTokuNull4, message, TypeMessage.TypeMessageError));
                    }
                }
            }
        }
    }
}
