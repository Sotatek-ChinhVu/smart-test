using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInf;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using Helper.Constants;
using UseCase.OrdInfs.Validation;

namespace EmrCloudApi.Tenant.Presenters.OrdInfs
{
    public class ValidationOrdInfListPresenter : IValidationOrdInfListOutputPort
    {
        public Response<ValidationOrdInfListResponse> Result { get; private set; } = default!;

        public void Complete(ValidationOrdInfListOutputData outputData)
        {
            var validations = new List<ValidationOrdInfListItemResponse>();

            Result = new Response<ValidationOrdInfListResponse>()
            {
                Message = outputData.Status == ValidationOrdInfListStatus.Successed ? ResponseMessage.Success : ResponseMessage.Failed,
                Status = (byte)outputData.Status

            };
            foreach (var validation in outputData.Validations)
            {
                var value = validation.Value;
                switch (value.Value)
                {
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialItem:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSpecialItem, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialStadardUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdIInvalidSpecialStadardUsage, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidOdrKouiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidOdrKouiKbn, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialSuppUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSpecialSuppUsage, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasUsageButNotDrug:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasUsageButNotDrug, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasUsageButNotInjectionOrDrug:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasUsageButNotInjectionOrDrug, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasDrugButNotUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasDrugButNotUsage, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasInjectionButNotUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasInjectionButNotUsage, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasNotBothInjectionAndUsageOf28:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasNotBothInjectionAndUsageOf28, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidStandardUsageOfDrugOrInjection:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidStandardUsageOfDrugOrInjection, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuppUsageOfDrugOrInjection:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuppUsageOfDrugOrInjection, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatu:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidBunkatu, ResponseMessage.TodayOdrBunkatu));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUsageWhenBuntakuNull, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSumBunkatuDifferentSuryo, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidQuantityUnit:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidQuantityUnit, ResponseMessage.TodayOdrSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull, ResponseMessage.TodayOdrSuryoYohoKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu, ResponseMessage.TodayOdrSuryoBunkatu));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidPrice:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidPrice, ResponseMessage.TodayOdrPriceSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoOfReffill:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoOfReffill, ResponseMessage.TodayOdrSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt840:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt840, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt842, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842CmtOptMoreThan38:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt842CmtOptMoreThan38, ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOpt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt830CmtOpt, ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOptMoreThan38:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt830CmtOptMoreThan38, ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt831:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt831, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850Date:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt850Date, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850OtherDate:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt850OtherDate, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt851:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt851, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt852:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt852, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt853:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt853, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt880:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt880, ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.DuplicateTodayOrd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdDuplicateTodayOrd, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidKohatuKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidKohatuKbn, ResponseMessage.TodayOdrKohatuKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidDrugKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidDrugKbn, ResponseMessage.TodayOdrDrugKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidId, ResponseMessage.TodayOdrId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHpId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHpId, ResponseMessage.TodayOdrHpId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRaiinNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRaiinNo, ResponseMessage.TodayOdrRaiinNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRpNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpNo, ResponseMessage.TodayOdrRpNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRpEdaNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpEdaNo, ResponseMessage.TodayOdrRpEdaNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidPtId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidPtId, ResponseMessage.TodayOdrPtId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSinDate:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSinDate, ResponseMessage.TodayOdrSinDate));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHokenPId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHokenPId, ResponseMessage.TodayOdrHokenPid));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRpName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpName, ResponseMessage.TodayOdrRpName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidInoutKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidInoutKbn, ResponseMessage.TodayOdrInOutKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSikyuKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSikyuKbn, ResponseMessage.TodayOdrSikyuKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoSbt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoSbt, ResponseMessage.TodayOdrSyohoSbt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSanteiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSanteiKbn, ResponseMessage.TodayOdrSanteiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTosekiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidTosekiKbn, ResponseMessage.TodayOdrTosekiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidDaysCnt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidDaysCnt, ResponseMessage.TodayOdrDaysCnt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSortNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSortNo, ResponseMessage.TodayOdrSortNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRowNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRowNo, ResponseMessage.TodayOdrRowNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSinKouiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSinKouiKbn, ResponseMessage.TodayOdrSinKouiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidItemCd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidItemCd, ResponseMessage.TodayOdrItemCd));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidItemName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidItemName, ResponseMessage.TodayOdrItemName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryo, ResponseMessage.TodayOdrSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUnitName, ResponseMessage.TodayOdrUnitName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitSbt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUnitSbt, ResponseMessage.TodayOdrUnitSbt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTermVal:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidTermVal, ResponseMessage.TodayOdrTermVal));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoKbn, ResponseMessage.TodayOdrSyohoKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoLimitKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoLimitKbn, ResponseMessage.TodayOdrSyohoLimitKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidYohoKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidYohoKbn, ResponseMessage.TodayOdrYohoKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIsNodspRece:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIsNodspRece, ResponseMessage.TodayOdrIsNodspRece));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnCd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIpnCd, ResponseMessage.TodayOdrIpnCd));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIpnName, ResponseMessage.TodayOdrIpnName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiKbn, ResponseMessage.TodayOdrJissiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiId, ResponseMessage.TodayOdrJissiId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiMachine:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiMachine, ResponseMessage.TodayOdrJissiMachine));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidReqCd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidReqCd, ResponseMessage.TodayOdrReqCd));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatuLength:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidBunkatuLength, ResponseMessage.TodayOdrBunkatu));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmtName, ResponseMessage.TodayOdrCmtOpt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtOpt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmtOpt, ResponseMessage.TodayOdrCmtName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidFontColor:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidFontColor, ResponseMessage.TodayOdrFontColor));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCommentNewline:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCommentNewline, ResponseMessage.TodayOdrCommentNewline));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIsDeleted:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIsDeleted, ResponseMessage.TodayOdrIsDeleted));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTodayOrdInsertedExist:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidInsertedExist, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUpdatedNoExist, string.Empty));
                        break;
                    default:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, -1, -1, string.Empty, string.Empty));
                        break;
                }
            }

            Result.Data = new ValidationOrdInfListResponse(validations);
        }
    }
}
