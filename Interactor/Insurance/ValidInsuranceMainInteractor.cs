using Domain.Constant;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Domain.Models.User;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidMainInsurance;

namespace Interactor.Insurance
{
    public class ValidInsuranceMainInteractor : IValidMainInsuranceInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        public ValidInsuranceMainInteractor(IPatientInforRepository patientInforRepository, ISystemConfRepository systemConfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _systemConfRepository = systemConfRepository;
        }
        public ValidMainInsuranceOutputData Handle(ValidMainInsuranceInputData inputData)
        {
            try
            {
                // check validate Input
                var checkValidInput = CheckValidateInputData(inputData);
                if(!checkValidInput.Result)
                {
                    return checkValidInput;
                }   
                
                switch (inputData.HokenKbn)
                {
                    // 自費
                    case 0:

                        var checkMessageIsValidJihi = IsValidJihi(inputData.SelectedHokenInfHokenNo);
                        if (!String.IsNullOrEmpty(checkMessageIsValidJihi))
                        {
                            return new ValidMainInsuranceOutputData(false, checkMessageIsValidJihi, ValidMainInsuranceStatus.InvalidFaild);
                        }
                        // ignore
                        break;
                    // 社保
                    case 1:
                        var checkIsValidShaho = IsValidShaho(inputData.IsSelectedHokenPattern, inputData.SelectedHokenInfIsAddNew, inputData.SelectedHokenPatternIsEmptyHoken,
                                                inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4, inputData.IsSelectedHokenInf, inputData.HokenKbn,
                                                inputData.SelectedHokenInfHoubetu, inputData.IsSelectedHokenInf, inputData.SelectedHokenInfIsAddNew, inputData.HpId, inputData.SinDate,
                                                inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5,
                                                inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfIsJihi, inputData.HokenSyaNo, inputData.SelectedHokenInfHokenNo, inputData.IsSelectedHokenMst, inputData.SelectedHokenMstHoubetu, inputData.SelectedHokenMstHokenNo, inputData.SelectedHokenMstCheckDegit, inputData.PtBirthday,
                                                inputData.SelectedHokenMstAgeStart, inputData.SelectedHokenMstAgeEnd, inputData.SelectedHokenInfKigo, inputData.SelectedHokenInfBango, inputData.SelectedHokenInfHokensyaMstIsKigoNa, inputData.SelectedHokenInfHonkeKbn, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate,
                                                inputData.SelectedHokenInfTokureiYm1, inputData.SelectedHokenInfTokureiYm2, inputData.SelectedHokenInfIsShahoOrKokuho, inputData.SelectedHokenInfIsExpirated, inputData.SelectedHokenInfConfirmDate, inputData.SelectedHokenMstStartDate, inputData.SelectedHokenMstEndDate, inputData.SelectedHokenMstDisplayText);
                        if (!checkIsValidShaho.Result)
                        {
                            return checkIsValidShaho;
                        }
                        break;
                    // 国保
                    case 2:
                        var checkIsValidKokuho = IsValidShaho(inputData.IsSelectedHokenPattern, inputData.SelectedHokenInfIsAddNew, inputData.SelectedHokenPatternIsEmptyHoken,
                                                 inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4, inputData.IsSelectedHokenInf, inputData.HokenKbn,
                                                 inputData.SelectedHokenInfHoubetu, inputData.IsSelectedHokenInf, inputData.SelectedHokenInfIsAddNew, inputData.HpId, inputData.SinDate,
                                                 inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5,
                                                 inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfIsJihi, inputData.HokenSyaNo, inputData.SelectedHokenInfHokenNo, inputData.IsSelectedHokenMst, inputData.SelectedHokenMstHoubetu, inputData.SelectedHokenMstHokenNo, inputData.SelectedHokenMstCheckDegit, inputData.PtBirthday,
                                                 inputData.SelectedHokenMstAgeStart, inputData.SelectedHokenMstAgeEnd, inputData.SelectedHokenInfKigo, inputData.SelectedHokenInfBango, inputData.SelectedHokenInfHokensyaMstIsKigoNa, inputData.SelectedHokenInfHonkeKbn, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate,
                                                 inputData.SelectedHokenInfTokureiYm1, inputData.SelectedHokenInfTokureiYm2, inputData.SelectedHokenInfIsShahoOrKokuho, inputData.SelectedHokenInfIsExpirated, inputData.SelectedHokenInfConfirmDate, inputData.SelectedHokenMstStartDate, inputData.SelectedHokenMstEndDate, inputData.SelectedHokenMstDisplayText);
                        if (!checkIsValidKokuho.Result)
                        {
                            return checkIsValidKokuho;
                        }
                        break;
                }
                return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
            }
            catch (Exception)
            {

                return new ValidMainInsuranceOutputData(false, "Validate Exception", ValidMainInsuranceStatus.InvalidFaild);
            }
        }

