using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.SystemConf;
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
            var validateDetails = new List<ResultValidateInsurance<ValidateRousaiJibaiStatus>>();
            try
            {
                CheckValidateInput(ref validateDetails, inputData);

                switch (inputData.HokenKbn)
                {
                    // 労災(短期給付)	
                    case 11:
                        IsValidRodo(ref validateDetails, inputData.SelectedHokenInfRodoBango, inputData.HokenKbn, inputData.ListRousaiTenki, inputData.SelectedHokenInfRousaiSaigaiKbn, inputData.SelectedHokenInfRousaiSyobyoDate, inputData.SelectedHokenInfRousaiSyobyoCd, inputData.SelectedHokenInfRyoyoStartDate, inputData.SelectedHokenInfRyoyoEndDate, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SinDate, inputData.SelectedHokenInfIsAddNew, inputData.HpId);
                        break;
                    // 労災(傷病年金)
                    case 12:
                        IsValidNenkin(ref validateDetails, inputData.SelectedHokenInfNenkinBango, inputData.HokenKbn, inputData.ListRousaiTenki, inputData.SelectedHokenInfRousaiSaigaiKbn, inputData.SelectedHokenInfRousaiSyobyoDate, inputData.SelectedHokenInfRousaiSyobyoCd, inputData.SelectedHokenInfRyoyoStartDate, inputData.SelectedHokenInfRyoyoEndDate, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SinDate, inputData.SelectedHokenInfIsAddNew, inputData.HpId);
                        break;
                    // アフターケア
                    case 13:
                        IsValidKenko(ref validateDetails, inputData.SelectedHokenInfKenkoKanriBango, inputData.HokenKbn, inputData.ListRousaiTenki, inputData.SelectedHokenInfRousaiSaigaiKbn, inputData.SelectedHokenInfRousaiSyobyoDate, inputData.SelectedHokenInfRousaiSyobyoCd, inputData.SelectedHokenInfRyoyoStartDate, inputData.SelectedHokenInfRyoyoEndDate, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SinDate, inputData.SelectedHokenInfIsAddNew, inputData.HpId);
                        break;
                    // 自賠責
                    case 14:
                        IsValidJibai(ref validateDetails, inputData.ListRousaiTenki, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate, inputData.SelectedHokenInfHokenMasterModelIsNull, inputData.SelectedHokenInfIsAddNew, inputData.SinDate);
                        break;
                }
            }
            catch (Exception ex)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidFaild, ex.Message, TypeMessage.TypeMessageError));
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
            return new ValidateRousaiJibaiOutputData(validateDetails);
        }

        private void IsValidRodo(ref List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> validateDetails, string rodoBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var rousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            if (rousaiReceder == 1)
            {
                if (string.IsNullOrEmpty(rodoBango))
                {
                    var paramsMessage = new string[] { "労働保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidRodoBangoNull, message, TypeMessage.TypeMessageError));
                }
                if (rodoBango.Trim().Length != 14)
                {
                    var paramsMessage = new string[] { "労働保険番号", " 14桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidRodoBangoLengthNotEquals14, message, TypeMessage.TypeMessageError));
                }
            }
            CommonCheckForRosai(ref validateDetails, hokenKbn, listRousaiTenkis, rousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
        }

        private void IsValidNenkin(ref List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> validateDetails, string nenkinBago, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufu = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufu == 1)
            {
                if (string.IsNullOrEmpty(nenkinBago))
                {
                    var paramsMessage = new string[] { "年金証書番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidNenkinBangoIsNull, message, TypeMessage.TypeMessageError));
                }
                if (nenkinBago.Trim().Length != 9)
                {
                    var paramsMessage = new string[] { "年金証書番号", " 9桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidNenkinBangoLengthNotEquals9, message, TypeMessage.TypeMessageError));
                }
            }
            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            CommonCheckForRosai(ref validateDetails, hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
        }
        private void IsValidKenko(ref List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> validateDetails, string kenkoKanriBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufuValidate = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufuValidate == 1)
            {
                if (string.IsNullOrEmpty(kenkoKanriBango))
                {
                    var paramsMessage = new string[] { "健康管理手帳番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoIsNull, message, TypeMessage.TypeMessageError));
                }
                if (kenkoKanriBango.Trim().Length != 13)
                {
                    var paramsMessage = new string[] { "健康管理手帳番号", " 13桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoLengthNotEquals13, message, TypeMessage.TypeMessageError));
                }
            }

            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            CommonCheckForRosai(ref validateDetails, hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
        }

        private void IsValidJibai(ref List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> validateDetails, List<RousaiTenkiModel> listRousaiTenkis, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool SelectedHokenInfHokenMasterModelIsNull, bool selectedHokenInfIsAddNew, int sinDate)
        {
            var message = "";
            if (listRousaiTenkis != null && listRousaiTenkis.Count > 0)
            {
                var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                if (rousaiTenkiList.FirstOrDefault()?.RousaiTenkiTenki <= 0)
                {
                    var paramsMessage = new string[] { "転帰事由" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate, message, TypeMessage.TypeMessageError));
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
                        validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate, message, TypeMessage.TypeMessageError));
                    }
                }
            }

            int JibaiYokoFromDate = selectedHokenInfStartDate;
            int JibaiYokoToDate = selectedHokenInfEndDate;
            if (JibaiYokoFromDate != 0 && JibaiYokoToDate != 0 && JibaiYokoFromDate > JibaiYokoToDate)
            {
                var paramsMessage = new string[] { "自賠有効終了日", "自賠有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate, message, TypeMessage.TypeMessageError));
            }

            if (SelectedHokenInfHokenMasterModelIsNull)
            {
                var paramsMessage = new string[] { "負担率" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfHokenMasterModelIsNull, message, TypeMessage.TypeMessageError));
            }

            if (selectedHokenInfIsAddNew)
            {
                if ((selectedHokenInfEndDate > 0 && selectedHokenInfEndDate < sinDate)
                    || (selectedHokenInfStartDate > 0 && selectedHokenInfStartDate > sinDate))
                {

                    var paramsMessage = new string[] { "負担率" };
                    var paramsMessage2 = new string[] { "無視する", "戻る" };
                    message = String.Format(ErrorMessage.MessageType_mChk00020, paramsMessage, paramsMessage2);
                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfHokenMasterModelIsNull, message, TypeMessage.TypeMessageWarning));
                }
            }
        }

        private void CheckValidateInput(ref List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> validateDetails, ValidateRousaiJibaiInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidHpId, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.HokenKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidHokenKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SinDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSinDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRousaiSaigaiKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSaigaiKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRousaiSyobyoDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSyobyoDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRyoyoStartDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoStartDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRyoyoEndDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoEndDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfStartDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfStartDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfEndDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfEndDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfConfirmDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate, string.Empty, TypeMessage.TypeMessageError));
            }
        }

        private void CommonCheckForRosai(ref List<ResultValidateInsurance<ValidateRousaiJibaiStatus>> validateDetails, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int rosaiReceden, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew)
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
                        validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckItemFirstListRousaiTenki, message, TypeMessage.TypeMessageError));
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
                                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiSinkei, message, TypeMessage.TypeMessageError));
                                    break;
                                case 2:
                                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiTenki, message, TypeMessage.TypeMessageError));
                                    break;
                                case 3:
                                    validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiEndDate, message, TypeMessage.TypeMessageError));
                                    break;
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
                        validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckRousaiSaigaiKbnNotEquals1And2, message, TypeMessage.TypeMessageError));
                    }

                    if (sHokenInfRousaiSyobyoDate == 0)
                    {
                        var paramsMessage = new string[] { "傷病年月日" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckRousaiSyobyoDateEquals0, message, TypeMessage.TypeMessageError));
                    }
                }
            }
            else if (hokenKbn == 13 && string.IsNullOrEmpty(sHokenInfRousaiSyobyoCd))
            {
                var paramsMessage = new string[] { "傷病コード" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull, message, TypeMessage.TypeMessageError));
            }

            // 労災・療養期間ﾁｪｯｸ
            int rousaiRyoyoStartDate = sHokenInfRyoyoStartDate;
            int rousaiRyoyoEndDate = sHokenInfRyoyoEndDate;
            if (rousaiRyoyoStartDate != 0 && rousaiRyoyoEndDate != 0 && rousaiRyoyoStartDate > rousaiRyoyoEndDate)
            {
                var paramsMessage = new string[] { "労災療養終了日", "労災療養開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidRousaiRyoyoDate, message, TypeMessage.TypeMessageError));
            }

            // 労災・有効期限ﾁｪｯｸ
            int rosaiYukoFromDate = sHokenInfStartDate;
            int rosaiYukoToDate = sHokenInfEndDate;
            if (rosaiYukoFromDate != 0 && rosaiYukoToDate != 0 && rosaiYukoFromDate > rosaiYukoToDate)
            {
                var paramsMessage = new string[] { "労災有効終了日", "労災有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckRousaiRyoyoDate, message, TypeMessage.TypeMessageError));
            }
            // 労災・期限切れﾁｪｯｸ(有効保険の場合のみ)
            if (!DataChkFn10(sinDate, sHokenInfStartDate, sHokenInfEndDate, isAddNew))
            {
                var paramsMessage = new string[] { "労災保険", "無視する", "戻る" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidateRousaiJibaiStatus>(ValidateRousaiJibaiStatus.InvalidCheckDateExpirated, message, TypeMessage.TypeMessageWarning));
            }
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
