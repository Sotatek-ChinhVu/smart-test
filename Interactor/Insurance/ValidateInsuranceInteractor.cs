using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidateInsurance;

namespace Interactor.Insurance
{
    public class ValidateInsuranceInteractor: IValidateInsuranceInputPort
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
                // check validate Input
                var checkValidInput = CheckValidateInputData(inputData);
                if (!checkValidInput.Result)
                {
                    return checkValidInput;
                }

                var index = 0;
                var listHokenPattern = new List<SelectedHokenPattern>();
                var listHokenInf = new List<SelectedHokenInf>();
                if (inputData.ListDataModel.Any())
                {
                    foreach (var item in inputData.ListDataModel)
                    {
                        switch (item.SelectedHokenPattern.HokenKbn)
                        {
                            case 0:
                                var checkMessageIsValidJihi = IsValidJihi(item.SelectedHokenInf.HokenNo);
                                if (!String.IsNullOrEmpty(checkMessageIsValidJihi))
                                {
                                    return new ValidateInsuranceOutputData(false, checkMessageIsValidJihi, ValidateInsuranceStatus.InvalidJihiSelectedHokenInfHokenNoEquals0, index);
                                }
                                // ignore
                                break;
                            // 社保
                            case 1:
                                var checkIsValidShaho = IsValidShaho(item, index, inputData.HpId, inputData.SinDate, inputData.PtBirthday);
                                if (!checkIsValidShaho.Result)
                                {
                                    return checkIsValidShaho;
                                }
                                break;
                            // 国保
                            case 2:
                                var checkIsValidKokuho = IsValidShaho(item, index, inputData.HpId, inputData.SinDate, inputData.PtBirthday);
                                if (!checkIsValidKokuho.Result)
                                {
                                    return checkIsValidKokuho;
                                }
                                break;
                            // 労災(短期給付)	
                            case 11:
                                var checkIsValidRodo = IsValidRodo(index, item.SelectedHokenInf.RodoBango, item.SelectedHokenPattern.HokenKbn, item.SelectedHokenInf.ListRousaiTenki, item.SelectedHokenInf.RousaiSaigaiKbn, item.SelectedHokenInf.RousaiSyobyoDate, item.SelectedHokenInf.RousaiSyobyoCd, item.SelectedHokenInf.RyoyoStartDate, item.SelectedHokenInf.RyoyoEndDate, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate, inputData.SinDate, item.SelectedHokenInf.IsAddNew, inputData.HpId);
                                if (!checkIsValidRodo.Result)
                                {
                                    return checkIsValidRodo;
                                }
                                break;
                            // 労災(傷病年金)
                            case 12:
                                var checkIsValidNenkin = IsValidNenkin(index, item.SelectedHokenInf.NenkinBango, item.SelectedHokenPattern.HokenKbn, item.SelectedHokenInf.ListRousaiTenki, item.SelectedHokenInf.RousaiSaigaiKbn, item.SelectedHokenInf.RousaiSyobyoDate, item.SelectedHokenInf.RousaiSyobyoCd, item.SelectedHokenInf.RyoyoStartDate, item.SelectedHokenInf.RyoyoEndDate, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate, inputData.SinDate, item.SelectedHokenInf.IsAddNew, inputData.HpId);
                                if (!checkIsValidNenkin.Result)
                                {
                                    return checkIsValidNenkin;
                                }
                                break;
                            // アフターケア
                            case 13:
                                var checkIsValidKenko = IsValidKenko(index, item.SelectedHokenInf.KenkoKanriBango, item.SelectedHokenPattern.HokenKbn, item.SelectedHokenInf.ListRousaiTenki, item.SelectedHokenInf.RousaiSaigaiKbn, item.SelectedHokenInf.RousaiSyobyoDate, item.SelectedHokenInf.RousaiSyobyoCd, item.SelectedHokenInf.RyoyoStartDate, item.SelectedHokenInf.RyoyoEndDate, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate, inputData.SinDate, item.SelectedHokenInf.IsAddNew, inputData.HpId);
                                if (!checkIsValidKenko.Result)
                                {
                                    return checkIsValidKenko;
                                }
                                break;
                            // 自賠責
                            case 14:
                                var checkIsValidJibai = IsValidJibai(index, item.SelectedHokenInf.ListRousaiTenki);
                                if (!checkIsValidJibai.Result)
                                {
                                    return checkIsValidJibai;
                                }
                                break;
                        }
                        index++;
                        listHokenPattern.Add(item.SelectedHokenPattern);
                        listHokenInf.Add(item.SelectedHokenInf);
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
                            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.InvalidWarningDuplicatePattern, 0);
                        }
                    }

                    var checkAge = CheckAge(inputData, listHokenPattern, listHokenInf);
                    if (!checkAge.Result)
                    {
                        return checkAge;
                    }
                }
                return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, 0);
            }
            catch (Exception)
            {
                return new ValidateInsuranceOutputData(false, "Validate Exception", ValidateInsuranceStatus.InvalidFaild, 0);
            }
        }

        private ValidateInsuranceOutputData CheckValidateInputData(ValidateInsuranceInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new ValidateInsuranceOutputData(false, string.Empty, ValidateInsuranceStatus.InvalidHpId, 0);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidateInsuranceOutputData(false, string.Empty, ValidateInsuranceStatus.InvalidSindate, 0);
            }

            if (inputData.PtBirthday < 0)
            {
                return new ValidateInsuranceOutputData(false, string.Empty, ValidateInsuranceStatus.InvalidPtBirthday, 0);
            }

            return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, 0);
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

        private ValidateInsuranceOutputData IsValidShaho(ValidateInsuranceModel itemModel, int index, int hpId, int sinDate, int ptBirthday)
        {
            // Validate empty hoken
            var checkMessageIsValidEmptyHoken = IsValidEmptyHoken(itemModel.IsHaveSelectedHokenPattern, itemModel.SelectedHokenPattern.IsAddNew, itemModel.SelectedHokenPattern.IsEmptyHoken, itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.IsEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidEmptyHoken))
            {
                return new ValidateInsuranceOutputData(false, checkMessageIsValidEmptyHoken, ValidateInsuranceStatus.InvalidEmptyHoken, index);
            }
            // Validate HokenNashi only
            var checkMessageIsValidHokenNashiOnly = IsValidHokenNashiOnly(itemModel.IsHaveSelectedHokenPattern, itemModel.IsHaveSelectedHokenInf, itemModel.SelectedHokenPattern.HokenKbn, itemModel.SelectedHokenInf.Houbetu, itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.IsEmptyKohi4);
            if (!String.IsNullOrEmpty(checkMessageIsValidHokenNashiOnly))
            {
                return new ValidateInsuranceOutputData(false, checkMessageIsValidHokenNashiOnly, ValidateInsuranceStatus.InvalidEmptyHoken, index);
            }
            // Valiate HokenInf
            var checkIsValidHokenInf = IsValidHokenInf(itemModel, index, hpId, sinDate, ptBirthday);
            if (!checkIsValidHokenInf.Result)
            {
                return checkIsValidHokenInf;
            }

            //IsValidKohi 1
            var checkMessageKohi = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.Kohi1.IsKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi1.FutansyaNo, itemModel.SelectedHokenPattern.Kohi1.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi1.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi1.StartDate, itemModel.SelectedHokenPattern.Kohi1.EndDate, itemModel.SelectedHokenPattern.Kohi1.ConfirmDate, 
                                               itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.JyukyusyaCheckFlag, 
                                               itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.TokusyuCheckFlag, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.StartDate, 
                                               itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.EndDate, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.DisplayTextMaster, 1, 
                                               sinDate, itemModel.SelectedHokenPattern.Kohi1.IsAddNew, index);
            if (!checkMessageKohi.Result)
            {
                return checkMessageKohi;
            }

            // check Kohi No Function1
            var checkMessageKohiNoFnc1 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.Kohi1.IsKohiMst, itemModel.SelectedHokenPattern.Kohi1.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi1.FutansyaNo, itemModel.SelectedHokenPattern.Kohi1.TokusyuNo, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.JyukyusyaCheckFlag, 
                                                           itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.CheckDigit, 
                                                           itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.Houbetu, itemModel.SelectedHokenPattern.Kohi1.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.AgeStart, itemModel.SelectedHokenPattern.Kohi1.KohiHokenMst.AgeEnd, 1, ptBirthday, index);
            if (!checkMessageKohiNoFnc1.Result)
            {
                return checkMessageKohiNoFnc1;
            }

            //IsValidKohi 2
            var checkMessageKohi2 = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.Kohi2.IsKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi2.FutansyaNo, itemModel.SelectedHokenPattern.Kohi2.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi2.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi2.StartDate, itemModel.SelectedHokenPattern.Kohi2.EndDate, itemModel.SelectedHokenPattern.Kohi2.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.JyukyusyaCheckFlag,
                                               itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.TokusyuCheckFlag, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.EndDate, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.DisplayTextMaster, 2,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi2.IsAddNew, index);
            if (!checkMessageKohi2.Result)
            {
                return checkMessageKohi2;
            }

            // check Kohi No Function2
            var checkMessageKohiNoFnc2 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.Kohi2.IsKohiMst, itemModel.SelectedHokenPattern.Kohi2.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi2.FutansyaNo, itemModel.SelectedHokenPattern.Kohi2.TokusyuNo, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.JyukyusyaCheckFlag,
                                                           itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.Houbetu, itemModel.SelectedHokenPattern.Kohi2.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.AgeStart, itemModel.SelectedHokenPattern.Kohi2.KohiHokenMst.AgeEnd, 2, ptBirthday, index);
            if (!checkMessageKohiNoFnc2.Result)
            {
                return checkMessageKohiNoFnc2;
            }

            //IsValidKohi 3
            var checkMessageKohi3 = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.Kohi3.IsKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi3.FutansyaNo, itemModel.SelectedHokenPattern.Kohi3.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi3.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi3.StartDate, itemModel.SelectedHokenPattern.Kohi3.EndDate, itemModel.SelectedHokenPattern.Kohi3.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.JyukyusyaCheckFlag,
                                               itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.TokusyuCheckFlag, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.EndDate, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.DisplayTextMaster, 3,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi3.IsAddNew, index);
            if (!checkMessageKohi3.Result)
            {
                return checkMessageKohi3;
            }

            // check Kohi No Function3
            var checkMessageKohiNoFnc3 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.Kohi3.IsKohiMst, itemModel.SelectedHokenPattern.Kohi3.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi3.FutansyaNo, itemModel.SelectedHokenPattern.Kohi3.TokusyuNo, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.JyukyusyaCheckFlag,
                                                           itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.Houbetu, itemModel.SelectedHokenPattern.Kohi3.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.AgeStart, itemModel.SelectedHokenPattern.Kohi3.KohiHokenMst.AgeEnd, 3, ptBirthday, index);
            if (!checkMessageKohiNoFnc3.Result)
            {
                return checkMessageKohiNoFnc3;
            }

            //IsValidKohi 4
            var checkMessageKohi4 = IsValidKohi(itemModel.SelectedHokenPattern.IsEmptyKohi4, itemModel.SelectedHokenPattern.Kohi4.IsKohiMst,
                                               itemModel.SelectedHokenPattern.Kohi4.FutansyaNo, itemModel.SelectedHokenPattern.Kohi4.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi4.TokusyuNo,
                                               itemModel.SelectedHokenPattern.Kohi4.StartDate, itemModel.SelectedHokenPattern.Kohi4.EndDate, itemModel.SelectedHokenPattern.Kohi4.ConfirmDate,
                                               itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.JyukyusyaCheckFlag,
                                               itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.TokusyuCheckFlag, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.StartDate,
                                               itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.EndDate, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.DisplayTextMaster, 4,
                                               sinDate, itemModel.SelectedHokenPattern.Kohi4.IsAddNew, index);
            if (!checkMessageKohi4.Result)
            {
                return checkMessageKohi4;
            }

            // check Kohi No Function4
            var checkMessageKohiNoFnc4 = IsValidKohiNo_Fnc(itemModel.SelectedHokenPattern.IsEmptyKohi4, itemModel.SelectedHokenPattern.Kohi4.IsKohiMst, itemModel.SelectedHokenPattern.Kohi4.HokenNo,
                                                           itemModel.SelectedHokenPattern.Kohi4.FutansyaNo, itemModel.SelectedHokenPattern.Kohi4.TokusyuNo, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.JyukyusyaCheckFlag,
                                                           itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.FutansyaCheckFlag, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.JyuKyuCheckDigit, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.CheckDigit,
                                                           itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.Houbetu, itemModel.SelectedHokenPattern.Kohi4.JyukyusyaNo, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.AgeStart, itemModel.SelectedHokenPattern.Kohi4.KohiHokenMst.AgeEnd, 4, ptBirthday, index);
            if (!checkMessageKohiNoFnc4.Result)
            {
                return checkMessageKohiNoFnc4;
            }

            var checkMessageKohiAll = IsvalidKohiAll(index, itemModel.SelectedHokenPattern.IsEmptyKohi1, itemModel.SelectedHokenPattern.IsEmptyKohi2, itemModel.SelectedHokenPattern.IsEmptyKohi3, itemModel.SelectedHokenPattern.IsEmptyKohi4, itemModel.SelectedHokenPattern.Kohi1, itemModel.SelectedHokenPattern.Kohi2, itemModel.SelectedHokenPattern.Kohi3, itemModel.SelectedHokenPattern.Kohi4 );
            if (!checkMessageKohiAll.Result)
            {
                return checkMessageKohiAll;
            }

            //IsValidMaruchoOnly
            var checkIsValidMaruchoOnly = IsValidMaruchoOnly(index, itemModel);
            if (!checkIsValidMaruchoOnly.Result)
            {
                return checkMessageKohiAll;
            }

            return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, 0);
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

        private ValidateInsuranceOutputData IsValidHokenInf(ValidateInsuranceModel item, int index, int hpId, int sinDate, int ptBirthday)
        {
            var message = "";
            if (!item.IsHaveSelectedHokenInf)
            {
                return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, index);
            }
            // Validate not HokenInf
            if (item.SelectedHokenPattern.HokenKbn == 1 && item.SelectedHokenInf.Houbetu == HokenConstant.HOUBETU_NASHI)
            {
                var checkIsValidHokenNashi = IsValidHokenNashi(index, hpId, sinDate, item.SelectedHokenInf.Tokki1, item.SelectedHokenInf.Tokki2, item.SelectedHokenInf.Tokki3, item.SelectedHokenInf.Tokki4, item.SelectedHokenInf.Tokki5, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate);
                if (!checkIsValidHokenNashi.Result)
                {
                    return checkIsValidHokenNashi;
                }
            }
            // Validate Jihi
            if (item.SelectedHokenInf.IsJihi)
            {
                return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, index);
            }
            var checkIsValidHokenDetail = IsValidHokenDetail(index, hpId, sinDate, item.SelectedHokenInf.Tokki1, item.SelectedHokenInf.Tokki2, item.SelectedHokenInf.Tokki3, item.SelectedHokenInf.Tokki4, item.SelectedHokenInf.Tokki5);
            if (!checkIsValidHokenDetail.Result)
            {
                return checkIsValidHokenDetail;
            }
            var checkCHKHokno_Fnc = CHKHokno_Fnc(index, item.SelectedHokenInf.HokensyaNo, item.SelectedHokenInf.HokenNo, item.SelectedHokenInf.Houbetu, item.IsHaveSelectedHokenMst, item.SelectedHokenMst.Houbetu, item.SelectedHokenMst.HokenNo, item.SelectedHokenMst.CheckDigit, ptBirthday, item.SelectedHokenMst.AgeStart, item.SelectedHokenMst.AgeEnd);
            if (!checkCHKHokno_Fnc.Result)
            {
                return checkCHKHokno_Fnc;
            }
            if (string.IsNullOrEmpty(item.SelectedHokenInf.HokensyaNo))
            {
                var paramsMessage = new string[] { "保険者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokensyaNoNull, index);
            }
            if (item.SelectedHokenInf.HokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenNoEquals0, index);
            }
            if (Int32.Parse(item.SelectedHokenInf.HokensyaNo) == 0)
            {
                var paramsMessage = new string[] { "保険者番号は 0 〜 9 の範囲で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokensyaNoEquals0, index);
            }
            // 記号
            if (item.SelectedHokenInf.HokensyaNo.Length == 8 && item.SelectedHokenInf.HokensyaNo.Trim().StartsWith("39"))
            {
                if (!string.IsNullOrEmpty(item.SelectedHokenInf.Kigo)
                    && !string.IsNullOrEmpty(item.SelectedHokenInf.Kigo.Trim(' '))) //Trim only half-size space
                {
                    var paramsMessage = new string[] { "後期高齢者の", "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00150, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokensyaNoLength8StartWith39, index);
                }
            }
            else
            {
                if (item.SelectedHokenMst.IsKigoNashi == 0 && (string.IsNullOrEmpty(item.SelectedHokenInf.Kigo)
                    || string.IsNullOrEmpty(item.SelectedHokenInf.Kigo.Trim(' '))))
                {
                    var paramsMessage = new string[] { "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKigoNull, index);
                }
            }
            if (string.IsNullOrEmpty(item.SelectedHokenInf.Bango)
                    || string.IsNullOrEmpty(item.SelectedHokenInf.Bango.Trim(' '))) //Trim only half-size space
            {
                var paramsMessage = new string[] { "被保険者証番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidBangoNull, index);
            }
            if (item.SelectedHokenPattern.HokenKbn == 0)
            {
                var paramsMessage = new string[] { "本人家族区分" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenKbnEquals0, index);
            }
            var checkIsValidYukoKigen = IsValidYukoKigen(index, item.SelectedHokenInf.StartDate, item.SelectedHokenInf.EndDate);
            if (!checkIsValidYukoKigen.Result)
            {
                return checkIsValidYukoKigen;
            }
            var checkIsValidTokkurei = IsValidTokkurei(index, ptBirthday, sinDate, item.SelectedHokenInf.TokureiYm1, item.SelectedHokenInf.TokureiYm2, item.SelectedHokenInf.HokensyaNo, item.SelectedHokenInf.IsShahoOrKokuho, item.SelectedHokenInf.IsExpirated);
            if (!checkIsValidTokkurei.Result)
            {
                return checkIsValidTokkurei;
            }
            var checkMessageIsValidConfirmDateAgeCheck = IsValidConfirmDateAgeCheck(item.SelectedHokenInf.IsAddNew, item.SelectedHokenInf.IsExpirated, item.SelectedHokenInf.IsShahoOrKokuho, item.SelectedHokenInf.HokensyaNo, item.SelectedHokenInf.ConfirmDate, ptBirthday, sinDate, hpId);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateAgeCheck))
            {
                return new ValidateInsuranceOutputData(false, checkMessageIsValidConfirmDateAgeCheck, ValidateInsuranceStatus.InvalidHokenKbnEquals0, index);
            }
            // check valid hokenmst date
            var checkIsValidHokenMstDate = IsValidHokenMstDate(index, item.SelectedHokenMst.StartDate, item.SelectedHokenMst.EndDate, sinDate, item.SelectedHokenMst.DisplayTextMaster);
            if (!checkIsValidHokenMstDate.Result)
            {
                return checkIsValidHokenMstDate;
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }

        private ValidateInsuranceOutputData IsValidHokenNashi(int index, int hpId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
        {
            var checkIsValidHokenDetail = IsValidHokenDetail(index, hpId, sinDate, tokki1, tokki2, tokki3, tokki4, tokki5);
            if (!checkIsValidHokenDetail.Result)
            {
                return checkIsValidHokenDetail;
            }
            var checkIsValidYukoKigen = IsValidYukoKigen(index, startDate, endDate);
            if (!checkIsValidYukoKigen.Result)
            {
                return checkIsValidYukoKigen;
            }
            return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData IsValidYukoKigen(int index, int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            int yukoFromDate = selectedHokenInfStartDate;
            int yukoToDate = selectedHokenInfEndDate;
            if (yukoFromDate != 0 && yukoToDate != 0 && yukoFromDate > yukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
            }
            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidYukoKigen, index);
        }

        private ValidateInsuranceOutputData IsValidHokenDetail(int index, int hpId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue1, index);
                }
                if (!string.IsNullOrEmpty(tokki2Value) && tokki2Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki2Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue21, index);
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue31, index);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue41, index);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue51, index);
                }
            }
            if (!string.IsNullOrEmpty(tokki2Value))
            {
                if (!_isValidLengthTokki(tokki2Value))
                {
                    var paramsMessage = new string[] { "特記事項２", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue2, index);
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue23, index);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue24, index);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue25, index);
                }
            }
            if (!string.IsNullOrEmpty(tokki3Value))
            {
                if (!_isValidLengthTokki(tokki3Value))
                {
                    var paramsMessage = new string[] { "特記事項３", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue3, index);
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue34, index);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue35, index);
                }
            }
            if (!string.IsNullOrEmpty(tokki4Value))
            {
                if (!_isValidLengthTokki(tokki4Value))
                {
                    var paramsMessage = new string[] { "特記事項４", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue4, index);
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki4Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue45, index);
                }
            }
            if (!string.IsNullOrEmpty(tokki5Value) && !_isValidLengthTokki(tokki5Value))
            {
                var paramsMessage = new string[] { "特記事項５", "2文字" };
                message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkiValue5, index);
            }

            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.InvalidTokkiValue1, index);
        }

        private ValidateInsuranceOutputData CHKHokno_Fnc(int index, string hokenSyaNo, int hokenNo, string houbetu, bool isHaveSelectedHokenMst, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd)
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0, index);
                }
            }
            //保険番号入力あり
            else
            {
                if (hokenNo == 0)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenNoEquals0, index);
                }
                if (!isHaveSelectedHokenMst)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenNoHaveHokenMst, index);
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHoubetu, index);
                }
                //チェックデジット
                if (sHokenMstCheckDegit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(hokenSyaNo)))
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidCheckDigitEquals1, index);
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
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidCheckAgeHokenMst, index);
                    }
                }
            }
            return new ValidateInsuranceOutputData(true, string.Empty, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData IsValidTokkurei(int index, int ptBirthday, int sinDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, string hokenSyaNo, bool isShahoOrKokuho, bool isExpirated)
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
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokkurei, index);
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
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

        private ValidateInsuranceOutputData IsValidHokenMstDate(int index, int startDate, int endDate, int sinDate, string displayTextMaster)
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenMstStartDate, index);
                }
                if (hokenEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(hokenEndDate) + ")", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidHokenMstEndDate, index);
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData IsValidRodo(int index, string rodoBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var rousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            if (rousaiReceder == 1)
            {
                if (string.IsNullOrEmpty(rodoBango))
                {
                    var paramsMessage = new string[] { "労働保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRodoBangoNull, index);
                }
                if (rodoBango.Trim().Length != 14)
                {
                    var paramsMessage = new string[] { "労働保険番号", " 14桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRodoBangoLengthNotEquals14, index);
                }
            }
            var checkCommonCheckForRosai = CommonCheckForRosai(index, hokenKbn, listRousaiTenkis, rousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkCommonCheckForRosai.Result)
            {
                return checkCommonCheckForRosai;
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData IsValidNenkin(int index, string nenkinBago, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufu = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufu == 1)
            {
                if (string.IsNullOrEmpty(nenkinBago))
                {
                    var paramsMessage = new string[] { "年金証書番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidNenkinBangoNull, index);
                }
                if (nenkinBago.Trim().Length != 9)
                {
                    var paramsMessage = new string[] { "年金証書番号", " 9桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidNenkinBangoLengthNotEquals9, index);
                }
            }
            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            var checkCommonCheckForRosai = CommonCheckForRosai(index, hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkCommonCheckForRosai.Result)
            {
                return checkCommonCheckForRosai;
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData IsValidKenko(int index, string kenkoKanriBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufuValidate = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufuValidate == 1)
            {
                if (string.IsNullOrEmpty(kenkoKanriBango))
                {
                    var paramsMessage = new string[] { "健康管理手帳番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKenkoKanriBangoNull, index);
                }
                if (kenkoKanriBango.Trim().Length != 13)
                {
                    var paramsMessage = new string[] { "健康管理手帳番号", " 13桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKenkoKanriBangoLengthNotEquals13, index);
                }
            }

            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            var checkCommonCheckForRosai = CommonCheckForRosai(index, hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkCommonCheckForRosai.Result)
            {
                return checkCommonCheckForRosai;
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData IsValidJibai(int index, List<RousaiTenkiModel> listRousaiTenkis)
        {
            var message = "";
            if (listRousaiTenkis != null && listRousaiTenkis.Count > 0)
            {
                var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                if (rousaiTenkiList.FirstOrDefault()?.RousaiTenkiTenki <= 0)
                {
                    var paramsMessage = new string[] { "転帰事由" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiDefaultRow, index);
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
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiData, index);
                    }
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
        }

        private ValidateInsuranceOutputData CommonCheckForRosai(int index, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int rosaiReceden, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew)
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
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiDefaultRow, index);
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
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiTenkiData, index);
                        }
                    }
                }
                if (rosaiReceden == 1)
                {
                    if (sHokenInfRousaiSaigaiKbn != 1 && sHokenInfRousaiSaigaiKbn != 2)
                    {
                        var paramsMessage = new string[] { "災害区分" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiSaigaiKbn, index);
                    }

                    if (sHokenInfRousaiSyobyoDate == 0)
                    {
                        var paramsMessage = new string[] { "傷病年月日" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiSyobyoDateEquals0, index);
                    }
                }
            }
            else if (hokenKbn == 13 && string.IsNullOrEmpty(sHokenInfRousaiSyobyoCd))
            {
                var paramsMessage = new string[] { "傷病コード" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiSyobyoDateEquals0, index);
            }

            // 労災・療養期間ﾁｪｯｸ
            int rousaiRyoyoStartDate = sHokenInfRyoyoStartDate;
            int rousaiRyoyoEndDate = sHokenInfRyoyoEndDate;
            if (rousaiRyoyoStartDate != 0 && rousaiRyoyoEndDate != 0 && rousaiRyoyoStartDate > rousaiRyoyoEndDate)
            {
                var paramsMessage = new string[] { "労災療養終了日", "労災療養開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRousaiRyoyoDate, index);
            }

            // 労災・有効期限ﾁｪｯｸ
            int rosaiYukoFromDate = sHokenInfStartDate;
            int rosaiYukoToDate = sHokenInfEndDate;
            if (rosaiYukoFromDate != 0 && rosaiYukoToDate != 0 && rosaiYukoFromDate > rosaiYukoToDate)
            {
                var paramsMessage = new string[] { "労災有効終了日", "労災有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidRosaiYukoDate, index);
            }
            // 労災・期限切れﾁｪｯｸ(有効保険の場合のみ)
            if (!DataChkFn10(sinDate, sHokenInfStartDate, sHokenInfEndDate, isAddNew))
            {
                var paramsMessage = new string[] { "労災保険", "無視する", "戻る" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidCheckHokenInfDate, index);
            }

            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, index);
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

        private ValidateInsuranceOutputData IsValidKohi(bool isKohiModdel, bool isHokenMstModel, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenMstIsFutansyaCheckFlag, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsTokusyuCheckFlag, int hokenMstModelStartDate, int hokenMstModelEndDate, string hokenMstDisplayText, int numberKohi, int sinDate, bool isAddNew, int index)
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel4, index);
                }
            }

            if (isHokenMstModel)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty4, index);
                }
            }

            // check validate data
            var checkValidateData = CheckValidData(index, numberMessage, numberKohi, futansyaNo, hokenMstIsFutansyaCheckFlag, jyukyusyaNo, hokenMstIsJyukyusyaCheckFlag, tokusyuNo, hokenMstIsTokusyuCheckFlag);
            if (!checkValidateData.Result)
            {
                return checkValidateData;
            }

            // check kohi date
            var checkKohiDate = CheckKohiDate(index, startDate, endDate, numberMessage, numberKohi);
            if (!checkKohiDate.Result)
            {
                return checkKohiDate;
            }

            // check confirm date kohi
            var checkMessageIsValidConfirmDateKohi = IsValidConfirmDateKohi(index, confirmDate, numberMessage, sinDate, isAddNew, numberKohi);
            if (!checkMessageIsValidConfirmDateKohi.Result)
            {
                return checkMessageIsValidConfirmDateKohi;
            }

            // master date kohi IsValidMasterDateKohi
            var checkMasterDateKohi = CheckMasterDateKohi(index, hokenMstModelStartDate, hokenMstModelEndDate, sinDate, numberMessage, hokenMstDisplayText, numberKohi);
            if (!checkMasterDateKohi.Result)
            {
                return checkMasterDateKohi;
            }

            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }

        private ValidateInsuranceOutputData CheckValidData(int index, string numberMessage, int numberKohi, string futansyaNo, int hokenMstIsFutansyaCheckFlag, string jyukyusyaNo, int hokenMstIsJyukyusyaCheckFlag, string tokusyuNo, int hokenMstIsTokusyuCheckFlag)
        {
            var message = "";
            if (string.IsNullOrEmpty(futansyaNo.Trim())
                && hokenMstIsFutansyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty4, index);
                }
            }
            else if (string.IsNullOrEmpty(jyukyusyaNo.Trim())
                && hokenMstIsJyukyusyaCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidJyukyusyaNo4, index);
                }
            }
            else if (string.IsNullOrEmpty(tokusyuNo.Trim())
                && hokenMstIsTokusyuCheckFlag == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokusyuNo1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokusyuNo2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokusyuNo3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidTokusyuNo4, index);
                }
            }
            if (!string.IsNullOrEmpty(futansyaNo) && Int32.Parse(futansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNo01, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNo02, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNo03, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNo04, index);
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }
        private ValidateInsuranceOutputData CheckKohiDate(int index, int startDate, int endDate, string numberMessage, int numberKohi)
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiYukoDate4, index);
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }

        private ValidateInsuranceOutputData CheckMasterDateKohi(int index, int hokenMstModelStartDate, int hokenMstModelEndDate, int sinDate, string numberMessage, string hokenMstDisplayText, int numberKohi)
        {
            var message = "";
            if (hokenMstModelStartDate > sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelStartDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstStartDate4, index);
                }
            }
            if (hokenMstModelEndDate < sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + hokenMstDisplayText + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(hokenMstModelEndDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                if (numberKohi == 1)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEndDate4, index);
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }

        private ValidateInsuranceOutputData IsValidConfirmDateKohi(int index, int confirmDate, string numberMessage, int sinDate, bool isAddNew, int numberKohi)
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
                    return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiConfirmDate4, index);
                }
            }
            else
            {
                return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
            }
        }

        private ValidateInsuranceOutputData IsvalidKohiAll(int index, bool isKohiEmptyModel1, bool isKohiEmptyModel2, bool isKohiEmptyModel3, bool isKohiEmptyModel4, SelectedKohiModel kohi1, SelectedKohiModel kohi2, SelectedKohiModel kohi3, SelectedKohiModel kohi4)
        {
            var message = "";
            if (isKohiEmptyModel2 && isKohiEmptyModel1)
            {
                var paramsMessage = new string[] { "公費１" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmpty21, index);
            }

            if (isKohiEmptyModel3)
            {
                if (isKohiEmptyModel1)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmpty31, index);
                }

                if (isKohiEmptyModel2)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmpty32, index);
                }
            }

            if (isKohiEmptyModel4)
            {
                if (isKohiEmptyModel1)
                {
                    var paramsMessage = new string[] { "公費１" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmpty41, index);
                }

                if (isKohiEmptyModel2)
                {
                    var paramsMessage = new string[] { "公費２" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmpty42, index);
                }

                if (isKohiEmptyModel3)
                {
                    var paramsMessage = new string[] { "公費３" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmpty43, index);
                }
            }
            // check duplicate 1
            if (!isKohiEmptyModel1 && ((!isKohiEmptyModel2 && (kohi1.FutansyaNo == kohi2.FutansyaNo && kohi1.JyukyusyaNo == kohi2.JyukyusyaNo  && kohi1.StartDate == kohi2.StartDate && kohi1.EndDate == kohi2.EndDate && kohi1.ConfirmDate == kohi2.ConfirmDate))
                   || (!isKohiEmptyModel3 && (kohi1.FutansyaNo == kohi3.FutansyaNo && kohi1.JyukyusyaNo == kohi3.JyukyusyaNo  && kohi1.StartDate == kohi3.StartDate && kohi1.EndDate == kohi3.EndDate && kohi1.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel4 && (kohi1.FutansyaNo == kohi4.FutansyaNo && kohi1.JyukyusyaNo == kohi4.JyukyusyaNo  && kohi1.StartDate == kohi4.StartDate && kohi1.EndDate == kohi4.EndDate && kohi1.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi1, index);
            }
            // check duplicate 2
            if (!isKohiEmptyModel2 && ((!isKohiEmptyModel1 && (kohi1.FutansyaNo == kohi2.FutansyaNo && kohi1.JyukyusyaNo == kohi2.JyukyusyaNo && kohi1.StartDate == kohi2.StartDate && kohi1.EndDate == kohi2.EndDate && kohi1.ConfirmDate == kohi2.ConfirmDate))
                   || (!isKohiEmptyModel3 && (kohi2.FutansyaNo == kohi3.FutansyaNo && kohi2.JyukyusyaNo == kohi3.JyukyusyaNo && kohi2.StartDate == kohi3.StartDate && kohi2.EndDate == kohi3.EndDate && kohi2.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel4 && (kohi2.FutansyaNo == kohi4.FutansyaNo && kohi2.JyukyusyaNo == kohi4.JyukyusyaNo && kohi2.StartDate == kohi4.StartDate && kohi2.EndDate == kohi4.EndDate && kohi2.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi2, index);
            }
            // check duplicate 3
            if (!isKohiEmptyModel3 && ((!isKohiEmptyModel1 && (kohi1.FutansyaNo == kohi3.FutansyaNo && kohi1.JyukyusyaNo == kohi3.JyukyusyaNo && kohi1.StartDate == kohi3.StartDate && kohi1.EndDate == kohi3.EndDate && kohi1.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel2 && (kohi2.FutansyaNo == kohi3.FutansyaNo && kohi2.JyukyusyaNo == kohi3.JyukyusyaNo && kohi2.StartDate == kohi3.StartDate && kohi2.EndDate == kohi3.EndDate && kohi2.ConfirmDate == kohi3.ConfirmDate))
                   || (!isKohiEmptyModel4 && (kohi3.FutansyaNo == kohi4.FutansyaNo && kohi3.JyukyusyaNo == kohi4.JyukyusyaNo && kohi3.StartDate == kohi4.StartDate && kohi3.EndDate == kohi4.EndDate && kohi3.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi3, index);
            }
            // check duplicate 4
            if (!isKohiEmptyModel4 && ((!isKohiEmptyModel1 && (kohi1.FutansyaNo == kohi4.FutansyaNo && kohi1.JyukyusyaNo == kohi4.JyukyusyaNo && kohi1.StartDate == kohi4.StartDate && kohi1.EndDate == kohi4.EndDate && kohi1.ConfirmDate == kohi4.ConfirmDate))
                   || (!isKohiEmptyModel2 && (kohi2.FutansyaNo == kohi4.FutansyaNo && kohi2.JyukyusyaNo == kohi4.JyukyusyaNo && kohi2.StartDate == kohi4.StartDate && kohi2.EndDate == kohi4.EndDate && kohi2.ConfirmDate == kohi4.ConfirmDate))
                   || (!isKohiEmptyModel3 && (kohi3.FutansyaNo == kohi4.FutansyaNo && kohi3.JyukyusyaNo == kohi4.JyukyusyaNo && kohi3.StartDate == kohi4.StartDate && kohi3.EndDate == kohi4.EndDate && kohi3.ConfirmDate == kohi4.ConfirmDate))
                    ))
            {
                var paramsMessage = new string[] { "同じ公費は選択できません。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidDuplicateKohi4, index);
            }

            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }

        private ValidateInsuranceOutputData IsValidKohiNo_Fnc(bool isKohiModel, bool isKohiMstModel, int hokenNo, string futansyaNo, string tokusyuNo, int hokenMstIsJyukyusyaCheckFlag, int hokenMstIsFutansyaCheckFlag, int hokenMstJyukyuCheckDigit, int hokenMstCheckDigit, string hokenMstHoubetu, string jyukyusyaNo, int hokenMstAgeStartDate, int hokenMstAgeEndDate, int numberKohi, int ptBirthday, int index)
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiEmptyModel4, index);
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
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty1, index);
                }
                else if (numberKohi == 2)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty2, index);
                }
                else if (numberKohi == 3)
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty3, index);
                }
                else
                {
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiHokenMstEmpty4, index);
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
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty1, index);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty2, index);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty3, index);
                    }
                    else
                    {
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutansyaNoEmpty4, index);
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
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT1, index);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT2, index);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT3, index);
                        }
                        else
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckHBT4, index);
                        }
                    }
                    //チェックデジット
                    if (hokenMstCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(futansyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo1, index);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo2, index);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo3, index);
                        }
                        else
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo4, index);
                        }
                    }
                    if (hokenMstIsJyukyusyaCheckFlag == 1 && hokenMstJyukyuCheckDigit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(jyukyusyaNo)))
                    {
                        var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                        message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                        if (numberKohi == 1)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo1, index);
                        }
                        else if (numberKohi == 2)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo2, index);
                        }
                        else if (numberKohi == 3)
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo3, index);
                        }
                        else
                        {
                            return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo4, index);
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
                                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge1, index);
                            }
                            else if (numberKohi == 2)
                            {
                                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge2, index);
                            }
                            else if (numberKohi == 3)
                            {
                                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge3, index);
                            }
                            else
                            {
                                return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidKohiMstCheckAge4, index);
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
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull1, index);
                    }
                    else if (numberKohi == 2)
                    {
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull2, index);
                    }
                    else if (numberKohi == 3)
                    {
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull3, index);
                    }
                    else
                    {
                        return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidFutanJyoTokuNull4, index);
                    }
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }

        private bool CheckPatternDuplicate(SelectedHokenPattern pattern, SelectedHokenPattern item)
        {
            if (pattern.IsEmptyHoken != item.IsEmptyHoken)
                return false;

            if (!pattern.IsEmptyHoken && !item.IsEmptyHoken && pattern.HokenId != item.HokenId)
                return false;

            if (pattern.IsEmptyKohi1 != item.IsEmptyKohi1)
                return false;

            if (!pattern.IsEmptyKohi1 && !item.IsEmptyKohi1 && pattern.Kohi1Id != item.Kohi1Id)
                return false;

            if (pattern.IsEmptyKohi2 != item.IsEmptyKohi2)
                return false;

            if (!pattern.IsEmptyKohi2 && !item.IsEmptyKohi2 && pattern.Kohi2Id != item.Kohi2Id)
                return false;

            if (pattern.IsEmptyKohi3 != item.IsEmptyKohi3)
                return false;

            if (!pattern.IsEmptyKohi3 && !item.IsEmptyKohi3 && pattern.Kohi3Id != item.Kohi3Id)
                return false;

            if (pattern.IsEmptyKohi4 != item.IsEmptyKohi4)
                return false;

            if (!pattern.IsEmptyKohi4 && !item.IsEmptyKohi4 && pattern.Kohi4Id != item.Kohi4Id)
                return false;
            return true;
        }

        private ValidateInsuranceOutputData CheckAge(ValidateInsuranceInputData inputData, List<SelectedHokenPattern> listHokenPattern, List<SelectedHokenInf> listHokenInf)
        {
            var messageError = "";
            if (inputData.SinDate >= 20080401 && (listHokenPattern.Count > 0 && listHokenInf.Count > 0))
            {
                var patternHokenOnlyCheckAge = listHokenPattern.Where(pattern => pattern.IsDeleted == 0 && !pattern.IsExpirated);
                int age = CIUtil.SDateToAge(inputData.PtBirthday, inputData.SinDate);
                // hoken exist in at least 1 pattern
                var inUsedHokens = listHokenInf.Where(hoken => hoken.HokenId > 0 && hoken.IsDeleted == 0 && !hoken.IsExpirated
                                                            && patternHokenOnlyCheckAge.Any(pattern => pattern.HokenId == hoken.HokenId));

                var elderHokenQuery = inUsedHokens.Where(hoken => hoken.EndDate >= inputData.SinDate
                                                                        && hoken.HokensyaNo != null && hoken.HokensyaNo != ""
                                                                        && hoken.HokensyaNo.Length == 8 && hoken.HokensyaNo.StartsWith("39"));
                if (elderHokenQuery != null)
                {
                    if (age >= 75 && !elderHokenQuery.Any())
                    {
                        var paramsMessage75 = new string[] { "後期高齢者保険が入力されていません。", "保険者証" };
                        messageError = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage75);
                        return new ValidateInsuranceOutputData(false, messageError, ValidateInsuranceStatus.InvalidWarningAge75, 0);
                    }
                    else if (age < 65 && elderHokenQuery.Any())
                    {
                        var paramsMessage65 = new string[] { "後期高齢者保険の対象外の患者に、後期高齢者保険が登録されています。", "保険者証" };
                        messageError = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage65);
                        return new ValidateInsuranceOutputData(true, messageError, ValidateInsuranceStatus.InvalidWarningAge65, 0);
                    }
                }
            }
            return new ValidateInsuranceOutputData(true, messageError, ValidateInsuranceStatus.Successed, 0);
        }

        private ValidateInsuranceOutputData IsValidMaruchoOnly(int index, ValidateInsuranceModel item)
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
                if (!item.SelectedHokenPattern.IsEmptyKohi1 && item.SelectedHokenPattern.Kohi1.IsKohiMst)
                {
                    //2:マル長
                    isEmptyKohi1 = item.SelectedHokenPattern.Kohi1.KohiHokenMst.HokenSbtKbn == 2;
                }
                bool isEmptyKohi2 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi2 && item.SelectedHokenPattern.Kohi2.IsKohiMst)
                {
                    //2:マル長
                    isEmptyKohi2 = item.SelectedHokenPattern.Kohi2.KohiHokenMst.HokenSbtKbn == 2;
                }
                bool isEmptyKohi3 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi3 && item.SelectedHokenPattern.Kohi3.IsKohiMst)
                {
                    //2:マル長
                    isEmptyKohi3 = item.SelectedHokenPattern.Kohi3.KohiHokenMst.HokenSbtKbn == 2;
                }
                bool isEmptyKohi4 = true;
                if (!item.SelectedHokenPattern.IsEmptyKohi4 && item.SelectedHokenPattern.Kohi4.IsKohiMst)
                {
                    //2:マル長
                    isEmptyKohi1 = item.SelectedHokenPattern.Kohi4.KohiHokenMst.HokenSbtKbn == 2;
                }
                if (!item.SelectedHokenPattern.IsAddNew && isEmptyHoken && isEmptyKohi1 && isEmptyKohi2 && isEmptyKohi3 && isEmptyKohi4)
                {
                    var paramsMessage65 = new string[] { "保険組合せ", "情報" };
                    message = String.Format(ErrorMessage.MessageType_mInp00011, paramsMessage65);
                    return new ValidateInsuranceOutputData(false, message, ValidateInsuranceStatus.InvalidMaruchoOnly, index);
                }
            }
            return new ValidateInsuranceOutputData(true, message, ValidateInsuranceStatus.Successed, 0);
        }
    }
}