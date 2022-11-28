using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Helper.Common;
using UseCase.Insurance.ValidateOneKohi;

namespace Interactor.Insurance
{
    public class ValidateOneKohiInteractor : IValidOneKohiInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        public ValidateOneKohiInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public ValidOneKohiOutputData Handle(ValidOneKohiInputData inputData)
        {
            var validateDetails = new List<ResultValidateInsurance<ValidOneKohiStatus>>();
            try
            {
                if (inputData.SinDate < 0)
                    validateDetails.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidSindate, string.Empty, TypeMessage.TypeMessageError));

                if (inputData.PtBirthday < 0)
                    validateDetails.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidPtBirthday, string.Empty, TypeMessage.TypeMessageError));

                // Get HokenMst Kohi1
                var hokenMstKohi = _patientInforRepository.GetHokenMstByInfor(inputData.SelectedKohiHokenNo, inputData.SelectedKohiHokenEdraNo);
                if (hokenMstKohi is null)
                    hokenMstKohi = new HokenMstModel();

                //IsValidKohi1
                IsValidKohi(ref validateDetails, inputData.IsKohiEmptyModel, inputData.IsSelectedKohiMst, inputData.SelectedKohiFutansyaNo, inputData.SelectedKohiJyukyusyaNo, inputData.SelectedKohiTokusyuNo, inputData.SelectedKohiStartDate, inputData.SelectedKohiEndDate, inputData.SelectedKohiConfirmDate, hokenMstKohi.IsFutansyaNoCheck, hokenMstKohi.IsJyukyusyaNoCheck, hokenMstKohi.IsTokusyuNoCheck, hokenMstKohi.StartDate, hokenMstKohi.EndDate, hokenMstKohi.DisplayTextMaster, 1, inputData.SinDate, inputData.SelectedKohiIsAddNew);

                // check Kohi No Function1
                IsValidKohiNo_Fnc(ref validateDetails, inputData.IsKohiEmptyModel, inputData.IsSelectedKohiMst, inputData.SelectedKohiHokenNo, inputData.SelectedKohiFutansyaNo, inputData.SelectedKohiTokusyuNo, hokenMstKohi.IsJyukyusyaNoCheck, hokenMstKohi.IsFutansyaNoCheck, hokenMstKohi.JyuKyuCheckDigit, hokenMstKohi.CheckDigit, hokenMstKohi.Houbetu, inputData.SelectedKohiJyukyusyaNo, hokenMstKohi.AgeStart, hokenMstKohi.AgeEnd, 1, inputData.PtBirthday);
            }
            catch(Exception ex)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFaild, ex.Message, TypeMessage.TypeMessageError));
            }
            return new ValidOneKohiOutputData(validateDetails);
        }

        private void IsValidKohi(ref List<ResultValidateInsurance<ValidOneKohiStatus>> result, bool isKohiModdel, bool isHokenMstModel, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenMstIsFutansyaCheckFlag, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsTokusyuCheckFlag, int hokenMstModelStartDate, int hokenMstModelEndDate, string hokenMstDisplayText, int numberKohi, int sinDate, bool isAddNew)
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
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel4, message, TypeMessage.TypeMessageError));
                }
            }

            if (!isHokenMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty4, message, TypeMessage.TypeMessageError));
                }
            }

            // check validate data
            CheckValidData(ref result, numberMessage, numberKohi, futansyaNo, hokenMstIsFutansyaCheckFlag, jyukyusyaNo, hokenMstIsJyukyusyaCheckFlag, tokusyuNo, hokenMstIsTokusyuCheckFlag);

            // check kohi date
            CheckKohiDate(ref result, startDate, endDate, numberMessage, numberKohi);

            // check confirm date kohi
            IsValidConfirmDateKohi(ref result, confirmDate, numberMessage, sinDate, isAddNew, numberKohi);

            // master date kohi IsValidMasterDateKohi
            CheckMasterDateKohi(ref result, hokenMstModelStartDate, hokenMstModelEndDate, sinDate, numberMessage, hokenMstDisplayText, numberKohi);
        }

        private void CheckValidData(ref List<ResultValidateInsurance<ValidOneKohiStatus>> result, string numberMessage, int numberKohi, string futansyaNo, int hokenMstIsFutansyaCheckFlag, string jyukyusyaNo, int hokenMstIsJyukyusyaCheckFlag, string tokusyuNo, int hokenMstIsTokusyuCheckFlag)
        {
            var message = "";
            if (string.IsNullOrEmpty(futansyaNo.Trim())
                && hokenMstIsFutansyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty4, message, TypeMessage.TypeMessageError));
                }
            }
            else if (string.IsNullOrEmpty(jyukyusyaNo.Trim())
                && hokenMstIsJyukyusyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidJyukyusyaNo1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidJyukyusyaNo2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidJyukyusyaNo3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidJyukyusyaNo4, message, TypeMessage.TypeMessageError));
                }
            }
            else if (string.IsNullOrEmpty(tokusyuNo.Trim())
                && hokenMstIsTokusyuCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidTokusyuNo1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidTokusyuNo2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidTokusyuNo3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidTokusyuNo4, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(futansyaNo) && Int32.Parse(futansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNo01, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNo02, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNo03, message, TypeMessage.TypeMessageWarning));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNo04, message, TypeMessage.TypeMessageWarning));
                }
            }
        }

        private void CheckKohiDate(ref List<ResultValidateInsurance<ValidOneKohiStatus>> result,int startDate, int endDate, string numberMessage, int numberKohi)
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
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiYukoDate1, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiYukoDate2, message, TypeMessage.TypeMessageWarning));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiYukoDate3, message, TypeMessage.TypeMessageWarning));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiYukoDate4, message, TypeMessage.TypeMessageWarning));
                }
            }
        }

        private void IsValidConfirmDateKohi(ref List<ResultValidateInsurance<ValidOneKohiStatus>> result, int confirmDate, string numberMessage, int sinDate, bool isAddNew, int numberKohi)
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
            }
            if (!String.IsNullOrEmpty(message))
            {
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiConfirmDate1, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiConfirmDate2, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiConfirmDate3, message, TypeMessage.TypeMessageConfirmation));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiConfirmDate4, message, TypeMessage.TypeMessageConfirmation));
                }
            }
        }


        private void CheckMasterDateKohi(ref List<ResultValidateInsurance<ValidOneKohiStatus>> result, int hokenMstModelStartDate, int hokenMstModelEndDate, int sinDate, string numberMessage, string hokenMstDisplayText, int numberKohi)
        {
            var message = "";
            if (hokenMstModelStartDate > sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelStartDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstStartDate1, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstStartDate2, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstStartDate3, message, TypeMessage.TypeMessageConfirmation));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstStartDate4, message, TypeMessage.TypeMessageConfirmation));
                }
            }
            if (hokenMstModelEndDate < sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelEndDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEndDate1, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEndDate2, message, TypeMessage.TypeMessageConfirmation));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEndDate3, message, TypeMessage.TypeMessageConfirmation));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEndDate4, message, TypeMessage.TypeMessageConfirmation));
                }
            }
        }

        private void IsValidKohiNo_Fnc(ref List<ResultValidateInsurance<ValidOneKohiStatus>> result, bool isKohiModel, bool isKohiMstModel, int hokenNo, string futansyaNo, string tokusyuNo, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsFutansyaCheckFlag, int hokenMstJyukyuCheckDigit, int hokenMstCheckDigit, string hokenMstHoubetu, string jyukyusyaNo, int hokenMstAgeStartDate, int hokenMstAgeEndDate, int numberKohi, int ptBirthday)
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
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiEmptyModel4, message, TypeMessage.TypeMessageError));
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
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty1, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 2)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty2, message, TypeMessage.TypeMessageError));
                }
                else if (numberKohi == 3)
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty3, message, TypeMessage.TypeMessageError));
                }
                else
                {
                    result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidKohiHokenMstEmpty4, message, TypeMessage.TypeMessageError));
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
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty1, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 2)
                    {
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty2, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 3)
                    {
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty3, message, TypeMessage.TypeMessageError));
                    }
                    else
                    {
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutansyaNoEmpty4, message, TypeMessage.TypeMessageError));
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
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckHBT1, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 2)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckHBT2, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 3)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckHBT3, message, TypeMessage.TypeMessageError));
                        }
                        else
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckHBT4, message, TypeMessage.TypeMessageError));
                        }
                    }
                    //チェックデジット
                    if (hokenMstCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(futansyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo1, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 2)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo2, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 3)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo3, message, TypeMessage.TypeMessageError));
                        }
                        else
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitFutansyaNo4, message, TypeMessage.TypeMessageError));
                        }
                    }
                    if (hokenMstIsJyukyusyaCheckFlag == 1 && hokenMstJyukyuCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(jyukyusyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo1, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 2)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo2, message, TypeMessage.TypeMessageError));
                        }
                        else if (numberKohi == 3)
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo3, message, TypeMessage.TypeMessageError));
                        }
                        else
                        {
                            result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckDigitJyukyusyaNo4, message, TypeMessage.TypeMessageError));
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
                                result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckAge1, message, TypeMessage.TypeMessageError));
                            }
                            else if (numberKohi == 2)
                            {
                                result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckAge2, message, TypeMessage.TypeMessageError));
                            }
                            else if (numberKohi == 3)
                            {
                                result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckAge3, message, TypeMessage.TypeMessageError));
                            }
                            else
                            {
                                result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidMstCheckAge4, message, TypeMessage.TypeMessageError));
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
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutanJyoTokuNull1, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 2)
                    {
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutanJyoTokuNull2, message, TypeMessage.TypeMessageError));
                    }
                    else if (numberKohi == 3)
                    {
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutanJyoTokuNull3, message, TypeMessage.TypeMessageError));
                    }
                    else
                    {
                        result.Add(new ResultValidateInsurance<ValidOneKohiStatus>(ValidOneKohiStatus.InvalidFutanJyoTokuNull4, message, TypeMessage.TypeMessageError));
                    }
                }
            }
        }
    }
}
