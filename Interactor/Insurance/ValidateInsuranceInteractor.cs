using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Common;
using Helper.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidateInsurance;
using UseCase.OrdInfs.ValidationInputItem;

namespace Interactor.Insurance
{
    public class ValidateInsuranceInteractor : IValidateInsuranceInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        public ValidateInsuranceInteractor(IPatientInforRepository patientInforRepository, ISystemConfRepository systemConfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _systemConfRepository = systemConfRepository;
        }

        public ValidateInsuranceOutputData Handle(ValidateInsuranceInputData inputData)
        {
            try
            {
                //convert inputData ValidateInsuranceDto -> ValidateInsuranceModel 
                var listValidateInsuranceModel = new List<ValidateInsuranceModel>();
                if(inputData.ListDataModel.Any())
                {
                    foreach (var item in inputData.ListDataModel)
                    {
                        var itemValidateInsurance = ConvertModelToDto(item, inputData.SinDate);
                        listValidateInsuranceModel.Add(itemValidateInsurance);
                    }
                }    
                var listValidateData = new List<ValidateInsuranceListItem>();
                var itemListValidate = new List<ValidateInsuranceItem>();
                // check validate Input
                var checkValidInput = CheckValidateInputData(inputData, listValidateData);
                if (!checkValidInput.Result)
                {
                    return checkValidInput;
                }

                var index = 0;
                var listHokenPattern = new List<InsuranceModel>();
                var listHokenInf = new List<HokenInfModel>();
                if (inputData.ListDataModel.Any())
                {
                    foreach (var item in listValidateInsuranceModel)
                    {
                        var listItemValidateAdd = new List<ValidateInsuranceItem>();
                        switch (item.SelectedHokenPattern.HokenKbn)
                        {
                            case 0:
                                var checkMessageIsValidJihi = IsValidJihi(item.SelectedHokenInf.HokenNo);
                                if (!String.IsNullOrEmpty(checkMessageIsValidJihi))
                                {
                                    var itemValidateJihi = new ValidateInsuranceItem(false, checkMessageIsValidJihi, ValidateInsuranceStatus.InvalidJihiSelectedHokenInfHokenNoEquals0);
                                    listItemValidateAdd.Add(itemValidateJihi);
                                }
                                // ignore
                                break;
                            // 社保
                            case 1:
                                var checkIsValidShaho = IsValidShaho(item, inputData.HpId, inputData.SinDate, inputData.PtBirthday);
                                if (!checkIsValidShaho.Result)
                                {
                                    listItemValidateAdd.Add(checkIsValidShaho);
                                }
                                break;
                            // 国保
                            case 2:
                                var checkIsValidKokuho = IsValidShaho(item, inputData.HpId, inputData.SinDate, inputData.PtBirthday);
                                if (!checkIsValidKokuho.Result)
                                {
                                    listItemValidateAdd.Add(checkIsValidKokuho);
                                }
                                break;
                            // 労災(短期給付)	
                            case 11:
                                var checkIsValidRodo = IsValidRodo(item.SelectedHokenInf.RodoBango, item.SelectedHokenPattern.HokenKbn, item.SelectedHokenInf.ListRousaiTenki, item.SelectedHokenInf.RousaiSaigaiKbn, item.SelectedHokenInf.RousaiSyobyoDate, item.SelectedHokenInf.RousaiSyobyoCd, item.SelectedHokenInf.RyoyoStartDate, item.SelectedHokenInf.RyoyoEndDate, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate, inputData.SinDate, item.SelectedHokenInf.IsAddNew, inputData.HpId);
                                if (!checkIsValidRodo.Result)
                                {
                                    listItemValidateAdd.Add(checkIsValidRodo);
                                }
                                break;
                            // 労災(傷病年金)
                            case 12:
                                var checkIsValidNenkin = IsValidNenkin(item.SelectedHokenInf.NenkinBango, item.SelectedHokenPattern.HokenKbn, item.SelectedHokenInf.ListRousaiTenki, item.SelectedHokenInf.RousaiSaigaiKbn, item.SelectedHokenInf.RousaiSyobyoDate, item.SelectedHokenInf.RousaiSyobyoCd, item.SelectedHokenInf.RyoyoStartDate, item.SelectedHokenInf.RyoyoEndDate, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate, inputData.SinDate, item.SelectedHokenInf.IsAddNew, inputData.HpId);
                                if (!checkIsValidNenkin.Result)
                                {
                                    listItemValidateAdd.Add(checkIsValidNenkin);
                                }
                                break;
                            // アフターケア
                            case 13:
                                var checkIsValidKenko = IsValidKenko(item.SelectedHokenInf.KenkoKanriBango, item.SelectedHokenPattern.HokenKbn, item.SelectedHokenInf.ListRousaiTenki, item.SelectedHokenInf.RousaiSaigaiKbn, item.SelectedHokenInf.RousaiSyobyoDate, item.SelectedHokenInf.RousaiSyobyoCd, item.SelectedHokenInf.RyoyoStartDate, item.SelectedHokenInf.RyoyoEndDate, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate, inputData.SinDate, item.SelectedHokenInf.IsAddNew, inputData.HpId);
                                if (!checkIsValidKenko.Result)
                                {
                                    listItemValidateAdd.Add(checkIsValidKenko);
                                }
                                break;
                            // 自賠責
                            case 14:
                                var checkIsValidJibai = IsValidJibai(item.SelectedHokenInf.ListRousaiTenki);
                                if (!checkIsValidJibai.Result)
                                {
                                    listItemValidateAdd.Add(checkIsValidJibai);
                                }
                                break;
                        }
                        index++;
                        listHokenPattern.Add(item.SelectedHokenPattern);
                        listHokenInf.Add(item.SelectedHokenInf);
                        listValidateData.Add(new ValidateInsuranceListItem(listItemValidateAdd));
                    }

                    // Check Duplicate Pattern
                    if (inputData.ListDataModel.Any())
                    {
                        var patternHokenOnlyCheckDuplicate = listHokenPattern.Where(pattern => pattern.HokenKbn >= 1 && pattern.HokenKbn <= 4 && pattern.IsDeleted == 0);
                        var isDuplicate = false;

                        foreach (var pattern in patternHokenOnlyCheckDuplicate)
                        {
                            var duplicatePattern = patternHokenOnlyCheckDuplicate.Where(item => CheckPatternDuplicate(pattern, item)).ToList();

                            if (duplicatePattern.Count > 1)
                            {
                                var patternAddNew = duplicatePattern.FirstOrDefault(item => item.IsAddNew);
                                if (patternAddNew != null)
                                {
                                    isDuplicate = true;
                                    break;
                                }
                            }
                        }

                        if (isDuplicate)
                        {
                            var paramsMessage = new string[] { "同じ組合せの保険・公１・公２・公３・公４を持つ組合せ" };
                            var message = String.Format(ErrorMessage.MessageType_mEnt00020, paramsMessage);
                            // Warning Duplicate Pattern
                            var itemCheckValidate = new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.InvalidWarningDuplicatePattern);
                            itemListValidate.Add(itemCheckValidate);
                        }
                    }

                    var checkAge = CheckAge(inputData, listHokenPattern, listHokenInf);
                    if (!checkAge.Result)
                    {
                        itemListValidate.Add(checkAge);
                    }
                    listValidateData.Add(new ValidateInsuranceListItem(itemListValidate));
                }
                if (listValidateData.Any())
                {
                    return new ValidateInsuranceOutputData(false, ValidateInsuranceStatus.InvalidFaild, listValidateData);
                }
                else
                {
                    return new ValidateInsuranceOutputData(true, ValidateInsuranceStatus.Successed, listValidateData);
                }
            }
            catch (Exception)
            {
                return new ValidateInsuranceOutputData(false, ValidateInsuranceStatus.InvalidFaild, new List<ValidateInsuranceListItem>());
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
            }
        }

        private ValidateInsuranceOutputData CheckValidateInputData(ValidateInsuranceInputData inputData, List<ValidateInsuranceListItem> listValidate)
        {
            if (inputData.HpId < 0)
            {
                return new ValidateInsuranceOutputData(false, ValidateInsuranceStatus.InvalidHpId, listValidate);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidateInsuranceOutputData(false, ValidateInsuranceStatus.InvalidSindate, listValidate);
            }

            if (inputData.PtBirthday < 0)
            {
                return new ValidateInsuranceOutputData(false, ValidateInsuranceStatus.InvalidPtBirthday, listValidate);
            }

            return new ValidateInsuranceOutputData(true, ValidateInsuranceStatus.Successed, listValidate);
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

        private ValidateInsuranceItem IsValidShaho(ValidateInsuranceModel itemModel, int hpId, int sinDate, int ptBirthday)
        {
            // Validate empty hoken
            var checkMessageIsValidEmptyHoken = IsValidEmptyHoken(itemModel.IsHaveSelectedHokenPattern, itemModel.SelectedHokenPattern.IsAddNew, itemModel.SelectedHokenPattern.IsEmptyHoken, itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.IsEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidEmptyHoken))
            {
                return new ValidateInsuranceItem(false, checkMessageIsValidEmptyHoken, ValidateInsuranceStatus.InvalidEmptyHoken);
            }
            // Validate HokenNashi only
            var checkMessageIsValidHokenNashiOnly = IsValidHokenNashiOnly(itemModel.IsHaveSelectedHokenPattern, itemModel.IsHaveSelectedHokenInf, itemModel.SelectedHokenPattern.HokenKbn, itemModel.SelectedHokenInf.Houbetu, itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.IsEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenNashiOnly))
            {
                return new ValidateInsuranceItem(false, checkMessageIsValidHokenNashiOnly, ValidateInsuranceStatus.InvalidHokenNashiOnly);
            }
            // Valiate HokenInf
            var checkIsValidHokenInf = IsValidHokenInf(itemModel, hpId, sinDate, ptBirthday);
            if (!checkIsValidHokenInf.Result)
            {
                return checkIsValidHokenInf;
            }

            //IsValidKohi 1
            var checkMessageKohi = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.Kohi1.IsHaveKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi1.FutansyaNo, itemModel.SelectedHokenPattern.Kohi1.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi1.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi1.StartDate, itemModel.SelectedHokenPattern.Kohi1.EndDate, itemModel.SelectedHokenPattern.Kohi1.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.IsJyukyusyaNoCheck,
                                               itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.IsTokusyuNoCheck, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.EndDate, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.DisplayTextMaster, 1,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi1.IsAddNew);
            if (!checkMessageKohi.Result)
            {
                return checkMessageKohi;
            }

            // check Kohi No Function1
            var checkMessageKohiNoFnc1 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.Kohi1.IsHaveKohiMst, itemModel.SelectedHokenPattern.Kohi1.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi1.FutansyaNo, itemModel.SelectedHokenPattern.Kohi1.TokusyuNo, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.IsJyukyusyaNoCheck,
                                                           itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.Houbetu, itemModel.SelectedHokenPattern.Kohi1.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.AgeStart, itemModel.SelectedHokenPattern.Kohi1.HokenMstModel.AgeEnd, 1, ptBirthday);
            if (!checkMessageKohiNoFnc1.Result)
            {
                return checkMessageKohiNoFnc1;
            }

            //IsValidKohi 2
            var checkMessageKohi2 = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.Kohi2.IsHaveKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi2.FutansyaNo, itemModel.SelectedHokenPattern.Kohi2.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi2.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi2.StartDate, itemModel.SelectedHokenPattern.Kohi2.EndDate, itemModel.SelectedHokenPattern.Kohi2.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.IsJyukyusyaNoCheck,
                                               itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.IsTokusyuNoCheck, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.EndDate, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.DisplayTextMaster, 2,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi2.IsAddNew);
            if (!checkMessageKohi2.Result)
            {
                return checkMessageKohi2;
            }

            // check Kohi No Function2
            var checkMessageKohiNoFnc2 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.Kohi2.IsHaveKohiMst, itemModel.SelectedHokenPattern.Kohi2.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi2.FutansyaNo, itemModel.SelectedHokenPattern.Kohi2.TokusyuNo, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.IsJyukyusyaNoCheck,
                                                           itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.Houbetu, itemModel.SelectedHokenPattern.Kohi2.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.AgeStart, itemModel.SelectedHokenPattern.Kohi2.HokenMstModel.AgeEnd, 2, ptBirthday);
            if (!checkMessageKohiNoFnc2.Result)
            {
                return checkMessageKohiNoFnc2;
            }

            //IsValidKohi 3
            var checkMessageKohi3 = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.Kohi3.IsHaveKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi3.FutansyaNo, itemModel.SelectedHokenPattern.Kohi3.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi3.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi3.StartDate, itemModel.SelectedHokenPattern.Kohi3.EndDate, itemModel.SelectedHokenPattern.Kohi3.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.IsJyukyusyaNoCheck,
                                               itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.IsTokusyuNoCheck, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.EndDate, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.DisplayTextMaster, 3,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi3.IsAddNew);
            if (!checkMessageKohi3.Result)
            {
                return checkMessageKohi3;
            }

            // check Kohi No Function3
            var checkMessageKohiNoFnc3 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.Kohi3.IsHaveKohiMst, itemModel.SelectedHokenPattern.Kohi3.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi3.FutansyaNo, itemModel.SelectedHokenPattern.Kohi3.TokusyuNo, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.IsJyukyusyaNoCheck,
                                                           itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.Houbetu, itemModel.SelectedHokenPattern.Kohi3.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.AgeStart, itemModel.SelectedHokenPattern.Kohi3.HokenMstModel.AgeEnd, 3, ptBirthday);
            if (!checkMessageKohiNoFnc3.Result)
            {
                return checkMessageKohiNoFnc3;
            }

            //IsValidKohi 4
            var checkMessageKohi4 = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi4, itemModel.SelectedHokenPattern.Kohi4.IsHaveKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi4.FutansyaNo, itemModel.SelectedHokenPattern.Kohi4.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi4.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi4.StartDate, itemModel.SelectedHokenPattern.Kohi4.EndDate, itemModel.SelectedHokenPattern.Kohi4.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.IsJyukyusyaNoCheck,
                                               itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.IsTokusyuNoCheck, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.EndDate, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.DisplayTextMaster, 4,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi4.IsAddNew);
            if (!checkMessageKohi4.Result)
            {
                return checkMessageKohi4;
            }

            // check Kohi No Function4
            var checkMessageKohiNoFnc4 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi4, itemModel.SelectedHokenPattern.Kohi4.IsHaveKohiMst, itemModel.SelectedHokenPattern.Kohi4.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi4.FutansyaNo, itemModel.SelectedHokenPattern.Kohi4.TokusyuNo, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.IsJyukyusyaNoCheck,
                                                           itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.IsFutansyaNoCheck, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.Houbetu, itemModel.SelectedHokenPattern.Kohi4.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.AgeStart, itemModel.SelectedHokenPattern.Kohi4.HokenMstModel.AgeEnd, 4, ptBirthday);
            if (!checkMessageKohiNoFnc4.Result)
            {
                return checkMessageKohiNoFnc4;
            }

            var checkMessageKohiAll = IsvalidKohiAll(itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.IsEmptyKohi4, itemModel.SelectedHokenPattern.Kohi1, itemModel.SelectedHokenPattern.Kohi2, itemModel.SelectedHokenPattern.Kohi3, itemModel.SelectedHokenPattern.Kohi4);
            if (!checkMessageKohiAll.Result)
            {
                return checkMessageKohiAll;
            }

            //IsValidMaruchoOnly
            var checkIsValidMaruchoOnly = IsValidMaruchoOnly(itemModel);
            if (!checkIsValidMaruchoOnly.Result)
            {
                return checkMessageKohiAll;
            }

            return new ValidateInsuranceItem(true, string.Empty, ValidateInsuranceStatus.Successed);
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

        private ValidateInsuranceItem IsValidHokenInf(ValidateInsuranceModel item, int hpId, int sinDate, int ptBirthday)
        {
            var message = "";
            if (!item.IsHaveSelectedHokenInf)
            {
                return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
            }
            // Validate not HokenInf
            if (item.SelectedHokenPattern.HokenKbn == 1 && item.SelectedHokenInf.Houbetu == HokenConstant.HOUBETU_NASHI)
            {
                var checkIsValidHokenNashi = IsValidHokenNashi(hpId, sinDate, item.SelectedHokenInf.Tokki1, item.SelectedHokenInf.Tokki2, item.SelectedHokenInf.Tokki3, item.SelectedHokenInf.Tokki4, item.SelectedHokenInf.Tokki5, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate);
                if (!checkIsValidHokenNashi.Result)
                {
                    return checkIsValidHokenNashi;
                }
            }
            // Validate Jihi
            if (item.SelectedHokenInf.IsJihi)
            {
                return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
            }
            var checkIsValidHokenDetail = IsValidHokenDetail(hpId, sinDate, item.SelectedHokenInf.Tokki1, item.SelectedHokenInf.Tokki2, item.SelectedHokenInf.Tokki3, item.SelectedHokenInf.Tokki4, item.SelectedHokenInf.Tokki5);
            if (!checkIsValidHokenDetail.Result)
            {
                return checkIsValidHokenDetail;
            }
            var checkCHKHokno_Fnc = CHKHokno_Fnc(item.SelectedHokenInf.HokensyaNo, item.SelectedHokenInf.HokenNo, item.SelectedHokenInf.Houbetu, item.IsHaveSelectedHokenMst, item.SelectedHokenMst.Houbetu, item.SelectedHokenMst.HokenNo, item.SelectedHokenMst.CheckDigit, ptBirthday, item.SelectedHokenMst.AgeStart, item.SelectedHokenMst.AgeEnd);
            if (!checkCHKHokno_Fnc.Result)
            {
                return checkCHKHokno_Fnc;
            }
            if (string.IsNullOrEmpty(item.SelectedHokenInf.HokensyaNo))
            {
                var paramsMessage = new string[] { "保険者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokensyaNoNull);
            }
            if (item.SelectedHokenInf.HokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenNoEquals0);
            }
            if (Int32.Parse(item.SelectedHokenInf.HokensyaNo) == 0)
            {
                var paramsMessage = new string[] { "保険者番号は 0 〜 9 の範囲で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokensyaNoEquals0);
            }
            // 記号
            if (item.SelectedHokenInf.HokensyaNo.Length == 8 && item.SelectedHokenInf.HokensyaNo.Trim().StartsWith("39"))
            {
                if (!string.IsNullOrEmpty(item.SelectedHokenInf.Kigo)
                    && !string.IsNullOrEmpty(item.SelectedHokenInf.Kigo.Trim(' '))) //Trim only half-size space
                {
                    var paramsMessage = new string[] { "後期高齢者の", "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00150, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokensyaNoLength8StartWith39);
                }
            }
            else
            {
                if (item.SelectedHokenInf.HokensyaMst.IsKigoNa == 0 && (string.IsNullOrEmpty(item.SelectedHokenInf.Kigo)
                    || string.IsNullOrEmpty(item.SelectedHokenInf.Kigo.Trim(' '))))
                {
                    var paramsMessage = new string[] { "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKigoNull);
                }
            }
            if (string.IsNullOrEmpty(item.SelectedHokenInf.Bango)
                    || string.IsNullOrEmpty(item.SelectedHokenInf.Bango.Trim(' '))) //Trim only half-size space
            {
                var paramsMessage = new string[] { "被保険者証番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidBangoNull);
            }
            if (item.SelectedHokenPattern.HokenKbn == 0)
            {
                var paramsMessage = new string[] { "本人家族区分" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenKbnEquals0);
            }
            var checkIsValidYukoKigen = IsValidYukoKigen(item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate);
            if (!checkIsValidYukoKigen.Result)
            {
                return checkIsValidYukoKigen;
            }
            var checkIsValidTokkurei = IsValidTokkurei(ptBirthday, sinDate, item.SelectedHokenInf.TokureiYm1, item.SelectedHokenInf.TokureiYm2, item.SelectedHokenInf.HokensyaNo, item.SelectedHokenInf.IsShahoOrKokuho, item.SelectedHokenInf.IsExpirated);
            if (!checkIsValidTokkurei.Result)
            {
                return checkIsValidTokkurei;
            }
            var checkMessageIsValidConfirmDateAgeCheck = IsValidConfirmDateAgeCheck(item.SelectedHokenInf.IsAddNew, item.SelectedHokenInf.IsExpirated, item.SelectedHokenInf.IsShahoOrKokuho, item.SelectedHokenInf.HokensyaNo, item.SelectedHokenInf.ConfirmDate, ptBirthday, sinDate, hpId);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateAgeCheck))
            {
                return new ValidateInsuranceItem(false, checkMessageIsValidConfirmDateAgeCheck, ValidateInsuranceStatus.InvalidConfirmDateAgeCheck);
            }
            // check valid hokenmst date
            var checkIsValidHokenMstDate = IsValidHokenMstDate(item.SelectedHokenMst.StartDate, item.SelectedHokenMst.EndDate, sinDate, item.SelectedHokenMst.DisplayTextMaster);
            if (!checkIsValidHokenMstDate.Result)
            {
                return checkIsValidHokenMstDate;
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidHokenNashi(int hpId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
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
            return new ValidateInsuranceItem(true, string.Empty, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidYukoKigen(int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            int yukoFromDate = selectedHokenInfStartDate;
            int yukoToDate = selectedHokenInfEndDate;
            if (yukoFromDate != 0 && yukoToDate != 0 && yukoFromDate > yukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidYukoKigen);
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidHokenDetail(int hpId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue1);
                }
                if (!string.IsNullOrEmpty(tokki2Value) && tokki2Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki2Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue21);
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue31);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue41);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue51);
                }
            }
            if (!string.IsNullOrEmpty(tokki2Value))
            {
                if (!_isValidLengthTokki(tokki2Value))
                {
                    var paramsMessage = new string[] { "特記事項２", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue2);
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue23);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue24);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue25);
                }
            }
            if (!string.IsNullOrEmpty(tokki3Value))
            {
                if (!_isValidLengthTokki(tokki3Value))
                {
                    var paramsMessage = new string[] { "特記事項３", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue3);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue34);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue35);
                }
            }
            if (!string.IsNullOrEmpty(tokki4Value))
            {
                if (!_isValidLengthTokki(tokki4Value))
                {
                    var paramsMessage = new string[] { "特記事項４", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue4);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki4Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue45);
                }
            }
            if (!string.IsNullOrEmpty(tokki5Value) && !_isValidLengthTokki(tokki5Value))
            {
                var paramsMessage = new string[] { "特記事項５", "2文字" };
                message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkiValue5);
            }
            return new ValidateInsuranceItem(true, string.Empty, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem CHKHokno_Fnc(string hokenSyaNo, int hokenNo, string houbetu, bool isHaveSelectedHokenMst, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd)
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0);
                }
            }
            //保険番号入力あり
            else
            {
                if (hokenNo == 0)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenNoEquals0);
                }
                if (!isHaveSelectedHokenMst)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenNoHaveHokenMst);
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHoubetu);
                }
                //チェックデジット
                if (sHokenMstCheckDegit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(hokenSyaNo)))
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidCheckDigitEquals1);
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
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidCheckAgeHokenMst);
                    }
                }
            }
            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidTokkurei(int ptBirthday, int sinDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, string hokenSyaNo, bool isShahoOrKokuho, bool isExpirated)
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
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokkurei);
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
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

        private ValidateInsuranceItem IsValidHokenMstDate(int startDate, int endDate, int sinDate, string displayTextMaster)
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenMstStartDate);
                }
                if (hokenEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(hokenEndDate) + ")", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidHokenMstEndDate);
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidRodo(string rodoBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var rousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            if (rousaiReceder == 1)
            {
                if (string.IsNullOrEmpty(rodoBango))
                {
                    var paramsMessage = new string[] { "労働保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRodoBangoNull);
                }
                if (rodoBango.Trim().Length != 14)
                {
                    var paramsMessage = new string[] { "労働保険番号", " 14桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRodoBangoLengthNotEquals14);
                }
            }
            var checkCommonCheckForRosai = CommonCheckForRosai(hokenKbn, listRousaiTenkis, rousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkCommonCheckForRosai.Result)
            {
                return checkCommonCheckForRosai;
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidNenkin(string nenkinBago, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufu = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufu == 1)
            {
                if (string.IsNullOrEmpty(nenkinBago))
                {
                    var paramsMessage = new string[] { "年金証書番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidNenkinBangoNull);
                }
                if (nenkinBago.Trim().Length != 9)
                {
                    var paramsMessage = new string[] { "年金証書番号", " 9桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidNenkinBangoLengthNotEquals9);
                }
            }
            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            var checkCommonCheckForRosai = CommonCheckForRosai(hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkCommonCheckForRosai.Result)
            {
                return checkCommonCheckForRosai;
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidKenko(string kenkoKanriBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufuValidate = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufuValidate == 1)
            {
                if (string.IsNullOrEmpty(kenkoKanriBango))
                {
                    var paramsMessage = new string[] { "健康管理手帳番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKenkoKanriBangoNull);
                }
                if (kenkoKanriBango.Trim().Length != 13)
                {
                    var paramsMessage = new string[] { "健康管理手帳番号", " 13桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKenkoKanriBangoLengthNotEquals13);
                }
            }

            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            var checkCommonCheckForRosai = CommonCheckForRosai(hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkCommonCheckForRosai.Result)
            {
                return checkCommonCheckForRosai;
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidJibai(List<RousaiTenkiModel> listRousaiTenkis)
        {
            var message = "";
            if (listRousaiTenkis != null && listRousaiTenkis.Count > 0)
            {
                var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                if (rousaiTenkiList.FirstOrDefault()?.RousaiTenkiTenki <= 0)
                {
                    var paramsMessage = new string[] { "転帰事由" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiDefaultRow);
                }
                else
                {
                    string errorMsg = string.Empty;
                    foreach (var rousaiTenki in rousaiTenkiList)
                    {
                        if (rousaiTenki.RousaiTenkiTenki <= 0)
                        {
                            errorMsg = "転帰事由を入力してください。";
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        //Check duplicate end date
                        foreach (var rousaiTenki in rousaiTenkiList)
                        {
                            if (rousaiTenkiList.Count(p => p.RousaiTenkiEndDate == rousaiTenki.RousaiTenkiEndDate) > 1)
                            {
                                errorMsg = "有効期限が重複しています。";
                                break;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        var paramsMessage = new string[] { errorMsg };
                        message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiData);
                    }
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem CommonCheckForRosai(int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int rosaiReceden, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew)
        {
            var message = "";
            if (hokenKbn == 11 || hokenKbn == 12)
            {
                if (listRousaiTenkis.Count > 0)
                {
                    var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                    var itemFirst = rousaiTenkiList.FirstOrDefault();
                    // Check Rousai tenki grid default row
                    if (itemFirst != null && (itemFirst.RousaiTenkiSinkei <= 0 && itemFirst.RousaiTenkiTenki <= 0 && (itemFirst.RousaiTenkiEndDate == 0 || itemFirst.RousaiTenkiEndDate == 999999)))
                    {
                        var paramsMessage = new string[] { "新継再別" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiDefaultRow);
                    }
                    else
                    {
                        string errorMsg = string.Empty;
                        foreach (var rousaiTenki in rousaiTenkiList)
                        {
                            if (rousaiTenki.RousaiTenkiSinkei <= 0)
                            {
                                errorMsg = "新継再別を入力してください。";
                                break;
                            }
                            else if (rousaiTenki.RousaiTenkiTenki <= 0)
                            {
                                errorMsg = "転帰事由を入力してください。";
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            //Check duplicate end date
                            foreach (var rousaiTenki in rousaiTenkiList)
                            {
                                if (rousaiTenkiList.Count(p => p.RousaiTenkiEndDate == rousaiTenki.RousaiTenkiEndDate) > 1)
                                {
                                    errorMsg = "有効期限が重複しています。";
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            var paramsMessage = new string[] { errorMsg };
                            message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiData);
                        }
                    }
                }
                if (rosaiReceden == 1)
                {
                    if (sHokenInfRousaiSaigaiKbn != 1 && sHokenInfRousaiSaigaiKbn != 2)
                    {
                        var paramsMessage = new string[] { "災害区分" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiSaigaiKbn);
                    }

                    if (sHokenInfRousaiSyobyoDate == 0)
                    {
                        var paramsMessage = new string[] { "傷病年月日" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiSyobyoDateEquals0);
                    }
                }
            }
            else if (hokenKbn == 13 && string.IsNullOrEmpty(sHokenInfRousaiSyobyoCd))
            {
                var paramsMessage = new string[] { "傷病コード" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiSyobyoDateEquals0);
            }

            // 労災・療養期間ﾁｪｯｸ
            int rousaiRyoyoStartDate = sHokenInfRyoyoStartDate;
            int rousaiRyoyoEndDate = sHokenInfRyoyoEndDate;
            if (rousaiRyoyoStartDate != 0 && rousaiRyoyoEndDate != 0 && rousaiRyoyoStartDate > rousaiRyoyoEndDate)
            {
                var paramsMessage = new string[] { "労災療養終了日", "労災療養開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRousaiRyoyoDate);
            }

            // 労災・有効期限ﾁｪｯｸ
            int rosaiYukoFromDate = sHokenInfStartDate;
            int rosaiYukoToDate = sHokenInfEndDate;
            if (rosaiYukoFromDate != 0 && rosaiYukoToDate != 0 && rosaiYukoFromDate > rosaiYukoToDate)
            {
                var paramsMessage = new string[] { "労災有効終了日", "労災有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidRosaiYukoDate);
            }
            // 労災・期限切れﾁｪｯｸ(有効保険の場合のみ)
            if (!DataChkFn10(sinDate, sHokenInfStartDate, sHokenInfEndDate, isAddNew))
            {
                var paramsMessage = new string[] { "労災保険", "無視する", "戻る" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidCheckHokenInfDate);
            }

            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private bool DataChkFn10(int sinDate, int startDate, int endDate, bool isAddNew)
        {
            int fToDay = sinDate;
            if (isAddNew && ((endDate > 0 && endDate < fToDay)
                    || (startDate > 0 && startDate > fToDay)))
            {
                return false;
            }
            return true;
        }

        private ValidateInsuranceItem IsValidKohi(bool isKohiModdel, bool isHokenMstModel, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenMstIsIsFutansyaNoCheck, int hokenMstIsIsJyukyusyaNoCheck, int hokenMstIsIsTokusyuNoCheck, int hokenMstModelStartDate, int hokenMstModelEndDate, string hokenMstDisplayText, int numberKohi, int sinDate, bool isAddNew)
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel4);
                }
            }

            if (isHokenMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty4);
                }
            }

            // check validate data
            var checkValidateData = CheckValidData(numberMessage, numberKohi, futansyaNo, hokenMstIsIsFutansyaNoCheck, jyukyusyaNo, hokenMstIsIsJyukyusyaNoCheck, tokusyuNo, hokenMstIsIsTokusyuNoCheck);
            if (!checkValidateData.Result)
            {
                return checkValidateData;
            }

            // check kohi date
            var checkKohiDate = CheckKohiDate(startDate, endDate, numberMessage, numberKohi);
            if (!checkKohiDate.Result)
            {
                return checkKohiDate;
            }

            // check confirm date kohi
            var checkMessageIsValidConfirmDateKohi = IsValidConfirmDateKohi(confirmDate, numberMessage, sinDate, isAddNew, numberKohi);
            if (!checkMessageIsValidConfirmDateKohi.Result)
            {
                return checkMessageIsValidConfirmDateKohi;
            }

            // master date kohi IsValidMasterDateKohi
            var checkMasterDateKohi = CheckMasterDateKohi(hokenMstModelStartDate, hokenMstModelEndDate, sinDate, numberMessage, hokenMstDisplayText, numberKohi);
            if (!checkMasterDateKohi.Result)
            {
                return checkMasterDateKohi;
            }

            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem CheckValidData(string numberMessage, int numberKohi, string futansyaNo, int hokenMstIsIsFutansyaNoCheck, string jyukyusyaNo, int hokenMstIsIsJyukyusyaNoCheck, string tokusyuNo, int hokenMstIsIsTokusyuNoCheck)
        {
            var message = "";
            if (string.IsNullOrEmpty(futansyaNo.Trim())
                && hokenMstIsIsFutansyaNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty4);
                }
            }
            else if (string.IsNullOrEmpty(jyukyusyaNo.Trim())
                && hokenMstIsIsJyukyusyaNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo4);
                }
            }
            else if (string.IsNullOrEmpty(tokusyuNo.Trim())
                && hokenMstIsIsTokusyuNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokusyuNo1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokusyuNo2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokusyuNo3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidTokusyuNo4);
                }
            }
            if (!string.IsNullOrEmpty(futansyaNo) && Int32.Parse(futansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNo01);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNo02);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNo03);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNo04);
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }
        private ValidateInsuranceItem CheckKohiDate(int startDate, int endDate, string numberMessage, int numberKohi)
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate4);
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem CheckMasterDateKohi(int hokenMstModelStartDate, int hokenMstModelEndDate, int sinDate, string numberMessage, string hokenMstDisplayText, int numberKohi)
        {
            var message = "";
            if (hokenMstModelStartDate > sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelStartDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate4);
                }
            }
            if (hokenMstModelEndDate < sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelEndDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate4);
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidConfirmDateKohi(int confirmDate, string numberMessage, int sinDate, bool isAddNew, int numberKohi)
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
                    return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
                }
                else
                {
                    var paramsMessage = new string[] { "公費" + numberMessage, "受給者証等" };
                    message = String.Format(ErrorMessage.MessageType_mChk00030, paramsMessage);
                }
            }
            if (!String.IsNullOrEmpty(message))
            {
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate4);
                }
            }
            else
            {
                return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
            }
        }

        private ValidateInsuranceItem IsvalidKohiAll(bool isKohiEmptyModel1, bool isKohiEmptyModel2, bool isKohiEmptyModel3, bool isKohiEmptyModel4, KohiInfModel kohi1, KohiInfModel kohi2, KohiInfModel kohi3, KohiInfModel kohi4)
        {
            var message = "";
            if (isKohiEmptyModel2 && isKohiEmptyModel1)
            {
                var paramsMessage = new string[] { "公費１" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmpty21);
            }

            if (isKohiEmptyModel3)
            {
                if (isKohiEmptyModel1)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmpty31);
                }

                if (isKohiEmptyModel2)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmpty32);
                }
            }

            if (isKohiEmptyModel4)
            {
                if (isKohiEmptyModel1)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmpty41);
                }

                if (isKohiEmptyModel2)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmpty42);
                }

                if (isKohiEmptyModel3)
                {
                    var paramsMessage = new string[] { "公費３" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmpty43);
                }
            }
            // check duplicate 1
            if (!isKohiEmptyModel1 && ((!isKohiEmptyModel2 && (kohi1.FutansyaNo == kohi2.FutansyaNo && kohi1.JyukyusyaNo == kohi2.JyukyusyaNo && kohi1.StartDate == kohi2.StartDate && kohi1.EndDate == kohi2.EndDate && kohi1.ConfirmDate == kohi2.ConfirmDate))
                   || (!isKohiEmptyModel3 && (kohi1.FutansyaNo == kohi3.FutansyaNo && kohi1.JyukyusyaNo == kohi3.JyukyusyaNo && kohi1.StartDate == kohi3.StartDate && kohi1.EndDate == kohi3.EndDate && kohi1.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel4 && (kohi1.FutansyaNo == kohi4.FutansyaNo && kohi1.JyukyusyaNo == kohi4.JyukyusyaNo && kohi1.StartDate == kohi4.StartDate && kohi1.EndDate == kohi4.EndDate && kohi1.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi1);
            }
            // check duplicate 2
            if (!isKohiEmptyModel2 && ((!isKohiEmptyModel1 && (kohi1.FutansyaNo == kohi2.FutansyaNo && kohi1.JyukyusyaNo == kohi2.JyukyusyaNo && kohi1.StartDate == kohi2.StartDate && kohi1.EndDate == kohi2.EndDate && kohi1.ConfirmDate == kohi2.ConfirmDate))
                   || (!isKohiEmptyModel3 && (kohi2.FutansyaNo == kohi3.FutansyaNo && kohi2.JyukyusyaNo == kohi3.JyukyusyaNo && kohi2.StartDate == kohi3.StartDate && kohi2.EndDate == kohi3.EndDate && kohi2.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel4 && (kohi2.FutansyaNo == kohi4.FutansyaNo && kohi2.JyukyusyaNo == kohi4.JyukyusyaNo && kohi2.StartDate == kohi4.StartDate && kohi2.EndDate == kohi4.EndDate && kohi2.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi2);
            }
            // check duplicate 3
            if (!isKohiEmptyModel3 && ((!isKohiEmptyModel1 && (kohi1.FutansyaNo == kohi3.FutansyaNo && kohi1.JyukyusyaNo == kohi3.JyukyusyaNo && kohi1.StartDate == kohi3.StartDate && kohi1.EndDate == kohi3.EndDate && kohi1.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel2 && (kohi2.FutansyaNo == kohi3.FutansyaNo && kohi2.JyukyusyaNo == kohi3.JyukyusyaNo && kohi2.StartDate == kohi3.StartDate && kohi2.EndDate == kohi3.EndDate && kohi2.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel4 && (kohi3.FutansyaNo == kohi4.FutansyaNo && kohi3.JyukyusyaNo == kohi4.JyukyusyaNo && kohi3.StartDate == kohi4.StartDate && kohi3.EndDate == kohi4.EndDate && kohi3.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi3);
            }
            // check duplicate 4
            if (!isKohiEmptyModel4 && ((!isKohiEmptyModel1 && (kohi1.FutansyaNo == kohi4.FutansyaNo && kohi1.JyukyusyaNo == kohi4.JyukyusyaNo && kohi1.StartDate == kohi4.StartDate && kohi1.EndDate == kohi4.EndDate && kohi1.ConfirmDate == kohi4.ConfirmDate))
                   || (!isKohiEmptyModel2 && (kohi2.FutansyaNo == kohi4.FutansyaNo && kohi2.JyukyusyaNo == kohi4.JyukyusyaNo && kohi2.StartDate == kohi4.StartDate && kohi2.EndDate == kohi4.EndDate && kohi2.ConfirmDate == kohi4.ConfirmDate))
                   || (!isKohiEmptyModel3 && (kohi3.FutansyaNo == kohi4.FutansyaNo && kohi3.JyukyusyaNo == kohi4.JyukyusyaNo && kohi3.StartDate == kohi4.StartDate && kohi3.EndDate == kohi4.EndDate && kohi3.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi4);
            }

            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidKohiNo_Fnc(bool isKohiModel, bool isKohiMstModel, int hokenNo, string futansyaNo, string tokusyuNo, int hokenMstIsIsJyukyusyaNoCheck, int hokenMstIsIsFutansyaNoCheck, int hokenMstJyukyuCheckDigit, int hokenMstCheckDigit, string hokenMstHoubetu, string jyukyusyaNo, int hokenMstAgeStartDate, int hokenMstAgeEndDate, int numberKohi, int ptBirthday)
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel4);
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
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty1);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty2);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty3);
                }
                else
                {
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty4);
                }
            }
            if (hokenNo != 0)
            {
                if (string.IsNullOrEmpty(futansyaNo)
                    && hokenMstIsIsFutansyaNoCheck == 1)
                {
                    var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    if (numberKohi == 1)
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty1);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty2);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty3);
                    }
                    else
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty4);
                    }
                }
                //法別番号のチェック
                if (hokenMstIsIsFutansyaNoCheck == 1)
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
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT1);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT2);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT3);
                        }
                        else
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT4);
                        }
                    }
                    //チェックデジット
                    if (hokenMstCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(futansyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo1);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo2);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo3);
                        }
                        else
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo4);
                        }
                    }
                    if (hokenMstIsIsJyukyusyaNoCheck == 1 && hokenMstJyukyuCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(jyukyusyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo1);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo2);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo3);
                        }
                        else
                        {
                            return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo4);
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
                                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge1);
                            }
                            else if (numberKohi == 2)
                            {
                                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge2);
                            }
                            else if (numberKohi == 3)
                            {
                                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge3);
                            }
                            else
                            {
                                return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge4);
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
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull1);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull2);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull3);
                    }
                    else
                    {
                        return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull4);
                    }
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }

        private bool CheckPatternDuplicate(InsuranceModel pattern, InsuranceModel item)
        {
            if (pattern.IsEmptyHoken != item.IsEmptyHoken)
                return false;

            if (!pattern.IsEmptyHoken && !item.IsEmptyHoken && pattern.HokenInf.HokenId != item.HokenInf.HokenId)
                return false;

            if (pattern.IsEmptyKohi1 != item.IsEmptyKohi1)
                return false;

            if (!pattern.IsEmptyKohi1 && !item.IsEmptyKohi1 && pattern.Kohi1.HokenId != item.Kohi1.HokenId)
                return false;

            if (pattern.IsEmptyKohi2 != item.IsEmptyKohi2)
                return false;

            if (!pattern.IsEmptyKohi2 && !item.IsEmptyKohi2 && pattern.Kohi2.HokenId != item.Kohi2.HokenId)
                return false;

            if (pattern.IsEmptyKohi3 != item.IsEmptyKohi3)
                return false;

            if (!pattern.IsEmptyKohi3 && !item.IsEmptyKohi3 && pattern.Kohi3.HokenId != item.Kohi3.HokenId)
                return false;

            if (pattern.IsEmptyKohi4 != item.IsEmptyKohi4)
                return false;

            if (!pattern.IsEmptyKohi4 && !item.IsEmptyKohi4 && pattern.Kohi4.HokenId != item.Kohi4.HokenId)
                return false;
            return true;
        }

        private ValidateInsuranceItem CheckAge(ValidateInsuranceInputData inputData, List<InsuranceModel> listHokenPattern, List<HokenInfModel> listHokenInf)
        {
            var messageError = "";
            if (inputData.SinDate >= 20080401 && (listHokenPattern.Count > 0 && listHokenInf.Count > 0))
            {
                var patternHokenOnlyCheckAge = listHokenPattern.Where(pattern => pattern.IsDeleted == 0 && !pattern.IsExpirated);
                int age = CIUtil.SDateToAge(inputData.PtBirthday, inputData.SinDate);
                // hoken exist in at least 1 pattern
                var inUsedHokens = listHokenInf.Where(hoken => hoken.HokenId > 0 && hoken.IsDeleted == 0 && !hoken.IsExpirated
                                                            && patternHokenOnlyCheckAge.Any(pattern => pattern.HokenInf.HokenId == hoken.HokenId));

                var elderHokenQuery = inUsedHokens.Where(hoken => hoken.EndDate >= inputData.SinDate
                                                                        && hoken.HokensyaNo != null && hoken.HokensyaNo != ""
                                                                        && hoken.HokensyaNo.Length == 8 && hoken.HokensyaNo.StartsWith("39"));
                if (elderHokenQuery != null)
                {
                    if (age >= 75 && !elderHokenQuery.Any())
                    {
                        var paramsMessage75 = new string[] { "後期高齢者保険が入力されていません。", "保険者証" };
                        messageError = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage75);
                        return new ValidateInsuranceItem(false, messageError, ValidateInsuranceStatus.InvalidWarningAge75);
                    }
                    else if (age < 65 && elderHokenQuery.Any())
                    {
                        var paramsMessage65 = new string[] { "後期高齢者保険の対象外の患者に、後期高齢者保険が登録されています。", "保険者証" };
                        messageError = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage65);
                        return new ValidateInsuranceItem(true, messageError, ValidateInsuranceStatus.InvalidWarningAge65);
                    }
                }
            }
            return new ValidateInsuranceItem(true, messageError, ValidateInsuranceStatus.Successed);
        }

        private ValidateInsuranceItem IsValidMaruchoOnly(ValidateInsuranceModel item)
        {
            var message = "";
            if (item.IsHaveSelectedHokenPattern)
            {
                bool isEmptyHoken = true;
                if (item.IsHaveSelectedHokenInf)
                {
                    isEmptyHoken = !item.IsHaveSelectedHokenInf
                        || (item.SelectedHokenPattern.HokenKbn == 1 && item.SelectedHokenInf.Houbetu == HokenConstant.HOUBETU_NASHI);
                }
                bool isEmptyKohi1 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi1 && item.SelectedHokenPattern.Kohi1.IsHaveKohiMst)
                {
                    //2:マル長
                    isEmptyKohi1 = item.SelectedHokenPattern.Kohi1.HokenMstModel.HokenSbtKbn == 2;
                }
                bool isEmptyKohi2 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi2 && item.SelectedHokenPattern.Kohi2.IsHaveKohiMst)
                {
                    //2:マル長
                    isEmptyKohi2 = item.SelectedHokenPattern.Kohi2.HokenMstModel.HokenSbtKbn == 2;
                }
                bool isEmptyKohi3 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi3 && item.SelectedHokenPattern.Kohi3.IsHaveKohiMst)
                {
                    //2:マル長
                    isEmptyKohi3 = item.SelectedHokenPattern.Kohi3.HokenMstModel.HokenSbtKbn == 2;
                }
                bool isEmptyKohi4 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi4 && item.SelectedHokenPattern.Kohi4.IsHaveKohiMst)
                {
                    //2:マル長
                    isEmptyKohi1 = item.SelectedHokenPattern.Kohi4.HokenMstModel.HokenSbtKbn == 2;
                }
                if (!item.SelectedHokenPattern.IsAddNew && isEmptyHoken && isEmptyKohi1 && isEmptyKohi2 && isEmptyKohi3 && isEmptyKohi4)
                {
                    var paramsMessage65 = new string[] { "保険組合せ", "情報" };
                    message = String.Format(ErrorMessage.MessageType_mInp00011, paramsMessage65);
                    return new ValidateInsuranceItem(false, message, ValidateInsuranceStatus.InvalidMaruchoOnly);
                }
            }
            return new ValidateInsuranceItem(true, message, ValidateInsuranceStatus.Successed);
        }
        private ValidateInsuranceModel ConvertModelToDto(ValidateInsuranceDto item, int sinDate)
        {
            // get hokenMst
            var hokenMst = _patientInforRepository.GetHokenMstByInfor(item.HokenInf.HokenNo, item.HokenInf.HokenEdaNo, sinDate);

            //get FindHokensyaMstByNoNotrack
            string houbetuNo = string.Empty;
            string hokensyaNoSearch = string.Empty;
            CIUtil.GetHokensyaHoubetu(item.HokenInf.HokensyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
            var hokenSyaMst = _patientInforRepository.GetHokenSyaMstByInfor(item.HokenInf.HpId, houbetuNo, hokensyaNoSearch);
            // convert hokenInfDto To HokenInfModel
            var hokenInfModel = new HokenInfModel(
                                            item.HokenInf.HpId,
                                            item.HokenInf.PtId,
                                            item.HokenInf.HokenId,
                                            item.HokenInf.SeqNo,
                                            item.HokenInf.HokenNo,
                                            item.HokenInf.HokenEdaNo,
                                            item.HokenInf.HokenKbn,
                                            item.HokenInf.HokensyaNo ?? string.Empty,
                                            item.HokenInf.Kigo ?? string.Empty,
                                            item.HokenInf.Bango ?? string.Empty,
                                            item.HokenInf.EdaNo ?? string.Empty,
                                            item.HokenInf.HonkeKbn,
                                            item.HokenInf.StartDate,
                                            item.HokenInf.EndDate,
                                            item.HokenInf.SikakuDate,
                                            item.HokenInf.KofuDate,
                                            item.HokenInf.ConfirmDate,
                                            item.HokenInf.KogakuKbn,
                                            item.HokenInf.TasukaiYm,
                                            item.HokenInf.TokureiYm1,
                                            item.HokenInf.TokureiYm2,
                                            item.HokenInf.GenmenKbn,
                                            item.HokenInf.GenmenRate,
                                            item.HokenInf.GenmenGaku,
                                            item.HokenInf.SyokumuKbn,
                                            item.HokenInf.KeizokuKbn,
                                            item.HokenInf.Tokki1 ?? string.Empty,
                                            item.HokenInf.Tokki2 ?? string.Empty,
                                            item.HokenInf.Tokki3 ?? string.Empty,
                                            item.HokenInf.Tokki4 ?? string.Empty,
                                            item.HokenInf.Tokki5 ?? string.Empty,
                                            item.HokenInf.RousaiKofuNo ?? string.Empty,
                                            item.HokenInf.RousaiRoudouCd ?? string.Empty,
                                            item.HokenInf.RousaiSaigaiKbn,
                                            item.HokenInf.RousaiKantokuCd ?? string.Empty,
                                            item.HokenInf.RousaiSyobyoDate,
                                            item.HokenInf.RyoyoStartDate,
                                            item.HokenInf.RyoyoEndDate,
                                            item.HokenInf.RousaiSyobyoCd ?? string.Empty,
                                            item.HokenInf.RousaiJigyosyoName ?? string.Empty,
                                            item.HokenInf.RousaiPrefName ?? string.Empty,
                                            item.HokenInf.RousaiCityName ?? string.Empty,
                                            item.HokenInf.RousaiReceCount,
                                            string.Empty,
                                            string.Empty,
                                            string.Empty,
                                            sinDate,
                                            item.HokenInf.JibaiHokenName ?? string.Empty,
                                            item.HokenInf.JibaiHokenTanto ?? string.Empty,
                                            item.HokenInf.JibaiHokenTel ?? string.Empty,
                                            item.HokenInf.JibaiJyusyouDate,
                                            item.HokenInf.Houbetu ?? string.Empty,
                                            new List<ConfirmDateModel>(),
                                            item.HokenInf.ListRousaiTenki,
                                            item.HokenInf.IsReceKisaiOrNoHoken,
                                            item.HokenInf.IsDeleted,
                                            Mapper.Map(hokenMst, new HokenMstModel(), (src, dest) =>
                                            {
                                                return dest;
                                            }),
                                            hokenSyaMst ?? new HokensyaMstModel(),
                                            item.HokenInf.IsAddNew,
                                            false
            );

            // Kohi1
            // get hokenMst Kohi1
            var hokenMstKohi1 = _patientInforRepository.GetHokenMstByInfor(item.Kohi1.HokenNo, item.Kohi1.HokenEdaNo, sinDate);
            var kohi1 = new KohiInfModel(item.Kohi1.ConfirmDateList, item.Kohi1.FutansyaNo, item.Kohi1.JyukyusyaNo, item.Kohi1.HokenId, item.Kohi1.StartDate, item.Kohi1.EndDate, item.Kohi1.ConfirmDate, item.Kohi1.Rate, item.Kohi1.GendoGaku, item.Kohi1.SikakuDate, item.Kohi1.KofuDate, item.Kohi1.TokusyuNo, item.Kohi1.HokenSbtKbn, item.Kohi1.Houbetu, hokenMstKohi1, item.Kohi1.HokenNo, item.Kohi1.HokenEdaNo, item.Kohi1.PrefNo, item.Kohi1.SinDate, hokenMstKohi1 != null, item.Kohi1.IsDeleted, 0, item.Kohi1.IsAddNew);

            // Kohi2
            // get hokenMst Kohi2
            var hokenMstKohi2 = _patientInforRepository.GetHokenMstByInfor(item.Kohi2.HokenNo, item.Kohi2.HokenEdaNo, sinDate);
            var kohi2 = new KohiInfModel(item.Kohi2.ConfirmDateList, item.Kohi2.FutansyaNo, item.Kohi2.JyukyusyaNo, item.Kohi2.HokenId, item.Kohi2.StartDate, item.Kohi2.EndDate, item.Kohi2.ConfirmDate, item.Kohi2.Rate, item.Kohi2.GendoGaku, item.Kohi2.SikakuDate, item.Kohi2.KofuDate, item.Kohi2.TokusyuNo, item.Kohi2.HokenSbtKbn, item.Kohi2.Houbetu, hokenMstKohi2, item.Kohi2.HokenNo, item.Kohi2.HokenEdaNo, item.Kohi2.PrefNo, item.Kohi2.SinDate, hokenMstKohi2 != null, item.Kohi2.IsDeleted, 0, item.Kohi1.IsAddNew);

            // Kohi3
            // get hokenMst Kohi3
            var hokenMstKohi3 = _patientInforRepository.GetHokenMstByInfor(item.Kohi3.HokenNo, item.Kohi3.HokenEdaNo, sinDate);
            var kohi3 = new KohiInfModel(item.Kohi3.ConfirmDateList, item.Kohi3.FutansyaNo, item.Kohi3.JyukyusyaNo, item.Kohi3.HokenId, item.Kohi3.StartDate, item.Kohi3.EndDate, item.Kohi3.ConfirmDate, item.Kohi3.Rate, item.Kohi3.GendoGaku, item.Kohi3.SikakuDate, item.Kohi3.KofuDate, item.Kohi3.TokusyuNo, item.Kohi3.HokenSbtKbn, item.Kohi3.Houbetu, hokenMstKohi3, item.Kohi3.HokenNo, item.Kohi3.HokenEdaNo, item.Kohi3.PrefNo, item.Kohi3.SinDate, hokenMstKohi3 != null, item.Kohi3.IsDeleted, 0, item.Kohi1.IsAddNew);

            // Kohi4
            // get hokenMst Kohi4
            var hokenMstKohi4 = _patientInforRepository.GetHokenMstByInfor(item.Kohi4.HokenNo, item.Kohi4.HokenEdaNo, sinDate);
            var kohi4 = new KohiInfModel(item.Kohi4.ConfirmDateList, item.Kohi4.FutansyaNo, item.Kohi4.JyukyusyaNo, item.Kohi4.HokenId, item.Kohi4.StartDate, item.Kohi4.EndDate, item.Kohi4.ConfirmDate, item.Kohi4.Rate, item.Kohi4.GendoGaku, item.Kohi4.SikakuDate, item.Kohi4.KofuDate, item.Kohi4.TokusyuNo, item.Kohi4.HokenSbtKbn, item.Kohi4.Houbetu, hokenMstKohi4, item.Kohi4.HokenNo, item.Kohi4.HokenEdaNo, item.Kohi4.PrefNo, item.Kohi4.SinDate, hokenMstKohi4 != null, item.Kohi4.IsDeleted, 0, item.Kohi1.IsAddNew);

            var itemInsurance = new InsuranceModel(item.HpId, item.PtId, item.PtBirthday, item.SeqNo, item.HokenSbtCd, item.HokenPid, item.HokenKbn, sinDate, item.HokenMemo, hokenInfModel, kohi1, kohi2, kohi3, kohi4, item.IsDeleted, item.StartDate, item.EndDate, item.IsAddNew);

            var itemModel = new ValidateInsuranceModel(hokenInfModel, itemInsurance, hokenInfModel.HokenMst);

            return itemModel;
        }
    }
}