using Domain.Constant;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
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
                    var messageCheckIsValidShaho = IsValidShaho(inputData.IsSelectedHokenPattern, inputData.SelectedHokenInfIsAddNew, inputData.SelectedHokenPatternIsEmptyHoken, 
                                            inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4, inputData.IsSelectedHokenInf, inputData.HokenKbn,
                                            inputData.SelectedHokenInfHoubetu, inputData.IsSelectedHokenInf, inputData.SelectedHokenInfIsAddNew, inputData.HpId, inputData.SinDate,
                                            inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5,
                                            inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfIsJihi, inputData.HokenSyaNo, inputData.SelectedHokenInfHokenNo, inputData.IsSelectedHokenMst, inputData.SelectedHokenMstHoubetu, inputData.SelectedHokenMstHokenNo, inputData.SelectedHokenMstCheckDegit, inputData.PtBirthday,
                                            inputData.SelectedHokenMstAgeStart, inputData.SelectedHokenMstAgeEnd, inputData.SelectedHokenInfKigo, inputData.SelectedHokenInfBango, inputData.SelectedHokenInfHokensyaMstIsKigoNa, inputData.SelectedHokenInfHonkeKbn, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, 
                                            inputData.SelectedHokenInfTokureiYm1, inputData.SelectedHokenInfTokureiYm2, inputData.SelectedHokenInfIsShahoOrKokuho, inputData.SelectedHokenInfIsExpirated, inputData.SelectedHokenInfConfirmDate,  inputData.SelectedHokenMstStartDate, inputData.SelectedHokenMstEndDate, inputData.SelectedHokenMstDisplayText);
                    if (!String.IsNullOrEmpty(messageCheckIsValidShaho))
                    {
                        return new ValidMainInsuranceOutputData(false, messageCheckIsValidShaho, ValidMainInsuranceStatus.InvalidFaild);
                    }
                    break;
                // 国保
                case 2:
                    var messageCheckIsValidKokuho = IsValidShaho(inputData.IsSelectedHokenPattern, inputData.SelectedHokenInfIsAddNew, inputData.SelectedHokenPatternIsEmptyHoken,
                                             inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4, inputData.IsSelectedHokenInf, inputData.HokenKbn,
                                             inputData.SelectedHokenInfHoubetu, inputData.IsSelectedHokenInf, inputData.SelectedHokenInfIsAddNew, inputData.HpId, inputData.SinDate,
                                             inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5,
                                             inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfIsJihi, inputData.HokenSyaNo, inputData.SelectedHokenInfHokenNo, inputData.IsSelectedHokenMst, inputData.SelectedHokenMstHoubetu, inputData.SelectedHokenMstHokenNo, inputData.SelectedHokenMstCheckDegit, inputData.PtBirthday,
                                             inputData.SelectedHokenMstAgeStart, inputData.SelectedHokenMstAgeEnd, inputData.SelectedHokenInfKigo, inputData.SelectedHokenInfBango, inputData.SelectedHokenInfHokensyaMstIsKigoNa, inputData.SelectedHokenInfHonkeKbn, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate,
                                             inputData.SelectedHokenInfTokureiYm1, inputData.SelectedHokenInfTokureiYm2, inputData.SelectedHokenInfIsShahoOrKokuho, inputData.SelectedHokenInfIsExpirated, inputData.SelectedHokenInfConfirmDate, inputData.SelectedHokenMstStartDate, inputData.SelectedHokenMstEndDate, inputData.SelectedHokenMstDisplayText);
                    if (!String.IsNullOrEmpty(messageCheckIsValidKokuho))
                    {
                        return new ValidMainInsuranceOutputData(false, messageCheckIsValidKokuho, ValidMainInsuranceStatus.InvalidFaild);
                    }
                    break;
            }
            return new ValidMainInsuranceOutputData(true, string.Empty, ValidMainInsuranceStatus.ValidSuccess);
        }

        private string IsValidJihi(int SelectedHokenInfHokenNo)
        {
            var message = "";
            if (SelectedHokenInfHokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
            }
            return message;
        }

        private string IsValidShaho(bool isSelectedHokenPattern, bool isAddNew, bool isEmptyHoken, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4, bool isSelectedHokenInf, int hokenKbn, string selectedHokenInfHoubetu, bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText)
        {
            // Validate empty hoken
            var checkMessageIsValidEmptyHoken = IsValidEmptyHoken(isSelectedHokenPattern, isAddNew, isEmptyHoken, isEmptyKohi1, isEmptyKohi2, isEmptyKohi3, isEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidEmptyHoken))
            {
                return checkMessageIsValidEmptyHoken;
            }
            // Validate HokenNashi only
            var checkMessageIsValidHokenNashiOnly = IsValidHokenNashiOnly(isSelectedHokenPattern, isSelectedHokenInf, hokenKbn, selectedHokenInfHoubetu, isEmptyKohi1, isEmptyKohi2, isEmptyKohi3, isEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenNashiOnly))
            {
                return checkMessageIsValidHokenNashiOnly;
            }
            // Valiate HokenInf
            var checkMessageIsValidHokenInf = IsValidHokenInf(selectedHokenInf, selectedHokenInfIsAddNew, hokenKbn, selectedHokenInfHoubetu, hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5, selectedHokenInfStartDate, selectedHokenInfEndDate, selectedHokenInfIsJihi, hokenSyaNo, hokenNo, isHaveSelectedHokenMst, selectedHokenInfHoubetu, sHokenMstHoubetsuNumber, sHokenMstHokenNumber, sHokenMstCheckDegit, ptBirthday, sHokenMstAgeStart, sHokenMstAgeEnd, kigo, bango, hokenMstIsKigoNa, honkeKbn, startDate, endDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated, selectedHokenInfconfirmDate, selectedHokenMstStartDate, selectedHokenMstEndDate, selectedHokenMstDisplayText);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenInf))
            {
                return checkMessageIsValidHokenInf;
            }
            return string.Empty;
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

        public string IsValidHokenInf(bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hokenKbn, string selectedHokenInfHoubetu, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText)
        {
            var message = "";
            if(selectedHokenInf)
            {
                return message;
            }
            // Validate not HokenInf
            if (hokenKbn == 1 && selectedHokenInfHoubetu == HokenConstant.HOUBETU_NASHI)
            {
                return IsValidHokenNashi(hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5, selectedHokenInfStartDate, selectedHokenInfEndDate);
            }
            // Validate Jihi
            if (selectedHokenInfIsJihi)
            {
                return message;
            }
            var checkMessageIsValidHokenDetail = IsValidHokenDetail(hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5);
            if(!String.IsNullOrEmpty(checkMessageIsValidHokenDetail))
            {
                return checkMessageIsValidHokenDetail;
            }
            var checkMessageCHKHokno_Fnc = CHKHokno_Fnc(hokenSyaNo, hokenNo, isHaveSelectedHokenMst, houbetu, sHokenMstHoubetsuNumber, sHokenMstHokenNumber, sHokenMstCheckDegit, ptBirthday, sHokenMstAgeStart, sHokenMstAgeEnd);
            if (!String.IsNullOrEmpty(checkMessageCHKHokno_Fnc))
            {
                return checkMessageCHKHokno_Fnc;
            }
            if (string.IsNullOrEmpty(hokenSyaNo))
            {
                var paramsMessage = new string[] { "保険者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            if (hokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return message;
            }
            if (Int32.Parse(hokenSyaNo) == 0)
            {
                var paramsMessage = new string[] { "保険者番号は 0 〜 9 の範囲で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return message;
            }
            // 記号
            if (hokenSyaNo.Length == 8 && hokenSyaNo.Trim().StartsWith("39"))
            {
                if (!string.IsNullOrEmpty(kigo)
                    && !string.IsNullOrEmpty(kigo.Trim(' '))) //Trim only half-size space
                {
                    var paramsMessage = new string[] { "後期高齢者の", "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00150, paramsMessage);
                    return message;
                }
            }
            else
            {
                if (hokenMstIsKigoNa == 0 && (string.IsNullOrEmpty(kigo)
                    || string.IsNullOrEmpty(kigo.Trim(' '))))
                {
                    var paramsMessage = new string[] { "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }
            }
            if (string.IsNullOrEmpty(bango)
                    || string.IsNullOrEmpty(bango.Trim(' '))) //Trim only half-size space
            {
                var paramsMessage = new string[] { "被保険者証番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            if (honkeKbn == 0)
            {
                var paramsMessage = new string[] { "本人家族区分" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            var checkMessageIsValidYukoKigen = IsValidYukoKigen(startDate, endDate);
            if (!String.IsNullOrEmpty(checkMessageIsValidYukoKigen))
            {
                return checkMessageIsValidYukoKigen;
            }
            var checkMessageIsValidTokkurei = IsValidTokkurei(ptBirthday, sinDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, hokenSyaNo, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated);
            if (!String.IsNullOrEmpty(checkMessageIsValidTokkurei))
            {
                return checkMessageIsValidTokkurei;
            }
            var checkMessageIsValidConfirmDateAgeCheck = IsValidConfirmDateAgeCheck(selectedHokenInfIsAddNew, selectedHokenInfisExpirated, selectedHokenInfisShahoOrKokuho, hokenSyaNo, selectedHokenInfconfirmDate, ptBirthday, sinDate);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateAgeCheck))
            {
                return checkMessageIsValidConfirmDateAgeCheck;
            }
            // check valid hokenmst date
            var checkMessageIsValidHokenMstDate = IsValidHokenMstDate(selectedHokenMstStartDate, selectedHokenMstEndDate, sinDate, selectedHokenMstDisplayText);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenMstDate))
            {
                return checkMessageIsValidHokenMstDate;
            }
            return message;
        }

        public string IsValidHokenNashi(int hpId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
        {
            var checkMessageIsValidHokenDetail = IsValidHokenDetail(hpId, sinDate, tokki1, tokki2, tokki3, tokki4, tokki5);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenDetail))
            {
                return checkMessageIsValidHokenDetail;
            }
            var checkMessageIsValidYukoKigen = IsValidYukoKigen(startDate, endDate);
            if (!String.IsNullOrEmpty(checkMessageIsValidYukoKigen))
            {
                return checkMessageIsValidYukoKigen;
            }
            return string.Empty;
        }

        private string IsValidYukoKigen(int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            int yukoFromDate = selectedHokenInfStartDate;
            int yukoToDate = selectedHokenInfEndDate;
            if (yukoFromDate != 0 && yukoToDate != 0 && yukoFromDate > yukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
            }
            return message;
        }

        private string IsValidHokenDetail(int hpId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
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
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki2Value) && tokki2Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki2Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
            }
            if (!string.IsNullOrEmpty(tokki2Value))
            {
                if (!_isValidLengthTokki(tokki2Value))
                {
                    var paramsMessage = new string[] { "特記事項２", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
            }
            if (!string.IsNullOrEmpty(tokki3Value))
            {
                if (!_isValidLengthTokki(tokki3Value))
                {
                    var paramsMessage = new string[] { "特記事項３", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
            }
            if (!string.IsNullOrEmpty(tokki4Value))
            {
                if (!_isValidLengthTokki(tokki4Value))
                {
                    var paramsMessage = new string[] { "特記事項４", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return message;
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki4Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return message;
                }
            }
            if (!string.IsNullOrEmpty(tokki5Value) && !_isValidLengthTokki(tokki5Value))
            {
                var paramsMessage = new string[] { "特記事項５", "2文字" };
                message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                return message;
            }
            return message;
        }

        private string CHKHokno_Fnc(string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd)
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
                    return message;
                }
            }
            //保険番号入力あり
            else
            {
                if (hokenNo == 0)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
                }
                if (!isHaveSelectedHokenMst)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
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
                    return message;
                }
                //チェックデジット
                if (sHokenMstCheckDegit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(hokenSyaNo)))
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    return message;
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
                        return message;
                    }
                }
            }
            return message;
        }

        private string IsValidTokkurei(int ptBirthday, int sinDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, string hokenSyaNo, bool isShahoOrKokuho, bool isExpirated)
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
                return message;
            }
            return message;
        }

        private string IsValidConfirmDateAgeCheck(bool isAddNew, bool isExpirated, bool isShahoOrKokuho, string hokensyaNo, int confirmDate, int ptBirthday, int sinDate)
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
            var configCheckAge = _systemConfRepository.GetSettingValue(1005, 0);
            if (configCheckAge == 1)
            {
                int invalidAgeCheck = 0;
                string checkParam = _systemConfRepository.GetSettingParams(1005, 0);
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

        private string IsValidHokenMstDate(int startDate, int endDate, int sinDate, string displayTextMaster)
        {
            var message = "";
            int HokenStartDate = startDate;
            int HokenEndDate = endDate;
            // 期限切れﾁｪｯｸ(有効保険の場合のみ)
            if ((HokenStartDate <= sinDate || HokenStartDate == 0)
                && (HokenEndDate >= sinDate || HokenEndDate == 0))
            {
                if (HokenStartDate > sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(startDate) + "～)", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    return message;
                }
                if (HokenEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(HokenEndDate) + ")", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    return message;
                }
            }
            return message;
        }
    }
}
