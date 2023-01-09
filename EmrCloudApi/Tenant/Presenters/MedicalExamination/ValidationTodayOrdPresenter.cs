﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteInf;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using Helper.Constants;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace EmrCloudApi.Tenant.Presenters.MedicalExamination
{
    public class ValidationTodayOrdPresenter : IValidationTodayOrdOutputPort
    {
        public Response<ValidationTodayOrdResponse> Result { get; private set; } = default!;

        public void Complete(ValidationTodayOrdOutputData outputData)
        {
            var validations = new List<ValidationTodayOrdItemResponse>();
            Result = new Response<ValidationTodayOrdResponse>()
            {
                Message = outputData.Status == ValidationTodayOrdStatus.Successed ? ResponseMessage.Success : ResponseMessage.Failed,
                Status = (byte)outputData.Status

            };

            foreach (var validation in outputData.Validations)
            {
                var value = validation.Value;
                switch (value.Value)
                {
                    case OrderInfConst.OrdInfValidationStatus.InvalidSpecialItem:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MDrug), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSpecialStadardUsage:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MUsage), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidOdrKouiKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSpecialSuppUsage:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MSupUsage1), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHasUsageButNotDrug:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHasUsageButNotInjectionOrDrug:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHasDrugButNotUsage:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MUsage), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHasInjectionButNotUsage:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MInjection), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHasNotBothInjectionAndUsageOf28:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MInjection), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidStandardUsageOfDrugOrInjection:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MInjection), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSuppUsageOfDrugOrInjection:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MSupUsage2), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidBunkatu:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MBunkatu), ResponseMessage.TodayOdrBunkatu));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidUsageWhenBuntakuNull:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MUsage), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSumBunkatuDifferentSuryo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00041, ResponseMessage.MUsageQuantity, ResponseMessage.MSumBunkatu), string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidQuantityUnit:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrSuryo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00070, ResponseMessage.MUsageQuantity, ResponseMessage.MMaxQuantity), ResponseMessage.TodayOdrSuryoYohoKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrSuryoBunkatu));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidPrice:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MTooLargeQuantity), ResponseMessage.TodayOdrPriceSuryo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSuryoOfReffill:
                        var keyAndRefill = value.Key.Split("_");
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, keyAndRefill?.Length > 0 ? keyAndRefill[0].ToString() : "-1", string.Format(ResponseMessage.MInp00070, ResponseMessage.MUsageQuantity, keyAndRefill?.Length > 1 ? keyAndRefill[1].ToString() : 0), ResponseMessage.TodayOdrSuryo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt840:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt842:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt842CmtOptMoreThan38:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt830CmtOpt:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MCmtOptOf830), ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt830CmtOptMoreThan38:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt831:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MCmt831), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt850Date:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateInfor850_1), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt850OtherDate:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateInfor850_2), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt851:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MTimeInfor851), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt852:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MTimeInfor852), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt853:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateTimeInfor853), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmt880:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MDateTimeInfor880), ResponseMessage.TodayOdrCmt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.DuplicateTodayOrd:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidKohatuKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrKohatuKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidDrugKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrDrugKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidId:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrId));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHpId:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrHpId));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidRaiinNo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRaiinNo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidRpNo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpNo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidRpEdaNo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpEdaNo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidPtId:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrPtId));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSinDate:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSinDate));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidHokenPId:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrHokenPid));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidRpName:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpName));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidInoutKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrInOutKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSikyuKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSikyuKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSyohoSbt:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoSbt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSanteiKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSanteiKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidTosekiKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrTosekiKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidDaysCnt:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrDaysCnt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSortNo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSortNo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidRowNo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRowNo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSinKouiKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSinKouiKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidItemCd:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrItemCd));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidItemName:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrItemName));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSuryo:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSuryo));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidUnitName:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrUnitName));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidUnitSbt:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrUnitSbt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidTermVal:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrTermVal));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSyohoKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidSyohoLimitKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoLimitKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidYohoKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrYohoKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidIsNodspRece:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIsNodspRece));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidIpnCd:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIpnCd));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidIpnName:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIpnName));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidJissiKbn:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiKbn));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidJissiId:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiId));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidJissiMachine:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiMachine));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidReqCd:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrReqCd));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidBunkatuLength:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrBunkatu));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmtName:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCmtOpt));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCmtOpt:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmtName));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidFontColor:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrFontColor));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidCommentNewline:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCommentNewline));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidIsDeleted:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIsDeleted));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.InvalidTodayOrdUpdatedNoExist:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.HpIdNoExist:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.PtIdNoExist:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.RaiinNoNoExist:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.HokenPidNoExist:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case OrderInfConst.OrdInfValidationStatus.OdrNoMapOdrDetail:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    default:
                        validations.Add(new ValidationTodayOrdItemResponse(value.Value, "-1", "-1", string.Empty, string.Empty));
                        break;
                }
            }

            ValidationKarteInfResponse validationKarte = outputData.ValidationKarte switch
            {
                KarteConst.KarteValidationStatus.InvalidHpId => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfInvalidHpId),
                KarteConst.KarteValidationStatus.InvalidRaiinNo => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfInvalidRaiinNo),
                KarteConst.KarteValidationStatus.InvalidPtId => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfInvalidPtId),
                KarteConst.KarteValidationStatus.InvalidSinDate => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfInvalidSinDate),
                KarteConst.KarteValidationStatus.InvalidIsDelted => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfInvalidIsDeleted),
                KarteConst.KarteValidationStatus.RaiinNoNoExist => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfRaiinNoNoExist),
                KarteConst.KarteValidationStatus.PtIdNoExist => new ValidationKarteInfResponse(outputData.ValidationKarte, ResponseMessage.UpsertKarteInfPtIdNoExist),
                _ => new ValidationKarteInfResponse(KarteConst.KarteValidationStatus.Valid, ResponseMessage.Valid),
            };
            validations = validations.OrderBy(v => v.OrderInfPosition).ThenBy(v => v.OrderInfDetailPosition).ToList();

            Result.Data = new ValidationTodayOrdResponse(new RaiinInfItemResponse(outputData.ValidationRaiinInf, ConvertRaiinInfStatusToMessage(outputData.ValidationRaiinInf)), validations, validationKarte);
        }

        private static string ConvertRaiinInfStatusToMessage(RaiinInfConst.RaiinInfTodayOdrValidationStatus status)
        {
            return status switch
            {
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSyosaiKbn => ResponseMessage.RaiinInfTodayOdrInvalidSyosaiKbn,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidJikanKbn => ResponseMessage.RaiinInfTodayOdrInvalidJikanKbn,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHokenPid => ResponseMessage.RaiinInfTodayOdrInvalidHokenPid,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.HokenPidNoExist => ResponseMessage.RaiinInfTodayOdrHokenPidNoExist,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSanteiKbn => ResponseMessage.RaiinInfTodayOdrInvalidSanteiKbn,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidTantoId => ResponseMessage.RaiinInfTodayOdrInvalidTantoId,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.TatoIdNoExist => ResponseMessage.RaiinInfTodayOdrTatoIdNoExist,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidKaId => ResponseMessage.RaiinInfTodayOdrInvalidKaId,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.KaIdNoExist => ResponseMessage.RaiinInfTodayOdrKaIdNoExist,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidUKetukeTime => ResponseMessage.RaiinInfTodayOdrInvalidUKetukeTime,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinStartTime => ResponseMessage.RaiinInfTodayOdrInvalidSinStartTime,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinEndTime => ResponseMessage.RaiinInfTodayOdrInvalidSinEndTime,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidHpId => ResponseMessage.InvalidHpId,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidPtId => ResponseMessage.InvalidPtId,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.HpIdNoExist => ResponseMessage.RaiinInfTodayOdrHpIdNoExist,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.PtIdNoExist => ResponseMessage.RaiinInfTodayOdrPtIdNoExist,
                RaiinInfConst.RaiinInfTodayOdrValidationStatus.RaiinIdNoExist => ResponseMessage.RaiinInfTodayOdrRaiinNoExist,
                _ => ResponseMessage.Valid,
            };
        }
    }
}
