using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Helper.Common;
using UseCase.Insurance.ValidHokenInfAllType;

namespace Interactor.Insurance
{
    public class ValidHokenInfAllTypeInteractor : IValidHokenInfAllTypeInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IPatientInforRepository _patientInforRepository;

        public ValidHokenInfAllTypeInteractor(ISystemConfRepository systemConfRepository, IPatientInforRepository patientInforRepository)
        {
            _systemConfRepository = systemConfRepository;
            _patientInforRepository = patientInforRepository;
        }

        public ValidHokenInfAllTypeOutputData Handle(ValidHokenInfAllTypeInputData inputData)
        {
            var validateDetails = new List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>>();
            try
            {
                // Get HokenMst
                var hokenMst = _patientInforRepository.GetHokenMstByInfor(inputData.SelectedHokenInfHokenNo, inputData.SelectedHokenInfHokenEdraNo);

                string houbetuNo = string.Empty;
                string hokensyaNoSearch = string.Empty;
                CIUtil.GetHokensyaHoubetu(inputData.HokenSyaNo ?? string.Empty, ref hokensyaNoSearch, ref houbetuNo);
                var hokenSyaMst = _patientInforRepository.GetHokenSyaMstByInfor(inputData.HpId, houbetuNo, hokensyaNoSearch);

                CheckValidateInput(ref validateDetails, inputData);
                switch (inputData.HokenKbn)
                {
                    case 0:
                        break;
                    case 1:
                    case 2:
                        if(inputData.HokenKbn == 1 && inputData.SelectedHokenInfHoubetu == HokenConstant.HOUBETU_NASHI)
                        {
                            IsValidHokenNashi(ref validateDetails, inputData.HpId, inputData.SinDate, inputData.SelectedHokenInfTokki1, inputData.SelectedHokenInfTokki2, inputData.SelectedHokenInfTokki3, inputData.SelectedHokenInfTokki4, inputData.SelectedHokenInfTokki5, inputData.SelectedHokenInfStartDate, inputData.SelectedHokenInfEndDate);
                        }
                        else
                        {
                            IsValidHokenInf(ref validateDetails,
                                inputData.SelectedHokenInf,
                                inputData.SelectedHokenInfIsAddNew,
                                inputData.HokenKbn,
                                inputData.SelectedHokenInfHoubetu,
                                inputData.HpId,
                                inputData.SinDate,
                                inputData.SelectedHokenInfTokki1,
                                inputData.SelectedHokenInfTokki2,
                                inputData.SelectedHokenInfTokki3,
                                inputData.SelectedHokenInfTokki4,
                                inputData.SelectedHokenInfTokki5,
                                inputData.SelectedHokenInfStartDate,
                                inputData.SelectedHokenInfEndDate,
                                inputData.SelectedHokenInfIsJihi,
                                inputData.HokenSyaNo ?? string.Empty,
                                hokenMst.HokenNo,
                                inputData.IsSelectedHokenMst,
                                inputData.SelectedHokenInfHoubetu,
                                hokenMst.Houbetu,
                                hokenMst.HokenNo,
                                hokenMst.CheckDigit,
                                inputData.PtBirthday,
                                hokenMst.AgeStart,
                                hokenMst.AgeEnd,
                                inputData.SelectedHokenInfKigo,
                                inputData.SelectedHokenInfBango,
                                hokenSyaMst.IsKigoNa,
                                inputData.SelectedHokenInfHonkeKbn,
                                inputData.SelectedHokenInfStartDate,
                                inputData.SelectedHokenInfEndDate,
                                inputData.SelectedHokenInfTokureiYm1,
                                inputData.SelectedHokenInfTokureiYm2,
                                inputData.SelectedHokenInfisShahoOrKokuho,
                                inputData.SelectedHokenInfisExpirated,
                                inputData.SelectedHokenInfConfirmDate,
                                hokenMst.StartDate,
                                hokenMst.EndDate,
                                hokenMst.DisplayTextMaster,
                                inputData.HokenInfIsNoHoken,
                                inputData.HokenInfConfirmDate,
                                inputData.SelectedHokenInfIsAddHokenCheck,
                                inputData.SelectedHokenInfHokenChecksCount);
                        }
                        break;
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
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidFaild, ex.Message, TypeMessage.TypeMessageError));
            }
            return new ValidHokenInfAllTypeOutputData(validateDetails);
        }

        private void IsValidRodo(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, string rodoBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var rousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            if (rousaiReceder == 1)
            {
                if (string.IsNullOrEmpty(rodoBango))
                {
                    var paramsMessage = new string[] { "労働保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidRodoBangoNull, message, TypeMessage.TypeMessageError));
                }
                if (rodoBango.Trim().Length != 14)
                {
                    var paramsMessage = new string[] { "労働保険番号", " 14桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidRodoBangoLengthNotEquals14, message, TypeMessage.TypeMessageError));
                }
            }
            CommonCheckForRosai(ref validateDetails, hokenKbn, listRousaiTenkis, rousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
        }

        private void IsValidNenkin(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, string nenkinBago, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufu = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufu == 1)
            {
                if (string.IsNullOrEmpty(nenkinBago))
                {
                    var paramsMessage = new string[] { "年金証書番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidNenkinBangoIsNull, message, TypeMessage.TypeMessageError));
                }
                if (nenkinBago.Trim().Length != 9)
                {
                    var paramsMessage = new string[] { "年金証書番号", " 9桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidNenkinBangoLengthNotEquals9, message, TypeMessage.TypeMessageError));
                }
            }
            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            CommonCheckForRosai(ref validateDetails, hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
        }
        private void IsValidKenko(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, string kenkoKanriBango, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew, int hpId)
        {
            var message = "";
            var systemConfigRousaiKufuValidate = (int)_systemConfRepository.GetSettingValue(1006, 0, hpId);
            if (systemConfigRousaiKufuValidate == 1)
            {
                if (string.IsNullOrEmpty(kenkoKanriBango))
                {
                    var paramsMessage = new string[] { "健康管理手帳番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidKenkoKanriBangoIsNull, message, TypeMessage.TypeMessageError));
                }
                if (kenkoKanriBango.Trim().Length != 13)
                {
                    var paramsMessage = new string[] { "健康管理手帳番号", " 13桁" };
                    message = String.Format(ErrorMessage.MessageType_mInp00040, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidKenkoKanriBangoLengthNotEquals13, message, TypeMessage.TypeMessageError));
                }
            }

            var systemConfigRousaiReceder = (int)_systemConfRepository.GetSettingValue(100003, 0, hpId);
            CommonCheckForRosai(ref validateDetails, hokenKbn, listRousaiTenkis, systemConfigRousaiReceder, sHokenInfRousaiSaigaiKbn, sHokenInfRousaiSyobyoDate, sHokenInfRousaiSyobyoCd, sHokenInfRyoyoStartDate, sHokenInfRyoyoEndDate, sHokenInfStartDate, sHokenInfEndDate, sinDate, isAddNew);
        }

        private void IsValidJibai(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, List<RousaiTenkiModel> listRousaiTenkis, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool SelectedHokenInfHokenMasterModelIsNull, bool selectedHokenInfIsAddNew, int sinDate)
        {
            var message = "";
            if (listRousaiTenkis != null && listRousaiTenkis.Count > 0)
            {
                var rousaiTenkiList = listRousaiTenkis.Where(r => r.RousaiTenkiIsDeleted == 0);
                if (rousaiTenkiList.FirstOrDefault()?.RousaiTenkiTenki <= 0)
                {
                    var paramsMessage = new string[] { "転帰事由" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfConfirmDate, message, TypeMessage.TypeMessageError));
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
                        validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfConfirmDate, message, TypeMessage.TypeMessageError));
                    }
                }
            }

            int JibaiYokoFromDate = selectedHokenInfStartDate;
            int JibaiYokoToDate = selectedHokenInfEndDate;
            if (JibaiYokoFromDate != 0 && JibaiYokoToDate != 0 && JibaiYokoFromDate > JibaiYokoToDate)
            {
                var paramsMessage = new string[] { "自賠有効終了日", "自賠有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfConfirmDate, message, TypeMessage.TypeMessageError));
            }

            if (SelectedHokenInfHokenMasterModelIsNull)
            {
                var paramsMessage = new string[] { "負担率" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfHokenMasterModelIsNull, message, TypeMessage.TypeMessageError));
            }

            if (selectedHokenInfIsAddNew)
            {
                if ((selectedHokenInfEndDate > 0 && selectedHokenInfEndDate < sinDate)
                    || (selectedHokenInfStartDate > 0 && selectedHokenInfStartDate > sinDate))
                {

                    var paramsMessage = new string[] { "負担率" };
                    var paramsMessage2 = new string[] { "無視する", "戻る" };
                    message = String.Format(ErrorMessage.MessageType_mChk00020, paramsMessage, paramsMessage2);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfHokenMasterModelIsNull, message, TypeMessage.TypeMessageWarning));
                }
            }
        }

        private void CheckValidateInput(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, ValidHokenInfAllTypeInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHpId, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.HokenKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SinDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSinDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRousaiSaigaiKbn < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRousaiSaigaiKbn, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRousaiSyobyoDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRousaiSyobyoDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRyoyoStartDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRyoyoStartDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfRyoyoEndDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfRyoyoEndDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfStartDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfStartDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfEndDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfEndDate, string.Empty, TypeMessage.TypeMessageError));
            }

            if (inputData.SelectedHokenInfConfirmDate < 0)
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidSelectedHokenInfConfirmDate, string.Empty, TypeMessage.TypeMessageError));
            }
        }

        private void CommonCheckForRosai(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, int hokenKbn, List<RousaiTenkiModel> listRousaiTenkis, int rosaiReceden, int sHokenInfRousaiSaigaiKbn, int sHokenInfRousaiSyobyoDate, string sHokenInfRousaiSyobyoCd, int sHokenInfRyoyoStartDate, int sHokenInfRyoyoEndDate, int sHokenInfStartDate, int sHokenInfEndDate, int sinDate, bool isAddNew)
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
                        validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckItemFirstListRousaiTenki, message, TypeMessage.TypeMessageError));
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
                                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckRousaiTenkiSinkei, message, TypeMessage.TypeMessageError));
                                    break;
                                case 2:
                                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckRousaiTenkiTenki, message, TypeMessage.TypeMessageError));
                                    break;
                                case 3:
                                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckRousaiTenkiEndDate, message, TypeMessage.TypeMessageError));
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
                        validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckRousaiSaigaiKbnNotEquals1And2, message, TypeMessage.TypeMessageError));
                    }

                    if (sHokenInfRousaiSyobyoDate == 0)
                    {
                        var paramsMessage = new string[] { "傷病年月日" };
                        message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                        validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckRousaiSyobyoDateEquals0, message, TypeMessage.TypeMessageError));
                    }
                }
            }
            else if (hokenKbn == 13 && string.IsNullOrEmpty(sHokenInfRousaiSyobyoCd))
            {
                var paramsMessage = new string[] { "傷病コード" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull, message, TypeMessage.TypeMessageError));
            }

            // 労災・療養期間ﾁｪｯｸ
            int rousaiRyoyoStartDate = sHokenInfRyoyoStartDate;
            int rousaiRyoyoEndDate = sHokenInfRyoyoEndDate;
            if (rousaiRyoyoStartDate != 0 && rousaiRyoyoEndDate != 0 && rousaiRyoyoStartDate > rousaiRyoyoEndDate)
            {
                var paramsMessage = new string[] { "労災療養終了日", "労災療養開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidRousaiRyoyoDate, message, TypeMessage.TypeMessageError));
            }

            // 労災・有効期限ﾁｪｯｸ
            int rosaiYukoFromDate = sHokenInfStartDate;
            int rosaiYukoToDate = sHokenInfEndDate;
            if (rosaiYukoFromDate != 0 && rosaiYukoToDate != 0 && rosaiYukoFromDate > rosaiYukoToDate)
            {
                var paramsMessage = new string[] { "労災有効終了日", "労災有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckRousaiRyoyoDate, message, TypeMessage.TypeMessageError));
            }
            // 労災・期限切れﾁｪｯｸ(有効保険の場合のみ)
            if (!DataChkFn10(sinDate, sHokenInfStartDate, sHokenInfEndDate, isAddNew))
            {
                var paramsMessage = new string[] { "労災保険", "無視する", "戻る" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckDateExpirated, message, TypeMessage.TypeMessageWarning));
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

        private void IsValidHokenInf(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, bool selectedHokenInf, bool selectedHokenInfIsAddNew, int hokenKbn, string selectedHokenInfHoubetu, int hpId, int sinDate, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsJihi, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd, string kigo, string bango, int hokenMstIsKigoNa, int honkeKbn, int startDate, int endDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfconfirmDate, int selectedHokenMstStartDate, int selectedHokenMstEndDate, string selectedHokenMstDisplayText,bool hokenInfIsNoHoken, int hokenInfConfirmDate,bool selectedHokenInfIsAddHokenCheck,int selectedHokenInfHokenChecksCount)
        {
            var message = "";
            if (!selectedHokenInf)
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
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokensyaNoNull, message, TypeMessage.TypeMessageError));
            }
            if (hokenNo == 0)
            {
                var paramsMessage = new string[] { "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenNoEquals0, message, TypeMessage.TypeMessageError));
            }
            if (string.IsNullOrEmpty(hokenSyaNo) || Int32.Parse(hokenSyaNo) == 0)
            {
                var paramsMessage = new string[] { "保険者番号は 0 〜 9 の範囲で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokensyaNoEquals0, message, TypeMessage.TypeMessageError));
            }
            // 記号
            if (hokenSyaNo.Length == 8 && hokenSyaNo.Trim().StartsWith("39"))
            {
                if (!string.IsNullOrEmpty(kigo)
                    && !string.IsNullOrEmpty(kigo.Trim(' '))) //Trim only half-size space
                {
                    var paramsMessage = new string[] { "後期高齢者の", "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00150, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokensyaNoLength8StartWith39, message, TypeMessage.TypeMessageError));
                }
            }
            else
            {
                if (hokenMstIsKigoNa == 0 && (string.IsNullOrEmpty(kigo)
                    || string.IsNullOrEmpty(kigo.Trim(' '))))
                {
                    var paramsMessage = new string[] { "被保険者証記号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidKigoNull, message, TypeMessage.TypeMessageError));
                }
            }
            if (string.IsNullOrEmpty(bango)
                    || string.IsNullOrEmpty(bango.Trim(' '))) //Trim only half-size space
            {
                var paramsMessage = new string[] { "被保険者証番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidBangoNull, message, TypeMessage.TypeMessageError));
            }
            if (honkeKbn == 0)
            {
                var paramsMessage = new string[] { "本人家族区分" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenKbnEquals0, message, TypeMessage.TypeMessageError));
            }

            IsValidYukoKigen(ref validateDetails, startDate, endDate);
            IsValidTokkurei(ref validateDetails, ptBirthday, sinDate, selectedHokenInfTokureiYm1, selectedHokenInfTokureiYm2, hokenSyaNo, selectedHokenInfisShahoOrKokuho, selectedHokenInfisExpirated);

            string checkMessageIsValidConfirmDateAgeCheck = IsValidConfirmDateAgeCheck(selectedHokenInfIsAddNew, selectedHokenInfisExpirated, selectedHokenInfisShahoOrKokuho, hokenSyaNo, selectedHokenInfconfirmDate, ptBirthday, sinDate, hpId);
            if (!string.IsNullOrEmpty(checkMessageIsValidConfirmDateAgeCheck))
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidConfirmDateAgeCheck, checkMessageIsValidConfirmDateAgeCheck, TypeMessage.TypeMessageError));
            }

            string checkMessageIsValidConfirmDateHoken = IsValidConfirmDateHoken(sinDate,
                                                            selectedHokenInfisExpirated,
                                                            selectedHokenInfIsJihi,
                                                            hokenInfIsNoHoken,
                                                            selectedHokenInfisExpirated,
                                                            hokenInfConfirmDate,
                                                            selectedHokenInfIsAddNew,
                                                            selectedHokenInfIsAddHokenCheck,
                                                            selectedHokenInfHokenChecksCount);
            if (!string.IsNullOrEmpty(checkMessageIsValidConfirmDateHoken))
            {
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InValidConfirmDateHoken, checkMessageIsValidConfirmDateAgeCheck, TypeMessage.TypeMessageConfirmation));
            }

            // check valid hokenmst date
            IsValidHokenMstDate(ref validateDetails, selectedHokenInfStartDate, selectedHokenInfEndDate, sinDate, selectedHokenMstDisplayText, selectedHokenMstStartDate, selectedHokenMstEndDate);
        }

        private void IsValidHokenNashi(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, int hpId, int sinDate, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int startDate, int endDate)
        {
            IsValidHokenDetail(ref validateDetails, hpId, sinDate, tokki1, tokki2, tokki3, tokki4, tokki5);

            IsValidYukoKigen(ref validateDetails, startDate, endDate);
        }

        private void IsValidYukoKigen(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, int selectedHokenInfStartDate, int selectedHokenInfEndDate)
        {
            var message = "";
            int yukoFromDate = selectedHokenInfStartDate;
            int yukoToDate = selectedHokenInfEndDate;
            if (yukoFromDate != 0 && yukoToDate != 0 && yukoFromDate > yukoToDate)
            {
                var paramsMessage = new string[] { "保険有効終了日", "保険有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidYukoKigen, message, TypeMessage.TypeMessageError));
            }
        }

        private void IsValidHokenDetail(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, int hpId, int sinDate, string tokki1Value, string tokki2Value, string tokki3Value, string tokki4Value, string tokki5Value)
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
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue1, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki2Value) && tokki2Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki2Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue21, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue31, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue41, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki1Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue51, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki2Value))
            {
                if (!_isValidLengthTokki(tokki2Value))
                {
                    var paramsMessage = new string[] { "特記事項２", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue2, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki3Value) && tokki3Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki3Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue23, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue24, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki2Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue25, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki3Value))
            {
                if (!_isValidLengthTokki(tokki3Value))
                {
                    var paramsMessage = new string[] { "特記事項３", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue3, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki4Value) && tokki4Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki4Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue34, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki3Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue35, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki4Value))
            {
                if (!_isValidLengthTokki(tokki4Value))
                {
                    var paramsMessage = new string[] { "特記事項４", "2文字" };
                    message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue4, message, TypeMessage.TypeMessageError));
                }
                if (!string.IsNullOrEmpty(tokki5Value) && tokki5Value == tokki4Value)
                {
                    var paramsMessage = new string[] { "特記事項'" + tokki5Value + "'" };
                    message = String.Format(ErrorMessage.MessageType_mUnq00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue45, message, TypeMessage.TypeMessageError));
                }
            }
            if (!string.IsNullOrEmpty(tokki5Value) && !_isValidLengthTokki(tokki5Value))
            {
                var paramsMessage = new string[] { "特記事項５", "2文字" };
                message = String.Format(ErrorMessage.MessageType_mInp00080, paramsMessage);
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkiValue5, message, TypeMessage.TypeMessageError));
            }
        }


        private void CHKHokno_Fnc(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, string hokenSyaNo, int hokenNo, bool isHaveSelectedHokenMst, string houbetu, string sHokenMstHoubetsuNumber, int sHokenMstHokenNumber, int sHokenMstCheckDegit, int ptBirthday, int sHokenMstAgeStart, int sHokenMstAgeEnd)
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
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0, message, TypeMessage.TypeMessageError));
                }
            }
            //保険番号入力あり
            else
            {
                if (hokenNo == 0)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenNoEquals0, message, TypeMessage.TypeMessageError));
                }
                if (!isHaveSelectedHokenMst)
                {
                    var paramsMessage = new string[] { "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenNoHaveHokenMst, message, TypeMessage.TypeMessageError));
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
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHoubetu, message, TypeMessage.TypeMessageError));
                }
                //チェックデジット
                if (sHokenMstCheckDegit == 1 && !CIUtil.HokenNumberCheckDigits(Int32.Parse(hokenSyaNo)))
                {
                    var paramsMessage = new string[] { "保険者番号" };
                    message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckDigitEquals1, message, TypeMessage.TypeMessageError));
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
                        validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidCheckAgeHokenMst, message, TypeMessage.TypeMessageError));
                    }
                }
            }
        }


        private void IsValidTokkurei(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, int ptBirthday, int sinDate, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, string hokenSyaNo, bool isShahoOrKokuho, bool isExpirated)
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
                validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidTokkurei, message, TypeMessage.TypeMessageError));
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

        private void IsValidHokenMstDate(ref List<ResultValidateInsurance<ValidHokenInfAllTypeStatus>> validateDetails, int startDate, int endDate, int sinDate, string displayTextMaster, int selecttedHokenMstStartDate, int selecttedHokenMstEndDate)
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
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenMstStartDate, message, TypeMessage.TypeMessageConfirmation));
                }
                if (selecttedHokenMstEndDate < sinDate)
                {
                    var paramsMessage = new string[] { "主保険 '" + displayTextMaster + "' の適用期間外です。" + "\n\r" + " (～"
                            + CIUtil.SDateToShowSDate(selecttedHokenMstEndDate) + ")", "保険番号" };
                    message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                    validateDetails.Add(new ResultValidateInsurance<ValidHokenInfAllTypeStatus>(ValidHokenInfAllTypeStatus.InvalidHokenMstEndDate, message, TypeMessage.TypeMessageConfirmation));
                }
            }
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

        public string IsValidConfirmDateHoken(int sinDate, bool isExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, bool hokenInfIsExpirated, int hokenInfConfirmDate, bool selectedHokenInfIsAddNew, bool selectedHokenInfIsAddHokenCheck,int selectedHokenInfHokenChecksCount)
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
