using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KarteInf;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Responses.NextOrder;
using Helper.Constants;
using UseCase.NextOrder.Upsert;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.RsvkrtByomeiConst;

namespace EmrCloudApi.Presenters.NextOrder
{
    public class UpsertNextOrderListPresenter : IUpsertNextOrderListOutputPort
    {
        public Response<UpsertNextOrderListResponse> Result { get; private set; } = default!;

        public void Complete(UpsertNextOrderListOutputData outputData)
        {
            var validationOrders = new List<OrderInfItemResponse>();
            var validationKartes = new List<KarteInfItemResponse>();
            var validationNextOrders = new List<NextOrderItemResponse>();
            var validationByomeis = new List<ByomeiItemResponse>();

            Result = new Response<UpsertNextOrderListResponse>()
            {
                Message = ConvertUpsertNextOrderStatusToMessage(outputData.Status),
                Status = (byte)outputData.Status
            };

            if (outputData.ValidationOdrs.Any())
            {
                foreach (var validation in outputData.ValidationOdrs)
                {
                    var dictionaryOneNextOrder = new List<ValidationTodayOrdItemResponse>();
                    var validationOneNextOrder = validation.Item2;
                    foreach (var validationOrder in validationOneNextOrder)
                    {
                        switch (validationOrder.Value.Value)
                        {
                            case OrderInfConst.OrdInfValidationStatus.InvalidSpecialItem:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MDrug), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSpecialStadardUsage:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MUsage), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidOdrKouiKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSpecialSuppUsage:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MEnt01040, ResponseMessage.ErrorCaptionDrugOrInject, ResponseMessage.MSupUsage1), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHasUsageButNotDrug:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHasUsageButNotInjectionOrDrug:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDrug), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHasDrugButNotUsage:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MUsage), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHasInjectionButNotUsage:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MInjection), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHasNotBothInjectionAndUsageOf28:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MInjection), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidStandardUsageOfDrugOrInjection:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MInjection), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSuppUsageOfDrugOrInjection:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MSupUsage2), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidBunkatu:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MEnt01020, ResponseMessage.MBunkatu), ResponseMessage.TodayOdrBunkatu));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidUsageWhenBuntakuNull:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MUsage), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSumBunkatuDifferentSuryo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00041, ResponseMessage.MUsageQuantity, ResponseMessage.MSumBunkatu), string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidQuantityUnit:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrSuryo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00070, ResponseMessage.MUsageQuantity, ResponseMessage.MMaxQuantity), ResponseMessage.TodayOdrSuryoYohoKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrSuryoBunkatu));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidPrice:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MTooLargeQuantity), ResponseMessage.TodayOdrPriceSuryo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSuryoOfReffill:
                                var keyAndRefill = validationOrder.Value.Key.Split("_");
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, keyAndRefill?.Length > 0 ? keyAndRefill[0].ToString() : "-1", string.Format(ResponseMessage.MInp00070, ResponseMessage.MUsageQuantity, keyAndRefill?.Length > 1 ? keyAndRefill[1].ToString() : 0), ResponseMessage.TodayOdrSuryo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt840:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt842:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MQuantity), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt842CmtOptMoreThan38:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmt842_830));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt830CmtOpt:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MCmtOptOf830), ResponseMessage.TodayOdrCmt842_830));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt830CmtOptMoreThan38:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmt842_830));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt831:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MCmt831), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt850Date:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateInfor850_1), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt850OtherDate:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateInfor850_2), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt851:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MTimeInfor851), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt852:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MTimeInfor852), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt853:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MInp00010, ResponseMessage.MDateTimeInfor853), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmt880:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MDateTimeInfor880), ResponseMessage.TodayOdrCmt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.DuplicateTodayOrd:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidKohatuKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrKohatuKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidDrugKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrDrugKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidId:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrId));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHpId:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrHpId));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidRaiinNo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.InvalidRsvkrtNo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidRpNo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpNo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidRpEdaNo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpEdaNo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidPtId:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrPtId));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSinDate:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.InvalidRsvDate));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidHokenPId:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrHokenPid));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidRpName:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRpName));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidInoutKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrInOutKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSikyuKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSikyuKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSyohoSbt:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoSbt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSanteiKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSanteiKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidTosekiKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrTosekiKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidDaysCnt:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrDaysCnt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSortNo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSortNo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidRowNo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrRowNo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSinKouiKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSinKouiKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidItemCd:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrItemCd));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidItemName:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrItemName));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSuryo:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSuryo));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidUnitName:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrUnitName));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidUnitSbt:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrUnitSbt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidTermVal:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrTermVal));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSyohoKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidSyohoLimitKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrSyohoLimitKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidYohoKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrYohoKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidIsNodspRece:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIsNodspRece));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidIpnCd:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIpnCd));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidIpnName:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIpnName));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidJissiKbn:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiKbn));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidJissiId:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiId));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidJissiMachine:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrJissiMachine));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidReqCd:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrReqCd));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidBunkatuLength:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrBunkatu));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmtName:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCmtOpt));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCmtOpt:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, string.Format(ResponseMessage.MFree00030, ResponseMessage.MMaxLengthOfCmt), ResponseMessage.TodayOdrCmtName));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidFontColor:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrFontColor));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidCommentNewline:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrCommentNewline));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidIsDeleted:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, ResponseMessage.TodayOdrIsDeleted));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidTodayOrdUpdatedNoExist:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.HpIdNoExist:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.PtIdNoExist:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.RaiinNoNoExist:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.HokenPidNoExist:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.OdrNoMapOdrDetail:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidKokuji1:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            case OrderInfConst.OrdInfValidationStatus.InvalidKokuji2:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, validationOrder.Key, validationOrder.Value.Key, ResponseMessage.MCommonError, string.Empty));
                                break;
                            default:
                                dictionaryOneNextOrder.Add(new ValidationTodayOrdItemResponse(validationOrder.Value.Value, "-1", "-1", string.Empty, string.Empty));
                                break;
                        }
                    }
                    validationOrders.Add(new OrderInfItemResponse(validation.Item1, dictionaryOneNextOrder));
                }
            }

            ValidationKarteInfResponse validationKarte;
            foreach (var validation in outputData.ValidationKarte)
            {
                switch (validation.Item2)
                {
                    case KarteConst.KarteValidationStatus.InvalidHpId:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.UpsertKarteInfInvalidHpId);
                        break;
                    case KarteConst.KarteValidationStatus.InvalidRaiinNo:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.InvalidRsvkrtNo);
                        break;
                    case KarteConst.KarteValidationStatus.InvalidPtId:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.UpsertKarteInfInvalidPtId);
                        break;
                    case KarteConst.KarteValidationStatus.InvalidSinDate:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.InvalidRsvDate);
                        break;
                    case KarteConst.KarteValidationStatus.InvalidIsDelted:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.UpsertKarteInfInvalidIsDeleted);
                        break;
                    case KarteConst.KarteValidationStatus.RaiinNoNoExist:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.UpsertKarteInfRaiinNoNoExist);
                        break;
                    case KarteConst.KarteValidationStatus.PtIdNoExist:
                        validationKarte = new ValidationKarteInfResponse(validation.Item2, ResponseMessage.UpsertKarteInfPtIdNoExist);
                        break;
                    default:
                        validationKarte = new ValidationKarteInfResponse(KarteConst.KarteValidationStatus.Valid, ResponseMessage.Valid);
                        break;
                }
                validationKartes.Add(new KarteInfItemResponse(validation.Item1, validationKarte));
            }

            foreach (var validation in outputData.ValidationNextOrder)
            {
                var validationNextOrder = new NextOrderValidationItemResponse(validation.Value, ConvertNextOrderStatusToMessage(validation.Value));
                validationNextOrders.Add(new NextOrderItemResponse(validation.Key, validationNextOrder));
            }

            var groupByomeis = outputData.ValidationByomeis.GroupBy(g => g.Item1).ToList();
            foreach (var validation in groupByomeis)
            {
                var validationByomeisOfOne = new ByomeiItemResponse(validation.Key, validation.Select(v => new ByomeiValidationItemResponse(v.Item3, v.Item2, ConvertUpsertByomeiStatusToMessage(v.Item3))).ToList());
                validationByomeis.Add(validationByomeisOfOne);
            }

            Result.Data = new UpsertNextOrderListResponse(validationNextOrders, validationOrders, validationKartes, validationByomeis);
        }

        private static string ConvertNextOrderStatusToMessage(NextOrderStatus status)
        {
            return status switch
            {
                NextOrderStatus.InvalidRsvkrtNo => ResponseMessage.InvalidRsvkrtNo,
                NextOrderStatus.InvalidRsvkrtKbn => ResponseMessage.InvalidRsvkrtKbn,
                NextOrderStatus.InvalidRsvDate => ResponseMessage.InvalidRsvDate,
                NextOrderStatus.InvalidRsvName => ResponseMessage.InvalidRsvkrtName,
                NextOrderStatus.InvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
                _ => ResponseMessage.Valid,
            };
        }

        private static string ConvertUpsertNextOrderStatusToMessage(UpsertNextOrderListStatus status)
        {
            return status switch
            {
                UpsertNextOrderListStatus.InvalidUserId => ResponseMessage.InvalidUserId,
                UpsertNextOrderListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
                UpsertNextOrderListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
                UpsertNextOrderListStatus.Successed => ResponseMessage.Success,
                UpsertNextOrderListStatus.Failed => ResponseMessage.Failed,
                _ => string.Empty,
            };
        }

        private string ConvertUpsertByomeiStatusToMessage(RsvkrtByomeiStatus status) => status switch
        {
            RsvkrtByomeiStatus.Success => ResponseMessage.PtDiseaseUpsertSuccess,
            RsvkrtByomeiStatus.UpdateNoSuccess => ResponseMessage.PtDiseaseUpsertFail,
            RsvkrtByomeiStatus.InvalidInputNoData => ResponseMessage.PtDiseaseUpsertInputNoData,
            RsvkrtByomeiStatus.InvalidSikkanKbn => ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidNanByoCd => ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidFreeWord => ResponseMessage.MEnt00040_1,
            RsvkrtByomeiStatus.InvalidByomei => string.Format(ResponseMessage.MInp00160_1, ResponseMessage.MDisease),
            RsvkrtByomeiStatus.InvalidId =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidByomeiCd =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidSyubyoKbn =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidHosokuCmt =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidIsNodspRece =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidIsNodspKarte =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidSeqNo =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidIsDeleted =>
              ResponseMessage.MCommonError,
            RsvkrtByomeiStatus.InvalidRsvkrtNo =>
              ResponseMessage.MCommonError,
            _ => string.Empty
        };
    }
}
