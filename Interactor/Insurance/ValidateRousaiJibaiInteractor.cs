using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.ReceptionInsurance;
using Domain.Models.SystemConf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidateRousaiJibai;

namespace Interactor.Insurance
{
    public class ValidateRousaiJibaiInteractor : IValidateRousaiJibaiInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        public ValidateRousaiJibaiInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public ValidateRousaiJibaiOutputData Handle(ValidateRousaiJibaiInputData inputData)
        {
            try
            {
                var checkValidInputData = CheckValidateInput(inputData);
                if(!checkValidInputData.Result)
                {
                    return checkValidInputData;
                }    
                switch (inputData.HokenKbn)
                {
                    // 労災(短期給付)	
                    case 11:
                        var checkMessageIsValidRodo = IsValidRodo(inputData.SelectedHokenInfRodoBango, inputData.HokenKbn, inputData.ListRousaiTenki, inputData.SelectedHokenInfRousaiSaigaiKbn, inputData.SelectedHokenInfRousaiSyobyoDate, inputData.SelectedHokenInfRousaiSyobyoCd, inputData.SelectedHokenInfRyoyoStartDate, inputData.SelectedHokenInfRyoyoEndDate, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SinDate, inputData.SelectedHokenInfIsAddNew, inputData.HpId);
                        if (!checkMessageIsValidRodo.Result)
                        {
                            return checkMessageIsValidRodo;
                        }
                        break;
                    // 労災(傷病年金)
                    case 12:
                        var checkMessageIsValidNenkin = IsValidNenkin(inputData.SelectedHokenInfNenkinBango, inputData.HokenKbn, inputData.ListRousaiTenki, inputData.SelectedHokenInfRousaiSaigaiKbn, inputData.SelectedHokenInfRousaiSyobyoDate, inputData.SelectedHokenInfRousaiSyobyoCd, inputData.SelectedHokenInfRyoyoStartDate, inputData.SelectedHokenInfRyoyoEndDate, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SinDate, inputData.SelectedHokenInfIsAddNew, inputData.HpId);
                        if (!checkMessageIsValidNenkin.Result)
                        {
                            return checkMessageIsValidNenkin;
                        }
                        break;
                    // アフターケア
                    case 13:
                        var checkMessageIsValidKenko = IsValidKenko(inputData.SelectedHokenInfKenkoKanriBango, inputData.HokenKbn, inputData.ListRousaiTenki, inputData.SelectedHokenInfRousaiSaigaiKbn, inputData.SelectedHokenInfRousaiSyobyoDate, inputData.SelectedHokenInfRousaiSyobyoCd, inputData.SelectedHokenInfRyoyoStartDate, inputData.SelectedHokenInfRyoyoEndDate, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SinDate, inputData.SelectedHokenInfIsAddNew, inputData.HpId);
                        if (!checkMessageIsValidKenko.Result)
                        {
                            return checkMessageIsValidKenko;
                        }
                        break;
                    // 自賠責
                    case 14:
                        var checkMessageIsValidJibai = IsValidJibai(inputData.ListRousaiTenki);
                        if (!String.IsNullOrEmpty(checkMessageIsValidJibai))
                        {
                            return new ValidateRousaiJibaiOutputData(false, checkMessageIsValidJibai, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate);
                        }
                        break;
                }
                return new ValidateRousaiJibaiOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidateRousaiJibaiStatus.InvalidSuccess);
            }
            catch (Exception)
            {
                return new ValidateRousaiJibaiOutputData(false, "Validate Exception", TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidFaild);
            }
        }

        private ValidateRousaiJibaiOutputData IsValidRodo(string rodoBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var rousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            if (rousaiReceder == 1)
            {
                if (string.IsNullOrEmpty(rodoBango))
                {
                    var paramsMessage = new string[] { "労働保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidRodoBangoNull);
                }
                if (rodoBango.Trim().Length != 14)
                {
                    var paramsMessage = new string[] { "労働保険番号", " 14桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidRodoBangoLengthNotEquals14);
                }
            }
            var checkMessageCommonCheckForRosai = CommonCheckForRosai(hokenKbn, listRousaiTenkis, rousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkMessageCommonCheckForRosai.Result)
            {
                return checkMessageCommonCheckForRosai;
            }
            return new ValidateRousaiJibaiOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidateRousaiJibaiStatus.InvalidSuccess);
        }

        private ValidateRousaiJibaiOutputData IsValidNenkin(string nenkinBago, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufu = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufu == 1)
            {
                if (string.IsNullOrEmpty(nenkinBago))
                {
                    var paramsMessage = new string[] { "年金証書番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidNenkinBangoIsNull);
                }
                if (nenkinBago.Trim().Length != 9)
                {
                    var paramsMessage = new string[] { "年金証書番号", " 9桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidNenkinBangoLengthNotEquals9);
                }
            }
            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            var checkMessageCommonCheckForRosai = CommonCheckForRosai(hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkMessageCommonCheckForRosai.Result)
            {
                return checkMessageCommonCheckForRosai;
            }
            return new ValidateRousaiJibaiOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidateRousaiJibaiStatus.InvalidSuccess);
        }
        private ValidateRousaiJibaiOutputData IsValidKenko(string kenkoKanriBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufuValidate = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufuValidate == 1)
            {
                if (string.IsNullOrEmpty(kenkoKanriBango))
                {
                    var paramsMessage = new string[] { "健康管理手帳番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoIsNull);
                }
                if (kenkoKanriBango.Trim().Length != 13)
                {
                    var paramsMessage = new string[] { "健康管理手帳番号", " 13桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoLengthNotEquals13);
                }
            }

            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            var checkMessageCommonCheckForRosai = CommonCheckForRosai(hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
            if (!checkMessageCommonCheckForRosai.Result)
            {
                return checkMessageCommonCheckForRosai;
            }
            return new ValidateRousaiJibaiOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidateRousaiJibaiStatus.InvalidSuccess);
        }

        private string IsValidJibai(List<RousaiTenkiModel> listRousaiTenkis)
        {
            var message = "";
            if (listRousaiTenkis != null && listRousaiTenkis.Count > 0)
            {
                var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                if (rousaiTenkiList.FirstOrDefault()?.RousaiTenkiTenki <= 0)
                {
                    var paramsMessage = new string[] { "転帰事由" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    return message;
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
                        return message;
                    }
                }
            }
            return message;
        }

        private ValidateRousaiJibaiOutputData CheckValidateInput(ValidateRousaiJibaiInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidHpId);
            }

            if (inputData.HokenKbn < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidHokenKbn);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSinDate);
            }

            if (inputData.SelectedHokenInfRousaiSaigaiKbn < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSaigaiKbn);
            }

            if (inputData.SelectedHokenInfRousaiSyobyoDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSyobyoDate);
            }

            if (inputData.SelectedHokenInfRyoyoStartDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoStartDate);
            }

            if (inputData.SelectedHokenInfRyoyoEndDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoEndDate);
            }

            if (inputData.SelectedHokenInfStartDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfStartDate);
            }

            if (inputData.SelectedHokenInfEndDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfEndDate);
            }

            if (inputData.SelectedHokenInfConfirmDate < 0)
            {
                return new ValidateRousaiJibaiOutputData(false, string.Empty, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate);
            }

            return new ValidateRousaiJibaiOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidateRousaiJibaiStatus.InvalidSuccess);
        }    

        private ValidateRousaiJibaiOutputData CommonCheckForRosai(int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int rosaiReceden, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew)
        {
            var message = "";
            if (hokenKbn == 11 || hokenKbn == 12)
            {
                if (listRousaiTenkis.Count > 0)
                {
                    var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                    var itemFirst = rousaiTenkiList.FirstOrDefault();
                    var checkTypeRousaiTenki = 0;
                    // Check Rousai tenki grid default row
                    if (itemFirst != null && (itemFirst.RousaiTenkiSinkei <= 0 && itemFirst.RousaiTenkiTenki <= 0 && (itemFirst.RousaiTenkiEndDate == 0 || itemFirst.RousaiTenkiEndDate == 999999)))
                    {
                        var paramsMessage = new string[] { "新継再別" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckItemFirstListRousaiTenki);
                    }
                    else
                    {
                        string errorMsg = string.Empty;
                        foreach (var rousaiTenki in rousaiTenkiList)
                        {
                            if (rousaiTenki.RousaiTenkiSinkei <= 0)
                            {
                                errorMsg = "新継再別を入力してください。";
                                checkTypeRousaiTenki = 1;
                                break;
                            }
                            else if (rousaiTenki.RousaiTenkiTenki <= 0)
                            {
                                errorMsg = "転帰事由を入力してください。";
                                checkTypeRousaiTenki = 2;
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
                                    checkTypeRousaiTenki = 3;
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            var paramsMessage = new string[] { errorMsg };
                            message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                            switch (checkTypeRousaiTenki)
                            {
                                case 1:
                                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiSinkei);
                                case 2:
                                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiTenki);
                                case 3:
                                    return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiEndDate);
                            }
                        }
                    }
                }
                if (rosaiReceden == 1)
                {
                    if (sHokenInfRousaiSaigaiKbn != 1 && sHokenInfRousaiSaigaiKbn != 2)
                    {
                        var paramsMessage = new string[] { "災害区分" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckRousaiSaigaiKbnNotEquals1And2);

                    }

                    if (sHokenInfRousaiSyobyoDate == 0)
                    {
                        var paramsMessage = new string[] { "傷病年月日" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckRousaiSyobyoDateEquals0);

                    }
                }
            }
            else if (hokenKbn == 13 && string.IsNullOrEmpty(sHokenInfRousaiSyobyoCd))
            {
                var paramsMessage = new string[] { "傷病コード" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull);
            }

            // 労災・療養期間ﾁｪｯｸ
            int rousaiRyoyoStartDate = sHokenInfRyoyoStartDate;
            int rousaiRyoyoEndDate = sHokenInfRyoyoEndDate;
            if (rousaiRyoyoStartDate != 0 && rousaiRyoyoEndDate != 0 && rousaiRyoyoStartDate > rousaiRyoyoEndDate)
            {
                var paramsMessage = new string[] { "労災療養終了日", "労災療養開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull);
            }

            // 労災・有効期限ﾁｪｯｸ
            int rosaiYukoFromDate = sHokenInfStartDate;
            int rosaiYukoToDate = sHokenInfEndDate;
            if (rosaiYukoFromDate != 0 && rosaiYukoToDate != 0 && rosaiYukoFromDate > rosaiYukoToDate)
            {
                var paramsMessage = new string[] { "労災有効終了日", "労災有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckRousaiRyoyoDate);
            }
            // 労災・期限切れﾁｪｯｸ(有効保険の場合のみ)
            if (!DataChkFn10(sinDate, sHokenInfStartDate, sHokenInfEndDate, isAddNew))
            {
                var paramsMessage = new string[] { "労災保険", "無視する", "戻る" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return new ValidateRousaiJibaiOutputData(false, message, TypeMessage.TypeMessageError, ValidateRousaiJibaiStatus.InvalidCheckDateExpirated);
            }

            return new ValidateRousaiJibaiOutputData(true, string.Empty, TypeMessage.TypeMessageSuccess, ValidateRousaiJibaiStatus.InvalidSuccess);
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
    }
}
