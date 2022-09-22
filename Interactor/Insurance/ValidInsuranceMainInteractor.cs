using Domain.Constant;
using Domain.Models.PatientInfor;
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
        public ValidInsuranceMainInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
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
                    result = IsValidShaho();
                    break;
                // 国保
                case 2:
                    result = IsValidKokuho();
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

        private string IsValidShaho(bool isSelectedHokenPattern, bool isAddNew, bool isEmptyHoken, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4, bool isSelectedHokenInf, int hokenKbn, string selectedHokenInfHoubetu)
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
            if (!IsValidHokenInf())
            {
                return false;
            }

            return string.Empty;
        }

        public string IsValidEmptyHoken(bool isSelectedHokenPattern, bool isAddNew, bool isEmptyHoken, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4)
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

        public string IsValidHokenInf()
        {

        }

        public string IsValidHokenNashi(int hpId, long ptId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
        {
            var checkMessageIsValidHokenDetail = IsValidHokenDetail(hpId, ptId, sinDate, tokki1, tokki2, tokki3, tokki4, tokki5);
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
            int YukoFromDate = selectedHokenInfStartDate;
            int YukoToDate = selectedHokenInfEndDate;
            if (YukoFromDate != 0 && YukoToDate != 0 && YukoFromDate > YukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
            }

            return message;
        }

        private string IsValidHokenDetail(int hpId, long ptId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
        {
            var TokkiMstBinding = _patientInforRepository.GetListTokki(hpId, sinDate);
            var message = "";
            bool _isValidLengthTokki(string tokkiValue)
            {
                var itemToki = TokkiMstBinding.FirstOrDefault(x => x.TokkiCd == tokkiValue);
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

        private string IsValidYukoKigen(int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            if (selectedHokenInfStartDate != 0 && selectedHokenInfEndDate != 0 && selectedHokenInfStartDate > selectedHokenInfEndDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return message;
            }

            return message;
        }
    }
}
