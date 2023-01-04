using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Common;
using UseCase.Insurance.ValidHokenInfAllType;
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
            List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails = new List<ResultValidateInsurance<ValidMainInsuranceStatus>>();
            try
            {
                // Get HokenMst
                var hokenMst = _patientInforRepository.GetHokenMstByInfor(inputData.SelectedHokenInfHokenNo, inputData.SelectedHokenInfHokenEdraNo);

                // Get HokenSyaMst
                //get FindHokensyaMstByNoNotrack
                string houbetuNo = string.Empty;
                string hokensyaNoSearch = string.Empty;
                CIUtil.GetHokensyaHoubetu(inputData.HokenSyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                var hokenSyaMst = _patientInforRepository.GetHokenSyaMstByInfor(inputData.HpId, houbetuNo, hokensyaNoSearch);

                // check validate Input
                CheckValidateInputData(ref validateDetails, inputData);
                switch (inputData.HokenKbn)
                {
                    // 自費
                    case 0:

                        var checkMessageIsValidJihi = IsValidJihi(inputData.SelectedHokenInfHokenNo);
                        if (!String.IsNullOrEmpty(checkMessageIsValidJihi))
                        {
                            validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidJihi, checkMessageIsValidJihi, TypeMessage.TypeMessageError));
                        }
                        break;
                    // 社保
                    case 1:
                        if(inputData.SelectedHokenInfHoubetu == HokenConstant.HOUBETU_NASHI)
                        {
                            var checkMessageIsValidHokenNashiOnly = IsValidHokenNashiOnly(inputData.IsSelectedHokenPattern, inputData.IsSelectedHokenInf, inputData.HokenKbn, inputData.SelectedHokenInfHoubetu, inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4);
                            if (!String.IsNullOrEmpty(checkMessageIsValidHokenNashiOnly))
                            {
                                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenNashiOnly, checkMessageIsValidHokenNashiOnly, TypeMessage.TypeMessageError));
                            }
                        }
                        else
                        {
                            IsValidShaho(ref validateDetails, inputData.IsSelectedHokenPattern, inputData.SelectedHokenPatternIsAddNew, inputData.SelectedHokenPatternIsEmptyHoken,
                                                inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4, inputData.IsSelectedHokenInf, inputData.HokenKbn,
                                                inputData.SelectedHokenInfHoubetu, inputData.IsSelectedHokenInf, inputData.SelectedHokenInfIsAddNew, inputData.HpId, inputData.SinDate,
                                                inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5,
                                                inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfIsJihi, inputData.HokenSyaNo ?? string.Empty, inputData.SelectedHokenInfHokenNo, inputData.IsSelectedHokenMst, hokenMst.Houbetu, hokenMst.HokenNo, hokenMst.CheckDigit, inputData.PtBirthday,
                                                hokenMst.AgeStart, hokenMst.AgeEnd, inputData.SelectedHokenInfKigo, inputData.SelectedHokenInfBango, hokenSyaMst.IsKigoNa, inputData.SelectedHokenInfHonkeKbn, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate,
                                                inputData.SelectedHokenInfTokureiYm1, inputData.SelectedHokenInfTokureiYm2, inputData.SelectedHokenInfIsShahoOrKokuho, inputData.SelectedHokenInfIsExpirated, inputData.SelectedHokenInfConfirmDate, hokenMst.StartDate, hokenMst.EndDate, hokenMst.DisplayTextMaster, inputData.HokenInfIsNoHoken, inputData.SelectedHokenInfIsAddHokenCheck, inputData.SelectedHokenInfHokenChecksCount);

                        }
                        break;
                    // 国保
                    case 2:
                        IsValidShaho(ref validateDetails, inputData.IsSelectedHokenPattern, inputData.SelectedHokenInfIsAddNew, inputData.SelectedHokenPatternIsEmptyHoken,
                                                inputData.SelectedHokenPatternIsEmptyKohi1, inputData.SelectedHokenPatternIsEmptyKohi2, inputData.SelectedHokenPatternIsEmptyKohi3, inputData.SelectedHokenPatternIsEmptyKohi4, inputData.IsSelectedHokenInf, inputData.HokenKbn,
                                                inputData.SelectedHokenInfHoubetu, inputData.IsSelectedHokenInf, inputData.SelectedHokenInfIsAddNew, inputData.HpId, inputData.SinDate,
                                                inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5,
                                                inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfIsJihi, inputData.HokenSyaNo ?? string.Empty, inputData.SelectedHokenInfHokenNo, inputData.IsSelectedHokenMst, hokenMst.Houbetu, hokenMst.HokenNo, hokenMst.CheckDigit, inputData.PtBirthday,
                                                hokenMst.AgeStart, hokenMst.AgeEnd, inputData.SelectedHokenInfKigo, inputData.SelectedHokenInfBango, hokenSyaMst.IsKigoNa, inputData.SelectedHokenInfHonkeKbn, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate,
                                                inputData.SelectedHokenInfTokureiYm1, inputData.SelectedHokenInfTokureiYm2, inputData.SelectedHokenInfIsShahoOrKokuho, inputData.SelectedHokenInfIsExpirated, inputData.SelectedHokenInfConfirmDate, hokenMst.StartDate, hokenMst.EndDate, hokenMst.DisplayTextMaster, inputData.HokenInfIsNoHoken, inputData.SelectedHokenInfIsAddHokenCheck, inputData.SelectedHokenInfHokenChecksCount);
                        break;
                }
            }
            catch (Exception ex)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidFaild, ex.Message, TypeMessage.TypeMessageError));
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
            }
            return new ValidMainInsuranceOutputData(validateDetails);
        }

        private void CheckValidateInputData(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, ValidMainInsuranceInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHpId, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SinDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSinDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.PtBirthday < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidPtBirthday, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.HokenKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.HokenKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfHokenNo < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenNo, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfHokenEdraNo < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenEdraNo, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfStartDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfStartDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfEndDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfEndDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfHonkeKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfTokureiYm1 < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm1, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfTokureiYm2 < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm2, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfConfirmDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidSelectedHokenInfConfirmDate, string.Empty, TypeMessage.TypeMessageError));
            }
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


        private void IsValidShaho(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, bool isSelectedHokenPattern, bool isAddNew, bool isEmptyHoken, bool isEmptyKohi1, bool isEmptyKohi2, bool isEmptyKohi3, bool isEmptyKohi4, bool isSelectedHokenInf, int hokenKbn, string selectedHokenInfHoubetu, bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText, bool hokenInfIsNoHoken, bool selectedHokenInfIsAddHokenCheck, int selectedHokenInfHokenChecksCount)
        {
            // Validate empty hoken
            var checkMessageIsValidEmptyHoken = IsValidEmptyHoken(isSelectedHokenPattern, isAddNew, isEmptyHoken, isEmptyKohi1, isEmptyKohi2, isEmptyKohi3, isEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidEmptyHoken))
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidEmptyHoken, checkMessageIsValidEmptyHoken, TypeMessage.TypeMessageWarning));
            }
            // Validate HokenNashi only
            var checkMessageIsValidHokenNashiOnly = IsValidHokenNashiOnly(isSelectedHokenPattern, isSelectedHokenInf, hokenKbn, selectedHokenInfHoubetu, isEmptyKohi1, isEmptyKohi2, isEmptyKohi3, isEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenNashiOnly))
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenNashiOnly, checkMessageIsValidHokenNashiOnly, TypeMessage.TypeMessageError));
            }
            // Valiate HokenInf
            IsValidHokenInf(ref validateDetails, selectedHokenInf, selectedHokenInfIsAddNew, hokenKbn, selectedHokenInfHoubetu, hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5, selectedHokenInfStartDate, selectedHokenInfEndDate, selectedHokenInfIsJihi, hokenSyaNo, hokenNo, isHaveSelectedHokenMst, selectedHokenInfHoubetu, sHokenMstHoubetsuNumber, sHokenMstHokenNumber, sHokenMstCheckDegit, ptBirthday, sHokenMstAgeStart, sHokenMstAgeEnd, kigo, bango, hokenMstIsKigoNa, honkeKbn, startDate, endDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated, selectedHokenInfconfirmDate, selectedHokenMstStartDate, selectedHokenMstEndDate, selectedHokenMstDisplayText, hokenInfIsNoHoken, selectedHokenInfIsAddHokenCheck, selectedHokenInfHokenChecksCount);
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

        private void IsValidHokenInf(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hokenKbn, string selectedHokenInfHoubetu, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText, bool hokenInfIsNoHoken, bool selectedHokenInfIsAddHokenCheck, int selectedHokenInfHokenChecksCount)
        {
            var message = "";
            if(!selectedHokenInf)
            {
                return;
            }
            // Validate not HokenInf
            if (hokenKbn == 1 && selectedHokenInfHoubetu == HokenConstant.HOUBETU_NASHI)
            {
                IsValidHokenNashi(ref validateDetails, hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5, selectedHokenInfStartDate, selectedHokenInfEndDate);
            }
            // Validate Jihi
            if (selectedHokenInfIsJihi)
            {
                return;
            }

            IsValidHokenDetail(ref validateDetails, hpId, sinDate, selectedHokenInfTokki1, selectedHokenInfTokki2, selectedHokenInfTokki3, selectedHokenInfTokki4, selectedHokenInfTokki5);

            CHKHokno_Fnc(ref validateDetails, hokenSyaNo, hokenNo, isHaveSelectedHokenMst, houbetu, sHokenMstHoubetsuNumber, sHokenMstHokenNumber, sHokenMstCheckDegit, ptBirthday, sHokenMstAgeStart, sHokenMstAgeEnd);

            if (string.IsNullOrEmpty(hokenSyaNo))
            {
                var paramsMessage = new string[] { "保険者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokensyaNoNull, message, TypeMessage.TypeMessageError));
            }
            if (hokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenNoEquals0, message, TypeMessage.TypeMessageError));
            }
            if (string.IsNullOrEmpty(hokenSyaNo) || Int32.Parse(hokenSyaNo) == 0)
            {
                var paramsMessage = new string[] { "保険者番号は 0 〜 9 の範囲で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokensyaNoEquals0, message, TypeMessage.TypeMessageError));
            }
            // 記号
            if (hokenSyaNo.Length == 8 && hokenSyaNo.Trim().StartsWith("39"))
            {
                if (!string.IsNullOrEmpty(kigo)
                    && !string.IsNullOrEmpty(kigo.Trim(' '))) //Trim only half-size space
                {
                    var paramsMessage = new string[] { "後期高齢者の", "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00150, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokensyaNoLength8StartWith39, message, TypeMessage.TypeMessageError));
                }
            }
            else
            {
                if (hokenMstIsKigoNa == 0 && (string.IsNullOrEmpty(kigo)
                    || string.IsNullOrEmpty(kigo.Trim(' '))))
                {
                    var paramsMessage = new string[] { "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidKigoNull, message, TypeMessage.TypeMessageError));
                }
            }
            if (string.IsNullOrEmpty(bango)
                    || string.IsNullOrEmpty(bango.Trim(' '))) //Trim only half-size space
            {
                var paramsMessage = new string[] { "被保険者証番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidBangoNull, message, TypeMessage.TypeMessageError));
            }
            if (honkeKbn == 0)
            {
                var paramsMessage = new string[] { "本人家族区分" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenKbnEquals0, message, TypeMessage.TypeMessageError));
            }
            
            IsValidYukoKigen(ref validateDetails, startDate, endDate);
            IsValidTokkurei(ref validateDetails, ptBirthday, sinDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, hokenSyaNo, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated);

            string checkMessageIsValidConfirmDateAgeCheck = IsValidConfirmDateAgeCheck(selectedHokenInfIsAddNew, selectedHokenInfisExpirated, selectedHokenInfisShahoOrKokuho, hokenSyaNo, selectedHokenInfconfirmDate, ptBirthday, sinDate, hpId);
            if (!string.IsNullOrEmpty(checkMessageIsValidConfirmDateAgeCheck))
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidConfirmDateAgeCheck, checkMessageIsValidConfirmDateAgeCheck, TypeMessage.TypeMessageError));
            }

            string checkMessageIsValidConfirmDateHoken = IsValidConfirmDateHoken(sinDate,
                                                            selectedHokenInfisExpirated,
                                                            selectedHokenInfIsJihi,
                                                            hokenInfIsNoHoken,
                                                            selectedHokenInfisExpirated,
                                                            selectedHokenInfconfirmDate,
                                                            selectedHokenInfIsAddNew,
                                                            selectedHokenInfIsAddHokenCheck,
                                                            selectedHokenInfHokenChecksCount);

            if (!string.IsNullOrEmpty(checkMessageIsValidConfirmDateHoken))
            {
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InValidConfirmDateHoken, checkMessageIsValidConfirmDateAgeCheck, TypeMessage.TypeMessageConfirmation));
            }

            // check valid hokenmst date
            IsValidHokenMstDate(ref validateDetails ,selectedHokenInfStartDate, selectedHokenInfEndDate , sinDate, selectedHokenMstDisplayText, selectedHokenMstStartDate, selectedHokenMstEndDate );
        }

        private void IsValidHokenNashi(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, int hpId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
        {
            IsValidHokenDetail(ref validateDetails ,hpId, sinDate, tokki1, tokki2, tokki3, tokki4, tokki5);

            IsValidYukoKigen(ref validateDetails, startDate, endDate);
        }

        private void IsValidYukoKigen(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            int yukoFromDate = selectedHokenInfStartDate;
            int yukoToDate = selectedHokenInfEndDate;
            if (yukoFromDate != 0 && yukoToDate != 0 && yukoFromDate > yukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidYukoKigen, message, TypeMessage.TypeMessageError));
            }
        }

        private void IsValidHokenDetail(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, int hpId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
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
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue1, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki2Value) && tokki2Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki2Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue21, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue31, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue41, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue51, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki2Value))
            {
                if (!_isValidLengthTokki(tokki2Value))
                {
                    var paramsMessage = new string[] { "特記事項２", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue2, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue23, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue24, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue25, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki3Value))
            {
                if (!_isValidLengthTokki(tokki3Value))
                {
                    var paramsMessage = new string[] { "特記事項３", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue3, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue34, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue35, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki4Value))
            {
                if (!_isValidLengthTokki(tokki4Value))
                {
                    var paramsMessage = new string[] { "特記事項４", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue4, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki4Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue45, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki5Value) && !_isValidLengthTokki(tokki5Value))
            {
                var paramsMessage = new string[] { "特記事項５", "2文字" };
                message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkiValue5, message, TypeMessage.TypeMessageError));
            }
        }

        private void CHKHokno_Fnc(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd)
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
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0, message, TypeMessage.TypeMessageError));
                }
            }
            //保険番号入力あり
            else
            {
                if (hokenNo == 0)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenNoEquals0, message, TypeMessage.TypeMessageError));
                }
                if (!isHaveSelectedHokenMst)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenNoHaveHokenMst, message, TypeMessage.TypeMessageError));
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
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHoubetu, message, TypeMessage.TypeMessageError));
                }
                //チェックデジット
                if (sHokenMstCheckDegit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(hokenSyaNo)))
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidCheckDigitEquals1, message, TypeMessage.TypeMessageError));
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
                        validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidCheckAgeHokenMst, message, TypeMessage.TypeMessageError));
                    }
                }
            }
        }

        private void IsValidTokkurei(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, int ptBirthday, int sinDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, string hokenSyaNo, bool isShahoOrKokuho, bool isExpirated)
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
                validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidTokkurei, message, TypeMessage.TypeMessageError));
            }
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
            var configCheckAge = _systemConfRepository.GetSettingValue(hpId, 1005, hpId);
            if (configCheckAge == 1)
            {
                int invalidAgeCheck = 0;
                string checkParam = _systemConfRepository.GetSettingParams(hpId, 1005, hpId);
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

        private void IsValidHokenMstDate(ref List<ResultValidateInsurance<ValidMainInsuranceStatus>> validateDetails, int startDate, int endDate, int sinDate, string displayTextMaster, int selecttedHokenMstStartDate, int selecttedHokenMstEndDate)
        {
            var message = "";
            int hokenStartDate = startDate;
            int hokenEndDate = endDate;
            // 期限切れﾁｪｯｸ(有効保険の場合のみ)
            if ((hokenStartDate <= sinDate || hokenStartDate == 0)
                && (hokenEndDate >= sinDate || hokenEndDate == 0))
            {
                if (selecttedHokenMstStartDate > sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(selecttedHokenMstStartDate) + "～)", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenMstStartDate, message, TypeMessage.TypeMessageConfirmation));
                }
                if (selecttedHokenMstEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(selecttedHokenMstEndDate) + ")", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidMainInsuranceStatus>(ValidMainInsuranceStatus.InvalidHokenMstEndDate, message, TypeMessage.TypeMessageConfirmation));
                }
            }
        }


        public string IsValidConfirmDateHoken(int sinDate, bool isExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, bool hokenInfIsExpirated, int hokenInfConfirmDate, bool selectedHokenInfIsAddNew, bool selectedHokenInfIsAddHokenCheck, int selectedHokenInfHokenChecksCount)
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
                if (selectedHokenInfIsAddNew || (selectedHokenInfIsAddHokenCheck && selectedHokenInfHokenChecksCount <= 0))
                {
                    //Ignore
                }
                else
                {
                    var stringParams = new string[] { "保険", "保険証" };
                    var stringParams2 = new string[] { "無視する", "戻る" };
                    checkComfirmDateHoken = string.Format(ErrorMessage.MessageType_mChk00030, stringParams, stringParams2);
                    return checkComfirmDateHoken;
                }
            }
            return checkComfirmDateHoken;
        }
    }
}
