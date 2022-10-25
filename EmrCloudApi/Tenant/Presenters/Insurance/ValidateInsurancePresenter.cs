using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidateInsurance;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidateInsurancePresenter : IValidateInsuranceOutputPort
    {
        public Response<ValidateListInsuranceResponse> Result { get; private set; } = default!;

        public void Complete(ValidateInsuranceOutputData outputData)
        {
            var resultResponse = new List<ValidateInsuranceResponse>();
            if (outputData.ListResult.Count > 0)
            {
                foreach (var item in outputData.ListResult)
                {
                    if(item.ListValidate.Count > 0)
                    {
                        foreach (var itemValidate in item.ListValidate)
                        {
                            var itemReponse = new ValidateInsuranceResponse(itemValidate.Result, GetMessage(itemValidate.Status), item);
                            resultResponse.Add(itemReponse);
                        }
                    }
                }
            }

            Result = new Response<ValidateListInsuranceResponse>
            {
                Data = new ValidateListInsuranceResponse(resultResponse),
                Status = (byte)outputData.Status
            };
            
        }

        private string GetMessage(ValidateInsuranceStatus status)
        {
            var message = "";
            switch (status)
            {
                case ValidateInsuranceStatus.Successed:
                    message = ResponseMessage.Success;
                    break;
                case ValidateInsuranceStatus.InvalidFaild:
                    message = ResponseMessage.Failed;
                    break;
                case ValidateInsuranceStatus.InvalidHpId:
                    message = ResponseMessage.InvalidHpId;
                    break;
                case ValidateInsuranceStatus.InvalidSindate:
                    message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidateInsuranceStatus.InvalidPtBirthday:
                    message = ResponseMessage.InvalidPtBirthday;
                    break;
                case ValidateInsuranceStatus.InvalidJihiSelectedHokenInfHokenNoEquals0:
                    message = ResponseMessage.InvalidPatternJihiSelectedHokenInfHokenNoEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidEmptyHoken:
                    message = ResponseMessage.InvalidPatternEmptyHoken;
                    break;
                case ValidateInsuranceStatus.InvalidHokenNashiOnly:
                    message = ResponseMessage.InvalidPatternHokenNashiOnly;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue1:
                    message = ResponseMessage.InvalidPatternTokkiValue1;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue21:
                    message = ResponseMessage.InvalidPatternTokkiValue21;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue31:
                    message = ResponseMessage.InvalidPatternTokkiValue31;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue41:
                    message = ResponseMessage.InvalidPatternTokkiValue41;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue51:
                    message = ResponseMessage.InvalidPatternTokkiValue51;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue2:
                    message = ResponseMessage.InvalidPatternTokkiValue2;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue23:
                    message = ResponseMessage.InvalidPatternTokkiValue23;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue24:
                    message = ResponseMessage.InvalidPatternTokkiValue24;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue25:
                    message = ResponseMessage.InvalidPatternTokkiValue25;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue3:
                    message = ResponseMessage.InvalidPatternTokkiValue3;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue34:
                    message = ResponseMessage.InvalidPatternTokkiValue34;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue35:
                    message = ResponseMessage.InvalidPatternTokkiValue35;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue4:
                    message = ResponseMessage.InvalidPatternTokkiValue4;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue45:
                    message = ResponseMessage.InvalidPatternTokkiValue45;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue5:
                    message = ResponseMessage.InvalidPatternTokkiValue5;
                    break;
                case ValidateInsuranceStatus.InvalidYukoKigen:
                    message = ResponseMessage.InvalidPatternYukoKigen;
                    break;
                case ValidateInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0:
                    message = ResponseMessage.InvalidPatternHokenSyaNoNullAndHokenNoNotEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidHokenNoEquals0:
                    message = ResponseMessage.InvalidPatternHokenNoEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidHokenNoHaveHokenMst:
                    message = ResponseMessage.InvalidPatternHokenNoHaveHokenMst;
                    break;
                case ValidateInsuranceStatus.InvalidHoubetu:
                    message = ResponseMessage.InvalidPatternHoubetu;
                    break;
                case ValidateInsuranceStatus.InvalidCheckDigitEquals1:
                    message = ResponseMessage.InvalidPatternCheckDigitEquals1;
                    break;
                case ValidateInsuranceStatus.InvalidCheckAgeHokenMst:
                    message = ResponseMessage.InvalidPatternCheckAgeHokenMst;
                    break;
                case ValidateInsuranceStatus.InvalidHokensyaNoNull:
                    message = ResponseMessage.InvalidPatternHokensyaNoNull;
                    break;
                case ValidateInsuranceStatus.InvalidHokensyaNoEquals0:
                    message = ResponseMessage.InvalidPatternHokensyaNoEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidHokensyaNoLength8StartWith39:
                    message = ResponseMessage.InvalidPatternHokensyaNoLength8StartWith39;
                    break;
                case ValidateInsuranceStatus.InvalidKigoNull:
                    message = ResponseMessage.InvalidPatternKigoNull;
                    break;
                case ValidateInsuranceStatus.InvalidBangoNull:
                    message = ResponseMessage.InvalidPatternBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidHokenKbnEquals0:
                    message = ResponseMessage.InvalidPatternHokenKbnEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidTokkurei:
                    message = ResponseMessage.InvalidPatternTokkurei;
                    break;
                case ValidateInsuranceStatus.InvalidConfirmDateAgeCheck:
                    message = ResponseMessage.InvalidPatternConfirmDateAgeCheck;
                    break;
                case ValidateInsuranceStatus.InvalidHokenMstStartDate:
                    message = ResponseMessage.InvalidPatternHokenMstStartDate;
                    break;
                case ValidateInsuranceStatus.InvalidHokenMstEndDate:
                    message = ResponseMessage.InvalidPatternHokenMstEndDate;
                    break;
                case ValidateInsuranceStatus.InvalidRodoBangoNull:
                    message = ResponseMessage.InvalidPatternHokenRodoBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidRodoBangoLengthNotEquals14:
                    message = ResponseMessage.InvalidPatternRodoBangoLengthNotEquals14;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiTenkiDefaultRow:
                    message = ResponseMessage.InvalidPatternRousaiTenkiDefaultRow;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiTenkiData:
                    message = ResponseMessage.InvalidPatternRousaiTenkiData;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiSaigaiKbn:
                    message = ResponseMessage.InvalidPatternRousaiSaigaiKbn;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiSyobyoDateEquals0:
                    message = ResponseMessage.InvalidPatternRousaiSyobyoDateEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiSyobyoCdNull:
                    message = ResponseMessage.InvalidPatternRousaiSyobyoCdNull;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiRyoyoDate:
                    message = ResponseMessage.InvalidPatternRousaiRyoyoDate;
                    break;
                case ValidateInsuranceStatus.InvalidRosaiYukoDate:
                    message = ResponseMessage.InvalidPatternRosaiYukoDate;
                    break;
                case ValidateInsuranceStatus.InvalidCheckHokenInfDate:
                    message = ResponseMessage.InvalidPatternCheckHokenInfDate;
                    break;
                case ValidateInsuranceStatus.InvalidNenkinBangoNull:
                    message = ResponseMessage.InvalidPatternNenkinBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidNenkinBangoLengthNotEquals9:
                    message = ResponseMessage.InvalidPatternNenkinBangoLengthNotEquals9;
                    break;
                case ValidateInsuranceStatus.InvalidKenkoKanriBangoNull:
                    message = ResponseMessage.InvalidPatternKenkoKanriBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidKenkoKanriBangoLengthNotEquals13:
                    message = ResponseMessage.InvalidPatternKenkoKanriBangoLengthNotEquals13;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel1:
                    message = ResponseMessage.InvalidPatternKohiEmptyModel1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty1:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEmpty1;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty1:
                    message = ResponseMessage.InvalidPatternFutansyaNoEmpty1;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo1:
                    message = ResponseMessage.InvalidPatternJyukyusyaNo1;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo1:
                    message = ResponseMessage.InvalidPatternTokusyuNo1;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo01:
                    message = ResponseMessage.InvalidPatternFutansyaNo01;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate1:
                    message = ResponseMessage.InvalidPatternKohiYukoDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate1:
                    message = ResponseMessage.InvalidPatternKohiHokenMstStartDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate1:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEndDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate1:
                    message = ResponseMessage.InvalidPatternKohiConfirmDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT1:
                    message = ResponseMessage.InvalidPatternKohiMstCheckHBT1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo1:
                    message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo1:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge1:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge1;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull1:
                    message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel2:
                    message = ResponseMessage.InvalidPatternKohiEmptyModel2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty2:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEmpty2;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty2:
                    message = ResponseMessage.InvalidPatternFutansyaNoEmpty2;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo2:
                    message = ResponseMessage.InvalidPatternJyukyusyaNo2;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo2:
                    message = ResponseMessage.InvalidPatternTokusyuNo2;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo02:
                    message = ResponseMessage.InvalidPatternFutansyaNo02;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate2:
                    message = ResponseMessage.InvalidPatternKohiYukoDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate2:
                    message = ResponseMessage.InvalidPatternKohiHokenMstStartDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate2:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEndDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate2:
                    message = ResponseMessage.InvalidPatternKohiConfirmDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT2:
                    message = ResponseMessage.InvalidPatternKohiMstCheckHBT2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo2:
                    message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo2:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge2:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge2;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull2:
                    message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel3:
                    message = ResponseMessage.InvalidPatternKohiEmptyModel3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty3:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEmpty3;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty3:
                    message = ResponseMessage.InvalidPatternFutansyaNoEmpty3;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo3:
                    message = ResponseMessage.InvalidPatternJyukyusyaNo3;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo3:
                    message = ResponseMessage.InvalidPatternTokusyuNo3;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo03:
                    message = ResponseMessage.InvalidPatternFutansyaNo03;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate3:
                    message = ResponseMessage.InvalidPatternKohiYukoDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate3:
                    message = ResponseMessage.InvalidPatternKohiHokenMstStartDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate3:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEndDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate3:
                    message = ResponseMessage.InvalidPatternKohiConfirmDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT3:
                    message = ResponseMessage.InvalidPatternKohiMstCheckHBT3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo3:
                    message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo3:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge3:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge3;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull3:
                    message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel4:
                    message = ResponseMessage.InvalidPatternKohiEmptyModel4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty4:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEmpty4;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty4:
                    message = ResponseMessage.InvalidPatternFutansyaNoEmpty4;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo4:
                    message = ResponseMessage.InvalidPatternJyukyusyaNo4;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo4:
                    message = ResponseMessage.InvalidPatternTokusyuNo4;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo04:
                    message = ResponseMessage.InvalidPatternFutansyaNo04;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate4:
                    message = ResponseMessage.InvalidPatternKohiYukoDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate4:
                    message = ResponseMessage.InvalidPatternKohiHokenMstStartDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate4:
                    message = ResponseMessage.InvalidPatternKohiHokenMstEndDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate4:
                    message = ResponseMessage.InvalidPatternKohiConfirmDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT4:
                    message = ResponseMessage.InvalidPatternKohiMstCheckHBT4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo4:
                    message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo4:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge4:
                    message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge4;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull4:
                    message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty21:
                    message = ResponseMessage.InvalidPatternKohiEmpty21;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty31:
                    message = ResponseMessage.InvalidPatternKohiEmpty31;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty32:
                    message = ResponseMessage.InvalidPatternKohiEmpty32;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty41:
                    message = ResponseMessage.InvalidPatternKohiEmpty41;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty42:
                    message = ResponseMessage.InvalidPatternKohiEmpty42;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty43:
                    message = ResponseMessage.InvalidPatternKohiEmpty43;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi1:
                    message = ResponseMessage.InvalidPatternDuplicateKohi1;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi2:
                    message = ResponseMessage.InvalidPatternDuplicateKohi2;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi3:
                    message = ResponseMessage.InvalidPatternDuplicateKohi3;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi4:
                    message = ResponseMessage.InvalidPatternDuplicateKohi4;
                    break;
                case ValidateInsuranceStatus.InvalidWarningDuplicatePattern:
                    message = ResponseMessage.InvalidPatternWarningDuplicatePattern;
                    break;
                case ValidateInsuranceStatus.InvalidWarningAge75:
                    message = ResponseMessage.InvalidPatternWarningAge75;
                    break;
                case ValidateInsuranceStatus.InvalidWarningAge65:
                    message = ResponseMessage.InvalidPatternWarningAge65;
                    break;
                case ValidateInsuranceStatus.InvalidMaruchoOnly:
                    message = ResponseMessage.InvalidPatternMaruchoOnly;
                    break;
            }
            return message;
        }
    }
}