        private ValidMainInsuranceOutputData CheckValidateInputData(ValidMainInsuranceInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidHpId);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSinDate);
            }

            if (inputData.PtBirthday < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidPtBirthday);
            }

            if (inputData.HokenKbn < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn);
            }

            if (inputData.HokenKbn < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn);
            }

            if (inputData.SelectedHokenInfHokenNo < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenNo);
            }

            if (inputData.SelectedHokenInfStartDate < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfStartDate);
            }

            if (inputData.SelectedHokenInfEndDate < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfEndDate);
            }

            if (inputData.SelectedHokenInfHokensyaMstIsKigoNa < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvaliSelectedHokenInfHokensyaMstIsKigoNa);
            }

            if (inputData.SelectedHokenInfHonkeKbn < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn);
            }

            if (inputData.SelectedHokenInfTokureiYm1 < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm1);
            }

            if (inputData.SelectedHokenInfTokureiYm2 < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm2);
            }

            if (inputData.SelectedHokenInfConfirmDate < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenInfConfirmDate);
            }

            if (inputData.SelectedHokenMstHokenNo < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenMstHokenNo);
            }

            if (inputData.SelectedHokenMstCheckDegit < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenMstCheckDegit);
            }

            if (inputData.SelectedHokenMstAgeStart < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenMstAgeStart);
            }

            if (inputData.SelectedHokenMstAgeEnd < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenMstAgeEnd);
            }

            if (inputData.SelectedHokenMstStartDate < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenMstStartDate);
            }

            if (inputData.SelectedHokenMstEndDate < 0)
            {
                return new ValidMainInsuranceOutputData(false, string.Empty, ValidMainInsuranceStatus.InvalidSelectedHokenMstEndDate);
            }

            return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
        }

        private string IsValidJihi(int selectedHokenInfHokenNo)
        {
            var message = "";
            if (selectedHokenInfHokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
            }
            return message;
        }

        private ValidMainInsuranceOutputData IsValidShaho(bool isSelectedHokenPattern, bool isAddNew, bool isEmptyHoken, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4, bool isSelectedHokenInf, int hokenKbn, string selectedHokenInfHoubetu, bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText)
        {
            // Validate empty hoken
            var checkMessageIsValidEmptyHoken = IsValidEmptyHoken(isSelectedHokenPattern, isAddNew, isEmptyHoken, isEmptyKohi1, isEmptyKohi2, isEmptyKohi3, isEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidEmptyHoken))
            {
                return new ValidMainInsuranceOutputData(false, checkMessageIsValidEmptyHoken, ValidMainInsuranceStatus.InvalidEmptyHoken);
            }
            // Validate HokenNashi only
            var checkMessageIsValidHokenNashiOnly = IsValidHokenNashiOnly(isSelectedHokenPattern, isSelectedHokenInf, hokenKbn, selectedHokenInfHoubetu, isEmptyKohi1, isEmptyKohi2, isEmptyKohi3, isEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenNashiOnly))
            {
                return new ValidMainInsuranceOutputData(false, checkMessageIsValidHokenNashiOnly, ValidMainInsuranceStatus.InvalidHokenNashiOnly);
            }
            // Valiate HokenInf
            var checkIsValidHokenInf = IsValidHokenInf(selectedHokenInf, selectedHokenInfIsAddNew, hokenKbn, selectedHokenInfHoubetu, hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5, selectedHokenInfStartDate, selectedHokenInfEndDate, selectedHokenInfIsJihi, hokenSyaNo, hokenNo, isHaveSelectedHokenMst, selectedHokenInfHoubetu, sHokenMstHoubetsuNumber, sHokenMstHokenNumber, sHokenMstCheckDegit, ptBirthday, sHokenMstAgeStart, sHokenMstAgeEnd, kigo, bango, hokenMstIsKigoNa, honkeKbn, startDate, endDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated, selectedHokenInfconfirmDate, selectedHokenMstStartDate, selectedHokenMstEndDate, selectedHokenMstDisplayText);
            if (!checkIsValidHokenInf.Result)
            {
                return checkIsValidHokenInf;
            }
            return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
        }

        private string IsValidEmptyHoken(bool isSelectedHokenPattern, bool isAddNew, bool isEmptyHoken, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4)
        {
            var message = "";
            if (isSelectedHokenPattern && (!isAddNew
                    && (isEmptyHoken
                        && isEmptyKohi1
                        && isEmptyKohi2
                        && isEmptyKohi3
                        && isEmptyKohi4)))
            {
                var paramsMessage = new string[] { "保険組合せ", "情報" };
                message = String.Format(ErrorMessage.MessageType_mInp00011, paramsMessage);
            }
            return message;
        }

        public string IsValidHokenNashiOnly(bool isSelectedHokenPattern, bool isSelectedHokenInf, int hokenKbn, string selectedHokenInfHoubetu, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4)
        {
            var message = "";
            if (isSelectedHokenPattern && isSelectedHokenInf && hokenKbn == 1 && selectedHokenInfHoubetu == HokenConstant.HOUBETU_NASHI && (isEmptyKohi1 && isEmptyKohi2 && isEmptyKohi3 && isEmptyKohi4))
            {
                var paramsMessage = new string[] { "保険組合せ" };
                message = String.Format(ErrorMessage.MessageType_mSel01010, paramsMessage);
            }
            return message;
        }

        private ValidMainInsuranceOutputData IsValidHokenInf(bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hokenKbn, string selectedHokenInfHoubetu, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText)
        {
            var message = "";
            if(!selectedHokenInf)
            {
                return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
            }
            // Validate not HokenInf
            if (hokenKbn == 1 && selectedHokenInfHoubetu == HokenConstant.HOUBETU_NASHI)
            {
                return IsValidHokenNashi(hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5, selectedHokenInfStartDate, selectedHokenInfEndDate);
            }
            // Validate Jihi
            if (selectedHokenInfIsJihi)
            {
                return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
            }
            var checkIsValidHokenDetail = IsValidHokenDetail(hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5);
            if (!checkIsValidHokenDetail.Result)
            {
                return checkIsValidHokenDetail;
            }
            var checkCHKHokno_Fnc = CHKHokno_Fnc(hokenSyaNo, hokenNo, isHaveSelectedHokenMst, houbetu, sHokenMstHoubetsuNumber, sHokenMstHokenNumber, sHokenMstCheckDegit, ptBirthday, sHokenMstAgeStart, sHokenMstAgeEnd);
            if (!checkCHKHokno_Fnc.Result)
            {
                return checkCHKHokno_Fnc;
            }
            if (string.IsNullOrEmpty(hokenSyaNo))
            {
                var paramsMessage = new string[] { "保険者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokensyaNoNull);
            }
            if (hokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenNoEquals0);
            }
            if (Int32.Parse(hokenSyaNo) == 0)
            {
                var paramsMessage = new string[] { "保険者番号は 0 〜 9 の範囲で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokensyaNoEquals0);
            }
            // 記号
            if (hokenSyaNo.Length == 8 && hokenSyaNo.Trim().StartsWith("39"))
            {
                if (!string.IsNullOrEmpty(kigo)
                    && !string.IsNullOrEmpty(kigo.Trim(' '))) //Trim only half-size space
                {
                    var paramsMessage = new string[] { "後期高齢者の", "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00150, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokensyaNoLength8StartWith39);
                }
            }
            else
            {
                if (hokenMstIsKigoNa == 0 && (string.IsNullOrEmpty(kigo)
                    || string.IsNullOrEmpty(kigo.Trim(' '))))
                {
                    var paramsMessage = new string[] { "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidKigoNull);
                }
            }
            if (string.IsNullOrEmpty(bango)
                    || string.IsNullOrEmpty(bango.Trim(' '))) //Trim only half-size space
            {
                var paramsMessage = new string[] { "被保険者証番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidBangoNull);
            }
            if (honkeKbn == 0)
            {
                var paramsMessage = new string[] { "本人家族区分" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenKbnEquals0);
            }
            var checkIsValidYukoKigen = IsValidYukoKigen(startDate, endDate);
            if (!checkIsValidYukoKigen.Result)
            {
                return checkIsValidYukoKigen;
            }
            var checkIsValidTokkurei = IsValidTokkurei(ptBirthday, sinDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, hokenSyaNo, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated);
            if (!checkIsValidTokkurei.Result)
            {
                return checkIsValidTokkurei;
            }
            var checkMessageIsValidConfirmDateAgeCheck = IsValidConfirmDateAgeCheck(selectedHokenInfIsAddNew, selectedHokenInfisExpirated, selectedHokenInfisShahoOrKokuho, hokenSyaNo, selectedHokenInfconfirmDate, ptBirthday, sinDate, hpId);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateAgeCheck))
            {
                return new ValidMainInsuranceOutputData(false, checkMessageIsValidConfirmDateAgeCheck, ValidMainInsuranceStatus.InvalidConfirmDateAgeCheck);
            }
            // check valid hokenmst date
            var checkIsValidHokenMstDate = IsValidHokenMstDate(selectedHokenMstStartDate, selectedHokenMstEndDate, sinDate, selectedHokenMstDisplayText);
            if (!checkIsValidHokenMstDate.Result)
            {
                return checkIsValidHokenMstDate;
            }
            return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
        }

        private ValidMainInsuranceOutputData IsValidHokenNashi(int hpId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
        {
            var checkIsValidHokenDetail = IsValidHokenDetail(hpId, sinDate, tokki1, tokki2, tokki3, tokki4, tokki5);
            if (!checkIsValidHokenDetail.Result)
            {
                return checkIsValidHokenDetail;
            }
            var checkIsValidYukoKigen = IsValidYukoKigen(startDate, endDate);
            if (!checkIsValidYukoKigen.Result)
            {
                return checkIsValidYukoKigen;
            }
            return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
        }

        private ValidMainInsuranceOutputData IsValidYukoKigen(int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            int yukoFromDate = selectedHokenInfStartDate;
            int yukoToDate = selectedHokenInfEndDate;
            if (yukoFromDate != 0 && yukoToDate != 0 && yukoFromDate > yukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
            }
            return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidYukoKigen);
        }

        private ValidMainInsuranceOutputData IsValidHokenDetail(int hpId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
        {
            var tokkiMstBinding = _patientInforRepository.GetListTokki(hpId, sinDate);
            var message = "";
            bool _isValidLengthTokki(string tokkiValue)
            {
                var itemToki = tokkiMstBinding.FirstOrDefault(x => x.TokkiCd == tokkiValue);
                if (itemToki != null)
                {
                    return true;
                }
                return tokkiValue.Length <= 2;
            }
            if (!string.IsNullOrEmpty(tokki1Value))
            {
                if (!_isValidLengthTokki(tokki1Value))
                {
                    var paramsMessage = new string[] { "特記事項１", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue1);
                }
                if (!string.IsNullOrEmpty(tokki2Value) && tokki2Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki2Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue21);
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue31);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue41);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue51);
                }
            }
            if (!string.IsNullOrEmpty(tokki2Value))
            {
                if (!_isValidLengthTokki(tokki2Value))
                {
                    var paramsMessage = new string[] { "特記事項２", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue2);
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue23);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue24);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue25);
                }
            }
            if (!string.IsNullOrEmpty(tokki3Value))
            {
                if (!_isValidLengthTokki(tokki3Value))
                {
                    var paramsMessage = new string[] { "特記事項３", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue3);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue34);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue35);
                }
            }
            if (!string.IsNullOrEmpty(tokki4Value))
            {
                if (!_isValidLengthTokki(tokki4Value))
                {
                    var paramsMessage = new string[] { "特記事項４", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue4);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki4Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue45);
                }
            }
            if (!string.IsNullOrEmpty(tokki5Value) && !_isValidLengthTokki(tokki5Value))
            {
                var paramsMessage = new string[] { "特記事項５", "2文字" };
                message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkiValue5);
            }
            return new ValidMainInsuranceOutputData(true, message, ValidMainInsuranceStatus.InvalidTokkiValue1);
        }

        private ValidMainInsuranceOutputData CHKHokno_Fnc(string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd)
        {
            var message = "";
            //保険番号
            //保険者番号入力なし
            if (string.IsNullOrEmpty(hokenSyaNo)
            || string.IsNullOrEmpty(hokenSyaNo.Trim()))
            {
                if (hokenNo != 0)
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0);
                }
            }
            //保険番号入力あり
            else
            {
                if (hokenNo == 0)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenNoEquals0);
                }
                if (!isHaveSelectedHokenMst)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenNoHaveHokenMst);
                }
                //法別番号のチェック
                string hokenInfHoubetu = houbetu;
                //法別番号に一致する保険番号を初期値にセット
                if (!string.IsNullOrEmpty(hokenInfHoubetu) && hokenInfHoubetu != "0" && (hokenInfHoubetu != sHokenMstHoubetsuNumber
                        && sHokenMstHokenNumber != 68))
                {
                    // Hoken master filterred by HokenInf's houbetu => never go this case
                    //その法別のレコードがあるか　あればセット
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHoubetu);
                }
                //チェックデジット
                if (sHokenMstCheckDegit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(hokenSyaNo)))
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidCheckDigitEquals1);
                }
                //生年月日から年齢を取得
                int intAGE = -1;
                if (ptBirthday != 0)
                {
                    intAGE = CIUtil.SDateToAge(ptBirthday, Int32.Parse(DateTime.Now.ToString("yyyyMMdd")));
                }
                if (intAGE != -1)
                {
                    int ageStartMst = sHokenMstAgeStart;
                    int ageEndMst = sHokenMstAgeEnd;
                    if ((ageStartMst != 0 || ageEndMst != 999) && (intAGE < ageStartMst || intAGE > ageEndMst))
                    {
                        var paramsMessage = new string[] { "主保険の保険が適用年齢範囲外の", "・この保険の適用年齢範囲は。" + ageStartMst + "歳 〜 " + ageEndMst + "歳 です。" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidCheckAgeHokenMst);
                    }
                }
            }
            return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
        }

        private ValidMainInsuranceOutputData IsValidTokkurei(int ptBirthday, int sinDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, string hokenSyaNo, bool isShahoOrKokuho, bool isExpirated)
        {
            var message = "";
            int iYear = 0, iMonth = 0, iDay = 0;
            CIUtil.SDateToDecodeAge(ptBirthday, sinDate, ref iYear, ref iMonth, ref iDay);
            if (((iYear == 74 && iMonth == 11) || (iYear == 75 && iMonth == 0))
                && CIUtil.Copy(ptBirthday.ToString(), 5, 2) == CIUtil.Copy(sinDate.ToString(), 5, 2)
                && CIUtil.Copy(ptBirthday.ToString(), 7, 2) != "01"
                && CIUtil.Copy(ptBirthday.ToString(), 5, 4) != "0229"
                && (selectedHokenInfTokureiYm1 != sinDate)
                && (selectedHokenInfTokureiYm2 != sinDate)
                && !string.IsNullOrEmpty(hokenSyaNo.Trim())
                && isShahoOrKokuho
                && !isExpirated
                )
            {
                var paramsMessage = new string[] { "75歳到達月ですが、自己負担限度額の特例対象年月が入力されていません。", "保険", "無視する", "戻る" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidTokkurei);
            }
            return new ValidMainInsuranceOutputData(true, message, ValidMainInsuranceStatus.ValidSuccess);
        }

        private string IsValidConfirmDateAgeCheck(bool isAddNew, bool isExpirated, bool isShahoOrKokuho, string hokensyaNo, int confirmDate, int ptBirthday, int sinDate, int hpId)
        {
            var message = "";
            if (isAddNew)
            {
                return message;
            }
            if (isExpirated)
            {
                return message;
            }
            if (!isShahoOrKokuho)
            {
                return message;
            }
            string hokenSyaNo = hokensyaNo;
            if (hokenSyaNo.Length == 8 && (hokenSyaNo.StartsWith("109") || hokenSyaNo.StartsWith("99")))
            {
                return message;
            }
            var configCheckAge = _systemConfRepository.GetSettingValue(hpId, 1005, 0);
            if (configCheckAge == 1)
            {
                int invalidAgeCheck = 0;
                string checkParam = _systemConfRepository.GetSettingParams(hpId, 1005, 0);
                var splittedParam = checkParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var param in splittedParam)
                {
                    int ageCheck = Int32.Parse(param.Trim());
                    if (ageCheck == 0) continue;

                    if (!IsValidAgeCheckConfirm(ptBirthday, sinDate, ageCheck, confirmDate) && invalidAgeCheck <= ageCheck)
                    {
                        invalidAgeCheck = ageCheck;
                    }
                }
                if (invalidAgeCheck != 0)
                {
                    string cardName;
                    int age = CIUtil.SDateToAge(ptBirthday, sinDate);
                    if (age >= 70)
                    {
                        cardName = "高齢受給者証";
                    }
                    else
                    {
                        cardName = "保険証";
                    }
                    var paramsMessage = new string[] { $"{invalidAgeCheck}歳となりました。", cardName, "無視する", "戻る" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }
            }
            return message;
        }

        private bool IsValidAgeCheckConfirm(int ptBirthday, int sinDate, int ageCheck, int confirmDate)
        {
            int birthDay = ptBirthday;
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

        private ValidMainInsuranceOutputData IsValidHokenMstDate(int startDate, int endDate, int sinDate, string displayTextMaster)
        {
            var message = "";
            int hokenStartDate = startDate;
            int hokenEndDate = endDate;
            // 期限切れﾁｪｯｸ(有効保険の場合のみ)
            if ((hokenStartDate <= sinDate || hokenStartDate == 0)
                && (hokenEndDate >= sinDate || hokenEndDate == 0))
            {
                if (hokenStartDate > sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(startDate) + "～)", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenMstStartDate);
                }
                if (hokenEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(hokenEndDate) + ")", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    return new ValidMainInsuranceOutputData(false, message, ValidMainInsuranceStatus.InvalidHokenMstEndDate);
                }
            }
            return new ValidMainInsuranceOutputData(true, message, ValidMainInsuranceStatus.ValidSuccess);
        }
    }
}
