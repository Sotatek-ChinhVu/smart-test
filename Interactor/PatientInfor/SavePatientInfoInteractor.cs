using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using UseCase.PatientInfor.Save;

namespace Interactor.PatientInfor
{
    public class SavePatientInfoInteractor : ISavePatientInfoInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public SavePatientInfoInteractor(IPatientInforRepository patientInforRepository, ISystemConfRepository systemConfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _systemConfRepository = systemConfRepository;
        }

        public SavePatientInfoOutputData Handle(SavePatientInfoInputData inputData)
        {
            var validations = Validation(inputData);
            if (validations.Any())
            {
                return new SavePatientInfoOutputData(validations, SavePatientInfoStatus.Failed, 0);
            }
            try
            {
                (bool, long) result;
                if (inputData.Patient.PtId == 0)
                    result = _patientInforRepository.CreatePatientInfo(inputData.Patient, inputData.PtKyuseis, inputData.PtSanteis, inputData.Insurances, inputData.HokenInfs, inputData.HokenKohis, inputData.PtGrps, inputData.UserId);
                else
                    result = _patientInforRepository.UpdatePatientInfo(inputData.Patient, inputData.PtKyuseis, inputData.PtSanteis, inputData.Insurances, inputData.HokenInfs, inputData.HokenKohis, inputData.PtGrps, inputData.UserId);

                if (result.Item1)
                    return new SavePatientInfoOutputData(new List<SavePatientInfoValidationResult>(), SavePatientInfoStatus.Successful, result.Item2);
                else
                    return new SavePatientInfoOutputData(new List<SavePatientInfoValidationResult>(), SavePatientInfoStatus.Failed, 0);
            }
            catch
            {
                return new SavePatientInfoOutputData(new List<SavePatientInfoValidationResult>(), SavePatientInfoStatus.Failed, 0);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }

        private IEnumerable<SavePatientInfoValidationResult> Validation(SavePatientInfoInputData model)
        {
            var resultMessages = new List<SavePatientInfoValidationResult>();

            #region Patient Info

            if (model.Patient.HpId <= 0)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HpId`"), SavePatientInforValidationCode.InvalidHpId, TypeMessage.TypeMessageError));

            if (model.Patient.Name != null && model.Patient.Name.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Name`"), SavePatientInforValidationCode.InvalidName, TypeMessage.TypeMessageError));

            if (model.Patient.KanaName != null && model.Patient.KanaName.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.KanaName`"), SavePatientInforValidationCode.InvalidKanaName, TypeMessage.TypeMessageError));

            resultMessages.AddRange(IsValidKanjiName(model.Patient.KanaName ?? string.Empty, model.Patient.Name ?? string.Empty, model.Patient.HpId));
            int sinDay = DateTime.Now.ToString("yyyyMMdd").AsInteger();
            resultMessages.AddRange(IsValidHokenPatternAll(model.Insurances, model.HokenInfs, model.HokenKohis, model.Patient.PtId != 0, model.Patient.Birthday, sinDay , model.ReactSave, model.Patient.MainHokenPid));

            if (model.Patient.IsDead < 0 || model.Patient.IsDead > 1)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.IsDead`"), SavePatientInforValidationCode.InvalidIsDead, TypeMessage.TypeMessageError));

            if (model.Patient.IsDead == 1 && model.Patient.DeathDate == 0)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.DeathDate`"), SavePatientInforValidationCode.InvalidDeathDate, TypeMessage.TypeMessageError));

            if (model.Patient.HomePost != null && model.Patient.HomePost.Length > 7)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomePost`"), SavePatientInforValidationCode.InvalidHomePost, TypeMessage.TypeMessageError));

            if (model.Patient.HomeAddress1 != null && model.Patient.HomeAddress1.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomeAddress1`"), SavePatientInforValidationCode.InvalidHomeAddress1, TypeMessage.TypeMessageError));

            if (model.Patient.HomeAddress2 != null && model.Patient.HomeAddress2.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomeAddress2`"), SavePatientInforValidationCode.InvalidHomeAddress2, TypeMessage.TypeMessageError));

            if (model.Patient.Tel1 != null && model.Patient.Tel1.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Tel1`"), SavePatientInforValidationCode.InvalidTel1, TypeMessage.TypeMessageError));

            if (model.Patient.Tel2 != null && model.Patient.Tel2.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Tel2`"), SavePatientInforValidationCode.InvalidTel2, TypeMessage.TypeMessageError));

            if (model.Patient.Mail != null && model.Patient.Mail.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Mail`"), SavePatientInforValidationCode.InvalidMail, TypeMessage.TypeMessageError));

            if (model.Patient.Setanusi != null && model.Patient.Setanusi.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Setanusi`"), SavePatientInforValidationCode.InvalidSetanusi, TypeMessage.TypeMessageError));

            if (model.Patient.Zokugara != null && model.Patient.Zokugara.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Zokugara`"), SavePatientInforValidationCode.InvalidZokugara, TypeMessage.TypeMessageError));

            if (model.Patient.Job != null && model.Patient.Job.Length > 40)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Job`"), SavePatientInforValidationCode.InvalidJob, TypeMessage.TypeMessageError));

            if (model.Patient.RenrakuPost != null && model.Patient.RenrakuPost.Length > 7)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuPost`"), SavePatientInforValidationCode.InvalidRenrakuPost, TypeMessage.TypeMessageError));

            if (model.Patient.RenrakuAddress1 != null && model.Patient.RenrakuAddress1.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuAddress1`"), SavePatientInforValidationCode.InvalidRenrakuAddress1, TypeMessage.TypeMessageError));

            if (model.Patient.RenrakuAddress2 != null && model.Patient.RenrakuAddress2.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuAddress2`"), SavePatientInforValidationCode.InvalidRenrakuAddress2, TypeMessage.TypeMessageError));

            if (model.Patient.RenrakuTel != null && model.Patient.RenrakuTel.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuTel`"), SavePatientInforValidationCode.InvalidRenrakuTel, TypeMessage.TypeMessageError));

            if (model.Patient.RenrakuMemo != null && model.Patient.RenrakuMemo.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuMemo`"), SavePatientInforValidationCode.InvalidRenrakuMemo, TypeMessage.TypeMessageError));

            if (model.Patient.OfficeName != null && model.Patient.OfficeName.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeName`"), SavePatientInforValidationCode.InvalidOfficeName, TypeMessage.TypeMessageError));

            if (model.Patient.OfficePost != null && model.Patient.OfficePost.Length > 7)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficePost`"), SavePatientInforValidationCode.InvalidOfficePost, TypeMessage.TypeMessageError));

            if (model.Patient.OfficeAddress1 != null && model.Patient.OfficeAddress1.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeAddress1`"), SavePatientInforValidationCode.InvalidOfficeAddress1, TypeMessage.TypeMessageError));

            if (model.Patient.OfficeAddress2 != null && model.Patient.OfficeAddress2.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeAddress2`"), SavePatientInforValidationCode.InvalidOfficeAddress2, TypeMessage.TypeMessageError));

            if (model.Patient.OfficeTel != null && model.Patient.OfficeTel.Length > 15)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeTel`"), SavePatientInforValidationCode.InvalidOfficeTel, TypeMessage.TypeMessageError));

            if (model.Patient.OfficeMemo != null && model.Patient.OfficeMemo.Length > 100)
                resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeMemo`"), SavePatientInforValidationCode.InvalidOfficeMemo, TypeMessage.TypeMessageError));

            #endregion Patient Info

            #region Hoken

            var listHokenIdValid = model.HokenInfs.Where(x => x.IsDeleted == 0)
                                    .Select(x => x.HokenId).Where(x => x != 0).ToList();

            var listHokenKohiIdValid = model.HokenKohis.Where(x => x.IsDeleted == 0)
                                    .Select(x => x.HokenId).Where(x => x != 0).ToList();

            for (int i = 0; i < model.Insurances.Count; i++)
            {
                if (model.Insurances[i].HokenId != 0 && !listHokenIdValid.Contains(model.Insurances[i].HokenId))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].HokenId`"), SavePatientInforValidationCode.ÍnuranceInvalidHokenId, TypeMessage.TypeMessageError));

                if (model.Insurances[i].Kohi1Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi1Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi1Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi1Id, TypeMessage.TypeMessageError));

                if (model.Insurances[i].Kohi2Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi2Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi2Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi2Id, TypeMessage.TypeMessageError));

                if (model.Insurances[i].Kohi3Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi3Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi3Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi3Id, TypeMessage.TypeMessageError));

                if (model.Insurances[i].Kohi4Id != 0 && !listHokenKohiIdValid.Contains(model.Insurances[i].Kohi4Id))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`Insurances[{i}].Kohi4Id`"), SavePatientInforValidationCode.ÍnuranceInvalidKohi4Id, TypeMessage.TypeMessageError));
            }

            #endregion Hoken

            #region PtKytsei

            for (int i = 0; i < model.PtKyuseis.Count; i++)
            {
                if (model.PtKyuseis[i].SeqNo < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].SeqNo`"), SavePatientInforValidationCode.PtKyuseiInvalidSeqNo, TypeMessage.TypeMessageError));

                if (string.IsNullOrEmpty(model.PtKyuseis[i].KanaName))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtKyuseis[{i}].KanaName`"), SavePatientInforValidationCode.PtKyuseiInvalidKanaName, TypeMessage.TypeMessageError));

                if (model.PtKyuseis[i].KanaName.Length > 100)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].KanaName`"), SavePatientInforValidationCode.PtKyuseiInvalidKanaName, TypeMessage.TypeMessageError));

                if (string.IsNullOrEmpty(model.PtKyuseis[i].Name))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtKyuseis[{i}].Name`"), SavePatientInforValidationCode.PtKyuseiInvalidName, TypeMessage.TypeMessageError));

                if (model.PtKyuseis[i].Name.Length > 100)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].Name`"), SavePatientInforValidationCode.PtKyuseiInvalidName, TypeMessage.TypeMessageError));

                if (model.PtKyuseis[i].EndDate < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].EndDate`"), SavePatientInforValidationCode.PtKyuseiInvalidEndDate, TypeMessage.TypeMessageError));
            }

            #endregion PtKytsei

            #region PtSanteis

            for (int i = 0; i < model.PtSanteis.Count; i++)
            {
                if (model.PtSanteis[i].SeqNo < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].SeqNo`"), SavePatientInforValidationCode.PtSanteiInvalidSeqNo, TypeMessage.TypeMessageError));

                if (model.PtSanteis[i].EdaNo < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].EdaNo`"), SavePatientInforValidationCode.PtSanteiInvalidEdaNo, TypeMessage.TypeMessageError));

                if (model.PtSanteis[i].KbnVal < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].KbnVal`"), SavePatientInforValidationCode.PtSanteiInvalidKbnVal, TypeMessage.TypeMessageError));;

                if (model.PtSanteis[i].StartDate < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].StartDate`"), SavePatientInforValidationCode.PtSanteiInvalidStartDate, TypeMessage.TypeMessageError));

                if (model.PtSanteis[i].EndDate < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].EndDate`"), SavePatientInforValidationCode.PtSanteiInvalidEndDate, TypeMessage.TypeMessageError));
            }

            #endregion PtSanteis

            #region PtGrps
            for (int i = 0; i < model.PtGrps.Count; i++)
            {
                if (model.PtGrps[i].GroupId < 0)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtGrps[{i}].GroupId`"), SavePatientInforValidationCode.PtGrpInvalidGroupId, TypeMessage.TypeMessageError));

                if (string.IsNullOrEmpty(model.PtGrps[i].GroupCode))
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtGrps[{i}].GroupCode`"), SavePatientInforValidationCode.PtGrpInvalidGroupCode, TypeMessage.TypeMessageError));

                if (model.PtGrps[i].GroupCode.Length > 4)
                    resultMessages.Add(new SavePatientInfoValidationResult(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtGrps[{i}].GroupCode`"), SavePatientInforValidationCode.PtGrpInvalidGroupCode, TypeMessage.TypeMessageError));
            }
            #endregion PtGrps

            return resultMessages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kanaName">Kana Name</param>
        /// <param name="kanjiName">Name</param>
        /// <param name="hpId"></param>
        /// <returns></returns>
        private IEnumerable<SavePatientInfoValidationResult> IsValidKanjiName(string kanaName , string kanjiName, int hpId)
        {

            var resultMessages = new List<SavePatientInfoValidationResult>();
            SplitName(kanaName, out string firstNameKana, out string lastNameKana);
            SplitName(kanjiName, out string firstNameKanji, out string lastNameKanji);
            bool isValidateFullName = _systemConfRepository.GetSettingValue(1017, 0, hpId) == 0;

            string message = string.Empty;
            if (string.IsNullOrEmpty(firstNameKana))
            {
                message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "カナ" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidFirstNameKana, TypeMessage.TypeMessageError));
            }

            if (string.IsNullOrEmpty(firstNameKanji))
            {
                message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "氏名" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidFirstNameKanji, TypeMessage.TypeMessageError));
            }

            // validate full name if setting
            if (isValidateFullName)
            {
                if (string.IsNullOrEmpty(lastNameKana))
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "カナ" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidLastKanaName, TypeMessage.TypeMessageError));
                }

                if (string.IsNullOrEmpty(lastNameKanji))
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00010, new string[] { "氏名" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidLastKanjiName, TypeMessage.TypeMessageError));
                }
            }

            int FKanNmChkJIS = (int)_systemConfRepository.GetSettingValue(1003, 0, hpId);

            // 患者氏名チェック（受付）※JISコード
            if (FKanNmChkJIS > 0)
            {
                // 患者名_漢字 JisｺｰﾄﾞCheck
                string sBuf2 = string.Empty;
                string sBuf = CIUtil.Chk_JISKj(firstNameKanji, out sBuf2);
                if (!string.IsNullOrEmpty(sBuf))
                {
                    if (FKanNmChkJIS == 1)
                    {
                        message = "漢字名に '" + sBuf + "' の文字が入力されています。" + "\n\r" + "登録しますか？";
                        resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidJiscodeCheck, TypeMessage.TypeMessageWarning));
                    }
                    else if (FKanNmChkJIS == 2)
                    {
                        message = string.Format(ErrorMessage.MessageType_mInp00140, new string[] { "漢字名", "'" + sBuf + "'" + " の文字" });
                        resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidChineseCharacterName, TypeMessage.TypeMessageError));
                    }
                }
                if (isValidateFullName)
                {
                    sBuf2 = string.Empty;
                    // 患者姓_漢字 JisｺｰﾄﾞCheck
                    sBuf = CIUtil.Chk_JISKj(lastNameKanji, out sBuf2);
                    if (!string.IsNullOrEmpty(sBuf))
                    {
                        if (FKanNmChkJIS == 1)
                        {
                            message = "漢字姓に '" + sBuf + "' の文字が入力されています。" + "\n\r" + "登録しますか？";
                            resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidJiscodeCheck, TypeMessage.TypeMessageWarning));
                        }
                        else if (FKanNmChkJIS == 2)
                        {
                            message = string.Format(ErrorMessage.MessageType_mInp00140, new string[] { "漢字姓", "'" + sBuf + "'" + " の文字" });
                            resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidChineseCharacterName, TypeMessage.TypeMessageError));
                        }
                    }
                }
            }

            if (firstNameKana.Length > 20)
            {
                message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者名（カナ）は２０文字以下を入力してください。" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidFirstNameKanaLength, TypeMessage.TypeMessageError));
            }

            if (firstNameKanji.Length > 30)
            {
                message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者名は３０文字以下を入力してください。" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidFirstNameKanjiLength, TypeMessage.TypeMessageError));
            }

            if (isValidateFullName)
            {
                if (lastNameKana.Length > 20)
                {
                    message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者姓（カナ）は２０文字以下を入力してください。" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidLastKanaNameLength, TypeMessage.TypeMessageError));
                }

                if (lastNameKanji.Length > 30)
                {
                    message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { "患者姓は３０文字以下を入力してください。" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidLastKanjiNameLength, TypeMessage.TypeMessageError));
                }
            }
            return resultMessages;
        }

        private void SplitName(string name, out string firstName, out string lastName)
        {
            firstName = "";
            lastName = "";
            char[] arraySpace = { '　', ' ' };
            if (!string.IsNullOrEmpty(name))
            {
                if (!arraySpace.Any(u => name.Any(c => c == u)))
                {
                    firstName = name;
                    return;
                }
                for (int i = 0; i < arraySpace.Length; i++)
                {
                    var arrayName = name.Split(arraySpace[i]);
                    if (arrayName != null)
                    {
                        if (arrayName.Length < 2 && arrayName.Length > 0)
                        {
                            continue;
                        }
                        else if (arrayName.Length >= 2)
                        {
                            int index = name.IndexOf(arraySpace[i]);
                            lastName = name.Substring(0, index);
                            firstName = name.Substring(index + 1).Trim();
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private bool IsValidAgeCheckConfirm(int ageCheck, int confirmDate, int birthDay, int sinDay)
        {
            // 但し、2日生まれ以降の場合は翌月１日を誕生日とする。
            if (CIUtil.Copy(birthDay.AsString(), 7, 2) != "01")
            {
                int firstDay = birthDay / 100 * 100 + 1;
                int nextMonth = CIUtil.DateTimeToInt(CIUtil.IntToDate(firstDay).AddMonths(1));
                birthDay = nextMonth;
            }

            if (CIUtil.AgeChk(birthDay, sinDay, ageCheck)
                && !CIUtil.AgeChk(birthDay, confirmDate, ageCheck))
            {
                return false;
            }
            return true;
        }

        private IEnumerable<SavePatientInfoValidationResult> IsValidAgeCheck(
            List<InsuranceModel> insurances,
            int birthDay,
            int sinDay,
            ReactSavePatientInfo reactFromUI)
        {
            var resultMessages = new List<SavePatientInfoValidationResult>();
            if (_systemConfRepository.GetSettingValue(1005, 0, 0) == 1)
            {
                var validPattern = insurances?.Where(pattern => pattern.IsDeleted == DeleteTypes.None &&
                                                                    !pattern.IsExpirated &&
                                                                    !pattern.IsAddNew &&
                                                                    !pattern.IsEmptyHoken &&
                                                                    pattern.HokenInf.IsShahoOrKokuho &&
                                                                    !(pattern.HokenInf.HokensyaNo.Length == 8
                                                                        && (pattern.HokenInf.HokensyaNo.StartsWith("109") || pattern.HokenInf.HokensyaNo.StartsWith("99"))));

                if (validPattern == null || validPattern.Count() == 0)
                {
                    return resultMessages;
                }

                string checkParam = _systemConfRepository.GetSettingParams(1005, 0, 0);
                var splittedParam = checkParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int invalidAgeCheck = 0;
                foreach (var param in splittedParam)
                {
                    int ageCheck = param.Trim().AsInteger();
                    if (ageCheck == 0) continue;

                    foreach (var pattern in validPattern)
                    {
                        int confirmDate = pattern.HokenInf.ConfirmDate;
                        if (!IsValidAgeCheckConfirm(ageCheck, pattern.HokenInf.ConfirmDate, birthDay , sinDay))
                        {
                            if (invalidAgeCheck <= ageCheck)
                            {
                                invalidAgeCheck = ageCheck;
                            }
                        }
                    }
                }

                if (invalidAgeCheck != 0 && !reactFromUI.ConfirmAgeCheck)
                {
                    string cardName;
                    int age = CIUtil.SDateToAge(birthDay, sinDay);
                    if (age >= 70)
                    {
                        cardName = "高齢受給者証";
                    }
                    else
                    {
                        cardName = "保険証";
                    }

                    string message = string.Format(ErrorMessage.MessageType_mChk00080, new string[] { $"{invalidAgeCheck}歳となりました。", cardName },
                        new string[] { "無視する", "戻る" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningAgeCheck, TypeMessage.TypeMessageWarning));
                }
            }
            return resultMessages;
        }

        private IEnumerable<SavePatientInfoValidationResult> HasElderHoken(
            List<InsuranceModel> insurances, 
            List<HokenInfModel> hokenInfs, 
            int birthDay, 
            int sinDay,
            ReactSavePatientInfo reactFromUI)
        {
            var resultMessages = new List<SavePatientInfoValidationResult>();
            if (sinDay >= 20080401)
            {
                if (insurances != null && insurances.Count > 0)
                {
                    var PatternHokenOnly = insurances.Where(pattern => pattern.IsDeleted == 0 && pattern.IsExpirated == false);

                    int age = CIUtil.SDateToAge(birthDay, sinDay);

                    // hoken exist in at least 1 pattern
                    var inUsedHokens = hokenInfs.Where(hoken => hoken.HokenId > 0 && hoken.IsDeleted == 0 && hoken.IsExpirated == false
                                                                && PatternHokenOnly.Any(pattern => pattern.HokenId == hoken.HokenId));

                    var elderHokenQuery = inUsedHokens.Where(hoken => hoken.EndDate >= sinDay
                                                                        && hoken.HokensyaNo != null && hoken.HokensyaNo != ""
                                                                        && hoken.HokensyaNo.Length == 8 && hoken.HokensyaNo.StartsWith("39"));

                    string message = string.Empty;
                    if (elderHokenQuery != null)
                    {
                        if (age >= 75 && elderHokenQuery.Count() == 0 && !reactFromUI.ConfirmInsuranceElderlyLaterNotYetCovered)
                        {
                            message = string.Format(ErrorMessage.MessageType_mChk00080, new string[] { "後期高齢者保険が入力されていません。", "保険者証" },
                                    new string[] { "無視する", "戻る" });
                            resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningInsuranceElderlyLaterNotYetCovered, TypeMessage.TypeMessageWarning));
                        }
                        else if (age < 65 && elderHokenQuery.Count() > 0 && !reactFromUI.ConfirmLaterInsuranceRegisteredPatientsElderInsurance)
                        {
                            message = string.Format(ErrorMessage.MessageType_mChk00080, new string[] { "後期高齢者保険の対象外の患者に、後期高齢者保険が登録されています。", "保険者証" },
                                    new string[] { "無視する", "戻る" });
                            resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningLaterInsuranceRegisteredPatientsElderInsurance, TypeMessage.TypeMessageWarning));
                        }
                    }
                }
            }
            return resultMessages;
        }

        private bool NeedCheckMainHoken(List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs , int ptInfMainHokenPid)
        {
            var selectedHokenPattern = insurances.FirstOrDefault(x => x.HokenPatternSelected);
            var selectedInf = hokenInfs.FirstOrDefault(x => x.HokenId == selectedHokenPattern?.HokenId);

            if (selectedHokenPattern == null)
            {
                return false;
            }

            if (!selectedHokenPattern.IsAddNew)
            {
                return false;
            }
            if (selectedHokenPattern.IsEmptyModel)
            {
                return false;
            }
            if (selectedHokenPattern.IsExpirated)
            {
                return false;
            }
            if (ptInfMainHokenPid == selectedHokenPattern.HokenPid)
            {
                return false;
            }
            if (selectedInf?.IsJihi ?? false)
            {
                return false;
            }

            var mainHokenPattern = insurances.FirstOrDefault(p => p.HokenPid == ptInfMainHokenPid && p.IsDeleted == DeleteTypes.None);
            if (mainHokenPattern == null)
            {
                return true;
            }
            if (mainHokenPattern.IsExpirated)
            {
                return true;
            }

            if (mainHokenPattern.HokenKbn != selectedHokenPattern.HokenKbn)
            {
                int firstNumSelected = selectedHokenPattern.HokenSbtCd.AsString().PadRight(3, '0')[0].AsInteger();
                int firstNumMain = mainHokenPattern.HokenSbtCd.AsString().PadRight(3, '0')[0].AsInteger();
                // change from kohi to shaho or kokuho => doesn't need confirm
                if (selectedHokenPattern.HokenKbn == 1 && firstNumSelected == 5
                    && (mainHokenPattern.HokenKbn == 1 || mainHokenPattern.HokenKbn == 2) && firstNumMain > 0 && firstNumMain < 5)
                {
                    // Do nothing
                }
                else
                {
                    return true;
                }
            }
            if ((mainHokenPattern.HokenInf.IsShahoOrKokuho && selectedHokenPattern.HokenInf.IsShahoOrKokuho)
                || ((mainHokenPattern.IsEmptyHoken || mainHokenPattern.HokenInf.IsNoHoken) && (selectedHokenPattern.IsEmptyHoken || (selectedInf != null && selectedInf.IsNoHoken))))
            {
                return true;
            }

            return false;
        }

        private IEnumerable<SavePatientInfoValidationResult> IsValidMainHoken
            (List<InsuranceModel> insurances, 
            List<HokenInfModel> hokenInfs,
            ReactSavePatientInfo reactFromUI,
            int ptInfMainHokenPid)
        {
            var resultMessages = new List<SavePatientInfoValidationResult>();
            string message = string.Empty;
            if (ptInfMainHokenPid == 0 && NeedCheckMainHoken(insurances, hokenInfs , ptInfMainHokenPid)) //if patient info not set PatientInf.MainHokenPid
            {
                //In UI user set yes this message will set PatientInf.MainHokenPid = value;
                message = "'" + insurances.FirstOrDefault(x=>x.HokenPatternSelected)?.HokenName + "'" + "の保険組合せを主保険に設定しますか？";
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.ConfirmHokenPatternSelectedIsInfMainHokenPid, TypeMessage.TypeMessageConfirmation));
            }
            else
            {
                var mainHokenPattern = insurances.FirstOrDefault(p => p.HokenPid == ptInfMainHokenPid);
                if (mainHokenPattern != null && mainHokenPattern.IsExpirated && !reactFromUI.ConfirmHaveanExpiredHokenOnMain) //not ok
                {
                    message = "主保険に期限切れの保険が設定されています。主保険の設定を確認してください。";
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningHaveanExpiredHokenOnMain, TypeMessage.TypeMessageWarning));
                }
            }
            return resultMessages;
        }

        public IEnumerable<SavePatientInfoValidationResult> IsValidHokenPatternAll(
            List<InsuranceModel> insurances,
            List<HokenInfModel> hokenInfs,
            List<KohiInfModel> kohis,
            bool isUpdateMode, 
            int birthDay, 
            int sinDay,
            ReactSavePatientInfo reactFromUI,
            int ptInfMainHokenPid)
        {
            var resultMessages = new List<SavePatientInfoValidationResult>();
            string message = string.Empty;
            // In update mode, if doesn't exist valid hoken pattern then error
            if (isUpdateMode)
            {
                var validPatternList = insurances.Where(pattern => pattern.IsDeleted == 0).ToList();
                if (validPatternList.Count() > 0 && !validPatternList.Any(p => !p.IsEmptyModel))
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00011, new string[] { "保険組合せ", "情報" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.InvalidHokenPatternWhenUpdate, TypeMessage.TypeMessageError));
                }
            }

            if (insurances != null && insurances.Count > 0)
            {
                // 同じ組合せの保険が既に登録されている場合は警告。
                var PatternHokenOnly = insurances.Where(pattern => pattern.HokenKbn >= 1 && pattern.HokenKbn <= 4 && pattern.IsDeleted == 0);

                var duplicateQuery = PatternHokenOnly.GroupBy(x => new { x.HokenId, x.Kohi1Id, x.Kohi2Id, x.Kohi3Id, x.Kohi4Id })
                                            .Where(g => g.Count() > 1);
                if (duplicateQuery != null && duplicateQuery.Count() > 0 && !reactFromUI.ConfirmRegisteredInsuranceCombination)
                {
                    message = string.Format(ErrorMessage.MessageType_mEnt00020, new string[] { "同じ組合せの保険・公１・公２・公３・公４を持つ組合せ" });
                    resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningRegisteredInsuranceCombination, TypeMessage.TypeMessageWarning));
                }
            }

            resultMessages.AddRange(IsValidAgeCheck(insurances ?? new List<InsuranceModel>(), birthDay, sinDay, reactFromUI));

            resultMessages.AddRange(HasElderHoken(insurances ?? new List<InsuranceModel>(), hokenInfs, birthDay, sinDay, reactFromUI));

            // 主保険　組合せ確認

            resultMessages.AddRange(IsValidMainHoken(insurances ?? new List<InsuranceModel>(), hokenInfs, reactFromUI, ptInfMainHokenPid));

            if (!IsValidDuplicateHoken(hokenInfs) && !reactFromUI.ConfirmInsuranceSameInsuranceNumber)
            {
                message = string.Format(ErrorMessage.MessageType_mEnt00020, new string[] { "同じ保険番号を持つ保険" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningInsuranceSameInsuranceNumber, TypeMessage.TypeMessageWarning));
            }

            if (!IsValidPeriod(hokenInfs) && !reactFromUI.ConfirmMultipleHokenSignedUpSameTime)
            {
                message = ErrorMessage.MessageType_mChk00040;
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningMultipleHokenSignedUpSameTime, TypeMessage.TypeMessageWarning));
            }

            if (!IsValidDuplicateKohi(kohis) && !reactFromUI.ConfirmFundsWithSamePayerCode)
            {
                message = string.Format(ErrorMessage.MessageType_mEnt00020, new string[] { "同じ負担者番号を持つ公費" });
                resultMessages.Add(new SavePatientInfoValidationResult(message, SavePatientInforValidationCode.WarningFundsWithSamePayerCode, TypeMessage.TypeMessageWarning));
            }

            return resultMessages;
        }

        private bool IsValidDuplicateHoken(List<HokenInfModel> hokenInfs)
        {
            var allValidHoken = hokenInfs.Where(h => h.IsDeleted == 0 && !string.IsNullOrEmpty(h.HokensyaNo) && h.IsShahoOrKokuho);
            var duplicateQuery = allValidHoken.GroupBy(x => new { x.HokensyaNo, x.StartDate, x.EndDate })
                                        .Where(g => g.Count() > 1);
            if (duplicateQuery != null && duplicateQuery.Count() > 0)
            {
                return false;
            }
            return true;
        }


        private bool IsValidPeriod(List<HokenInfModel> hokenInfs)
        {
            var dupplicatePeriodHoken = hokenInfs.Count(h => h.IsDeleted == 0 && !string.IsNullOrEmpty(h.HokensyaNo) && h.IsShahoOrKokuho && !h.IsExpirated);
            if (dupplicatePeriodHoken > 1)
            {
                return false;
            }
            return true;
        }


        private bool IsValidDuplicateKohi(List<KohiInfModel> kohis)
        {
            var allValidKohi = kohis.Where(h => h.IsDeleted == 0 &&
                        (!string.IsNullOrEmpty(h.FutansyaNo) || !string.IsNullOrEmpty(h.JyukyusyaNo) || !string.IsNullOrEmpty(h.TokusyuNo)));
            var duplicateQuery = allValidKohi.GroupBy(x => new { x.FutansyaNo, x.JyukyusyaNo, x.TokusyuNo, x.StartDate, x.EndDate })
                                        .Where(g => g.Count() > 1);
            if (duplicateQuery != null && duplicateQuery.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}