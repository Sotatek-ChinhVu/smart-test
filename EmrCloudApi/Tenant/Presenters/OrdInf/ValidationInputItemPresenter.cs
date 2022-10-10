using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInf;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using Helper.Constants;
using UseCase.OrdInfs.ValidationInputItem;

namespace EmrCloudApi.Tenant.Presenters.OrdInfs
{
    public class ValidationInputItemPresenter : IValidationInputItemOutputPort
    {
        public Response<ValidationOrdInfListResponse> Result { get; private set; } = default!;

        public void Complete(ValidationInputItemOutputData outputData)
        {
            var validations = new List<ValidationOrdInfListItemResponse>();

            Result = new Response<ValidationOrdInfListResponse>()
            {
                Message = outputData.Status == ValidationInputItemStatus.Successed ? ResponseMessage.Success : ResponseMessage.Failed,
                Status = (byte)outputData.Status
            };
            foreach (var validation in outputData.Validations)
            {
                var value = validation.Value;
                switch (value.Value)
                {
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialItem:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MDrug), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialStadardUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MUsage), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidOdrKouiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialSuppUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MSupUsage1), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasUsageButNotDrug:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasUsageButNotInjectionOrDrug:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasDrugButNotUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MUsage), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasInjectionButNotUsage:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MInjection), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHasNotBothInjectionAndUsageOf28:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MInjection), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidStandardUsageOfDrugOrInjection:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MInjection), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuppUsageOfDrugOrInjection:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MSupUsage2), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatu:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MBunkatu), ResponseMessage.TodayOdrBunkatu));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MUsage), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00041, ResponseMessage.MUsageQuantity, ResponseMessage.MSumBunkatu), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidQuantityUnit:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00070, ResponseMessage.MUsageQuantity, ResponseMessage.MMaxQuantity), ResponseMessage.TodayOdrSuryoYohoKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrSuryoBunkatu));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidPrice:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MTooLargeQuantity), ResponseMessage.TodayOdrPriceSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoOfReffill:
                        var keyAndRefill = value.Key.Split("_");
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, keyAndRefill?.Length > 0 ? keyAndRefill[0].ToString() : "-1", string.Format(ResponseMessage.MInp00070, ResponseMessage.MUsageQuantity, keyAndRefill?.Length > 1 ? keyAndRefill[1].ToString() : 0), ResponseMessage.TodayOdrSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt840:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842CmtOptMoreThan38:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOpt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MCmtOptOf830), ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOptMoreThan38:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmt842_830));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt831:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MCmt831), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850Date:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateInfor850_1), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850OtherDate:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateInfor850_2), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt851:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MTimeInfor851), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt852:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MTimeInfor852), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt853:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateTimeInfor853), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt880:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MDateTimeInfor880), ResponseMessage.TodayOdrCmt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.DuplicateTodayOrd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidKohatuKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrKohatuKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidDrugKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrDrugKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHpId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrHpId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRaiinNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRaiinNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRpNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRpEdaNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpEdaNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidPtId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrPtId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSinDate:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSinDate));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidHokenPId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrHokenPid));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRpName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidInoutKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrInOutKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSikyuKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSikyuKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoSbt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoSbt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSanteiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSanteiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTosekiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrTosekiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidDaysCnt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrDaysCnt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSortNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSortNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidRowNo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRowNo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSinKouiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSinKouiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidItemCd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrItemCd));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidItemName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrItemName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryo:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSuryo));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrUnitName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitSbt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrUnitSbt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTermVal:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrTermVal));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoLimitKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoLimitKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidYohoKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrYohoKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIsNodspRece:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIsNodspRece));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnCd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIpnCd));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIpnName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiKbn));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiId:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiId));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiMachine:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiMachine));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidReqCd:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrReqCd));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatuLength:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrBunkatu));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtName:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCmtOpt));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtOpt:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCmtName));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidFontColor:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrFontColor));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidCommentNewline:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCommentNewline));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidIsDeleted:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIsDeleted));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MCommonError, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidGazoDensibaitaiHozon:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MProcedure, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTokuzaiKouiKbn:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.MProcedure, string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTokuzai:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                        break;
                    case TodayOrderConst.TodayOrdValidationStatus.InvalidTokuzaiDrugOrInjection:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                        break;

                    default:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, "-1", "-1", string.Empty, string.Empty));
                        break;
                }
            }

            Result.Data = new ValidationOrdInfListResponse(validations);
        }
    }
}
