using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteInf;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using EmrCloudApi.Tenant.Responses.OrdInf;
using Helper.Constants;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Tenant.Presenters.MedicalExamination
{
    public class UpsertTodayOdrPresenter : IUpsertTodayOrdOutputPort
    {
        public Response<UpsertTodayOdrResponse> Result { get; private set; } = default!;

        public void Complete(UpsertTodayOrdOutputData outputData)
        {
            var validations = new List<ValidationOrdInfListItemResponse>();
            var validationKartes = new List<ValidationKarteInfListItemResponse>();

            Result = new Response<UpsertTodayOdrResponse>()
            {
                Message = outputData.Status == UpsertTodayOrdStatus.Successed ? ResponseMessage.Success : ResponseMessage.Failed,
                Status = (byte)outputData.Status
            };

            if (outputData.ValidationOdrs.Any())
            {
                foreach (var validation in outputData.ValidationOdrs)
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
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidBunkatu, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUsageWhenBuntakuNull, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSumBunkatuDifferentSuryo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidQuantityUnit:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidQuantityUnit, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidPrice:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidPrice, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoOfReffill:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoOfReffill, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt840:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt840, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt842, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842CmtOptMoreThan38:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt842CmtOptMoreThan38, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOpt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt830CmtOpt, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOptMoreThan38:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt830CmtOptMoreThan38, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt831:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt831, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850Date:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt850Date, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850OtherDate:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt850OtherDate, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt851:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt851, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt852:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt852, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt853:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt853, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt880:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt880, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.DuplicateTodayOrd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdDuplicateTodayOrd, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidKohatuKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidKohatuKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidDrugKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidDrugKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidId, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHpId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHpId, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRaiinNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRaiinNo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRpNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpNo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRpEdaNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpEdaNo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidPtId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidPtId, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSinDate:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSinDate, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHokenPId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHokenPId, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRpName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpName, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidInoutKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidInoutKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSikyuKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSikyuKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoSbt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoSbt, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSanteiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSanteiKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTosekiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidTosekiKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidDaysCnt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidDaysCnt, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSortNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSortNo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRowNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRowNo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSinKouiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSinKouiKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidItemCd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidItemCd, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidItemName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidItemName, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryo, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUnitName, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitSbt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUnitSbt, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTermVal:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidTermVal, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoLimitKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoLimitKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidYohoKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidYohoKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIsNodspRece:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIsNodspRece, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnCd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIpnCd, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIpnName, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiKbn, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiId, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiMachine:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiMachine, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidReqCd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidReqCd, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatuLength:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidBunkatuLength, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmtName, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtOpt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmtOpt, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidFontColor:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidFontColor, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCommentNewline:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCommentNewline, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIsDeleted:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIsDeleted, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUpdatedNoExist, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.HokenPidNoExist:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHokenPidNoExist, string.Empty));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.OdrNoMapOdrDetail:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdOrdInfNoMapOrdInfDetail, string.Empty));
                            break;
                        default:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, -1, -1, string.Empty, string.Empty));
                            break;
                    }
                }
            }

            if (outputData.ValidationKartes.Any())
            {
                foreach (var validation in outputData.ValidationKartes)
                {
                    switch (validation.Value)
                    {
                        case TodayKarteConst.TodayKarteValidationStatus.InvalidHpId:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfInvalidHpId));
                            break;
                        case TodayKarteConst.TodayKarteValidationStatus.InvalidRaiinNo:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfInvalidRaiinNo));
                            break;
                        case TodayKarteConst.TodayKarteValidationStatus.InvalidPtId:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfInvalidPtId));
                            break;
                        case TodayKarteConst.TodayKarteValidationStatus.InvalidSinDate:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfInvalidSinDate));
                            break;
                        case TodayKarteConst.TodayKarteValidationStatus.InvalidIsDelted:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfInvalidIsDeleted));
                            break;
                        case TodayKarteConst.TodayKarteValidationStatus.RaiinNoNoExist:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfRaiinNoNoExist));
                            break;
                        case TodayKarteConst.TodayKarteValidationStatus.PtIdNoExist:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfPtIdNoExist));
                            break;
                    }
                }
            }

            Result.Data = new UpsertTodayOdrResponse(outputData.Status, new RaiinInfItemResponse(outputData.ValidationRaiinInf, ConvertRaiinInfStatusToMessage(outputData.ValidationRaiinInf)), validations, validationKartes);
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
