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
                    default:
                        validations.Add(new ValidationOrdInfListItemResponse(value.Value, -1, -1, string.Empty));
                        break;
                }
            }

            Result.Data = new ValidationOrdInfListResponse(validations);
        }
    }
}
