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
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSpecialItem));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialStadardUsage:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdIInvalidSpecialStadardUsage));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidOdrKouiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidOdrKouiKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSpecialSuppUsage:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSpecialSuppUsage));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHasUsageButNotDrug:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasUsageButNotDrug));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHasUsageButNotInjectionOrDrug:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasUsageButNotInjectionOrDrug));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHasDrugButNotUsage:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasDrugButNotUsage));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHasInjectionButNotUsage:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasInjectionButNotUsage));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHasNotBothInjectionAndUsageOf28:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHasNotBothInjectionAndUsageOf28));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidStandardUsageOfDrugOrInjection:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidStandardUsageOfDrugOrInjection));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuppUsageOfDrugOrInjection:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuppUsageOfDrugOrInjection));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatu:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidBunkatu));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUsageWhenBuntakuNull));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSumBunkatuDifferentSuryo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidQuantityUnit:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidQuantityUnit));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidPrice:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidPrice));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryoOfReffill:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryoOfReffill));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt840:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt840));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt842));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt842CmtOptMoreThan38:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt842CmtOptMoreThan38));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOpt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt830CmtOpt));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt830CmtOptMoreThan38:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt830CmtOptMoreThan38));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt831:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt831));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850Date:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt850Date));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt850OtherDate:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt850OtherDate));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt851:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt851));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt852:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt852));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt853:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt853));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmt880:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmt880));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.DuplicateTodayOrd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdDuplicateTodayOrd));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidKohatuKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidKohatuKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidDrugKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidDrugKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidId));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHpId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHpId));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRaiinNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRaiinNo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRpNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpNo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRpEdaNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpEdaNo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidPtId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidPtId));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSinDate:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSinDate));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidHokenPId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidHokenPId));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRpName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRpName));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidInoutKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidInoutKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSikyuKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSikyuKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoSbt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoSbt));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSanteiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSanteiKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTosekiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidTosekiKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidDaysCnt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidDaysCnt));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSortNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSortNo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidRowNo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidRowNo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSinKouiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSinKouiKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidItemCd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidItemCd));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidItemName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidItemName));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSuryo:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSuryo));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUnitName));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidUnitSbt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUnitSbt));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTermVal:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidTermVal));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidSyohoLimitKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidSyohoLimitKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidYohoKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidYohoKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIsNodspRece:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIsNodspRece));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnCd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIpnCd));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIpnName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIpnName));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiKbn:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiKbn));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiId:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiId));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidJissiMachine:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidJissiMachine));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidReqCd:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidReqCd));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidBunkatuLength:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidBunkatuLength));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtName:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmtName));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCmtOpt:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCmtOpt));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidFontColor:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidFontColor));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidCommentNewline:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidCommentNewline));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidIsDeleted:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidIsDeleted));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTodayOrdInsertedExist:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidInsertedExist));
                            break;
                        case TodayOrderConst.TodayOrdValidationStatus.InvalidTodayOrdUpdatedNoExist:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, validation.Key, value.Key, ResponseMessage.TodayOrdInvalidUpdatedNoExist));
                            break;
                        default:
                            validations.Add(new ValidationOrdInfListItemResponse(value.Value, -1, -1, string.Empty));
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
                        case TodayKarteConst.TodayKarteValidationStatus.InvalidKarteKbn:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfInvalidKarteKbn));
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
                        case TodayKarteConst.TodayKarteValidationStatus.KarteKbnNoExist:
                            validationKartes.Add(new ValidationKarteInfListItemResponse(validation.Key, validation.Value, ResponseMessage.UpsertKarteInfKarteKbnNoExist));
                            break;
                    }
                }
            }

            Result.Data = new UpsertTodayOdrResponse(outputData.Status, new RaiinInfItemResponse(outputData.ValidationRaiinInf, ConvertRaiinInfStatusToMessage(outputData.ValidationRaiinInf)), validations, validationKartes);
        }

        private string ConvertRaiinInfStatusToMessage(RaiinInfConst.RaiinInfValidationStatus status)
        {
            switch (status)
            {
                case RaiinInfConst.RaiinInfValidationStatus.InvalidStatus:
                    return ResponseMessage.RaiinInfInvalidStatus;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidSyosaiKbn:
                    return ResponseMessage.RaiinInfInvalidSyosaiKbn;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidJikanKbn:
                    return ResponseMessage.RaiinInfInvalidJikanKbn;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidHokenPid:
                    return ResponseMessage.RaiinInfInvalidHokenPid;
                case RaiinInfConst.RaiinInfValidationStatus.HokenPidNoExist:
                    return ResponseMessage.RaiinInfHokenPidNoExist;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidSanteiKbn:
                    return ResponseMessage.RaiinInfInvalidSanteiKbn;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidTantoId:
                    return ResponseMessage.RaiinInfInvalidTantoId;
                case RaiinInfConst.RaiinInfValidationStatus.TatoIdNoExist:
                    return ResponseMessage.RaiinInfTatoIdNoExist;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidKaId:
                    return ResponseMessage.RaiinInfInvalidKaId;
                case RaiinInfConst.RaiinInfValidationStatus.KaIdNoExist:
                    return ResponseMessage.RaiinInfKaIdNoExist;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidUKetukeTime:
                    return ResponseMessage.RaiinInfInvalidUKetukeTime;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidSinStartTime:
                    return ResponseMessage.RaiinInfInvalidSinStartTime;
                case RaiinInfConst.RaiinInfValidationStatus.InvalidSinEndTime:
                    return ResponseMessage.RaiinInfInvalidSinEndTime;
                default:
                    return ResponseMessage.Valid;

            }
        }
    }
}
