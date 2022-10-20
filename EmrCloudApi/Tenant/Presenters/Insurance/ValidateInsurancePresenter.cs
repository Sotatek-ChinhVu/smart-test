using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidateInsurance;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidateInsurancePresenter : IValidateInsuranceOutputPort
    {
        public Response<ValidateInsuranceResponse> Result { get; private set; } = default!;

        public void Complete(ValidateInsuranceOutputData outputData)
        {
            Result = new Response<ValidateInsuranceResponse>
            {
                Data = new ValidateInsuranceResponse(outputData.Result, outputData.Message, outputData.IndexItemError),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ValidateInsuranceStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ValidateInsuranceStatus.InvalidFaild:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case ValidateInsuranceStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ValidateInsuranceStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidateInsuranceStatus.InvalidPtBirthday:
                    Result.Message = ResponseMessage.InvalidPtBirthday;
                    break;
                case ValidateInsuranceStatus.InvalidJihiSelectedHokenInfHokenNoEquals0:
                    Result.Message = ResponseMessage.InvalidPatternJihiSelectedHokenInfHokenNoEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidEmptyHoken:
                    Result.Message = ResponseMessage.InvalidPatternEmptyHoken;
                    break;
                case ValidateInsuranceStatus.InvalidHokenNashiOnly:
                    Result.Message = ResponseMessage.InvalidPatternHokenNashiOnly;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue1:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue1;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue21:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue21;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue31:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue31;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue41:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue41;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue51:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue51;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue2:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue2;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue23:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue23;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue24:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue24;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue25:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue25;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue3:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue3;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue34:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue34;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue35:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue35;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue4:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue4;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue45:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue45;
                    break;
                case ValidateInsuranceStatus.InvalidTokkiValue5:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue5;
                    break;
                case ValidateInsuranceStatus.InvalidYukoKigen:
                    Result.Message = ResponseMessage.InvalidPatternYukoKigen;
                    break;
                case ValidateInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokenSyaNoNullAndHokenNoNotEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidHokenNoEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokenNoEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidHokenNoHaveHokenMst:
                    Result.Message = ResponseMessage.InvalidPatternHokenNoHaveHokenMst;
                    break;
                case ValidateInsuranceStatus.InvalidHoubetu:
                    Result.Message = ResponseMessage.InvalidPatternHoubetu;
                    break;
                case ValidateInsuranceStatus.InvalidCheckDigitEquals1:
                    Result.Message = ResponseMessage.InvalidPatternCheckDigitEquals1;
                    break;
                case ValidateInsuranceStatus.InvalidCheckAgeHokenMst:
                    Result.Message = ResponseMessage.InvalidPatternCheckAgeHokenMst;
                    break;
                case ValidateInsuranceStatus.InvalidHokensyaNoNull:
                    Result.Message = ResponseMessage.InvalidPatternHokensyaNoNull;
                    break;
                case ValidateInsuranceStatus.InvalidHokensyaNoEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokensyaNoEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidHokensyaNoLength8StartWith39:
                    Result.Message = ResponseMessage.InvalidPatternHokensyaNoLength8StartWith39;
                    break;
                case ValidateInsuranceStatus.InvalidKigoNull:
                    Result.Message = ResponseMessage.InvalidPatternKigoNull;
                    break;
                case ValidateInsuranceStatus.InvalidBangoNull:
                    Result.Message = ResponseMessage.InvalidPatternBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidHokenKbnEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokenKbnEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidTokkurei:
                    Result.Message = ResponseMessage.InvalidPatternTokkurei;
                    break;
                case ValidateInsuranceStatus.InvalidConfirmDateAgeCheck:
                    Result.Message = ResponseMessage.InvalidPatternConfirmDateAgeCheck;
                    break;
                case ValidateInsuranceStatus.InvalidHokenMstStartDate:
                    Result.Message = ResponseMessage.InvalidPatternHokenMstStartDate;
                    break;
                case ValidateInsuranceStatus.InvalidHokenMstEndDate:
                    Result.Message = ResponseMessage.InvalidPatternHokenMstEndDate;
                    break;
                case ValidateInsuranceStatus.InvalidRodoBangoNull:
                    Result.Message = ResponseMessage.InvalidPatternHokenRodoBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidRodoBangoLengthNotEquals14:
                    Result.Message = ResponseMessage.InvalidPatternRodoBangoLengthNotEquals14;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiTenkiDefaultRow:
                    Result.Message = ResponseMessage.InvalidPatternRousaiTenkiDefaultRow;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiTenkiData:
                    Result.Message = ResponseMessage.InvalidPatternRousaiTenkiData;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiSaigaiKbn:
                    Result.Message = ResponseMessage.InvalidPatternRousaiSaigaiKbn;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiSyobyoDateEquals0:
                    Result.Message = ResponseMessage.InvalidPatternRousaiSyobyoDateEquals0;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiSyobyoCdNull:
                    Result.Message = ResponseMessage.InvalidPatternRousaiSyobyoCdNull;
                    break;
                case ValidateInsuranceStatus.InvalidRousaiRyoyoDate:
                    Result.Message = ResponseMessage.InvalidPatternRousaiRyoyoDate;
                    break;
                case ValidateInsuranceStatus.InvalidRosaiYukoDate:
                    Result.Message = ResponseMessage.InvalidPatternRosaiYukoDate;
                    break;
                case ValidateInsuranceStatus.InvalidCheckHokenInfDate:
                    Result.Message = ResponseMessage.InvalidPatternCheckHokenInfDate;
                    break;
                case ValidateInsuranceStatus.InvalidNenkinBangoNull:
                    Result.Message = ResponseMessage.InvalidPatternNenkinBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidNenkinBangoLengthNotEquals9:
                    Result.Message = ResponseMessage.InvalidPatternNenkinBangoLengthNotEquals9;
                    break;
                case ValidateInsuranceStatus.InvalidKenkoKanriBangoNull:
                    Result.Message = ResponseMessage.InvalidPatternKenkoKanriBangoNull;
                    break;
                case ValidateInsuranceStatus.InvalidKenkoKanriBangoLengthNotEquals13:
                    Result.Message = ResponseMessage.InvalidPatternKenkoKanriBangoLengthNotEquals13;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel1:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmptyModel1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty1:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEmpty1;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty1:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNoEmpty1;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo1:
                    Result.Message = ResponseMessage.InvalidPatternJyukyusyaNo1;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo1:
                    Result.Message = ResponseMessage.InvalidPatternTokusyuNo1;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo01:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNo01;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate1:
                    Result.Message = ResponseMessage.InvalidPatternKohiYukoDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate1:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstStartDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate1:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEndDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate1:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT1:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckHBT1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo1:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo1:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge1:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge1;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull1:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull1;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel2:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmptyModel2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty2:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEmpty2;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty2:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNoEmpty2;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo2:
                    Result.Message = ResponseMessage.InvalidPatternJyukyusyaNo2;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo2:
                    Result.Message = ResponseMessage.InvalidPatternTokusyuNo2;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo02:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNo02;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate2:
                    Result.Message = ResponseMessage.InvalidPatternKohiYukoDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate2:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstStartDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate2:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEndDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate2:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT2:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckHBT2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo2:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo2:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge2:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge2;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull2:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull2;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel3:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmptyModel3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty3:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEmpty3;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty3:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNoEmpty3;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo3:
                    Result.Message = ResponseMessage.InvalidPatternJyukyusyaNo3;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo3:
                    Result.Message = ResponseMessage.InvalidPatternTokusyuNo3;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo03:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNo03;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate3:
                    Result.Message = ResponseMessage.InvalidPatternKohiYukoDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate3:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstStartDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate3:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEndDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate3:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT3:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckHBT3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo3:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo3:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge3:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge3;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull3:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull3;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmptyModel4:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmptyModel4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEmpty4:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEmpty4;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNoEmpty4:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNoEmpty4;
                    break;
                case ValidateInsuranceStatus.InvalidJyukyusyaNo4:
                    Result.Message = ResponseMessage.InvalidPatternJyukyusyaNo4;
                    break;
                case ValidateInsuranceStatus.InvalidTokusyuNo4:
                    Result.Message = ResponseMessage.InvalidPatternTokusyuNo4;
                    break;
                case ValidateInsuranceStatus.InvalidFutansyaNo04:
                    Result.Message = ResponseMessage.InvalidPatternFutansyaNo04;
                    break;
                case ValidateInsuranceStatus.InvalidKohiYukoDate4:
                    Result.Message = ResponseMessage.InvalidPatternKohiYukoDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstStartDate4:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstStartDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiHokenMstEndDate4:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstEndDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiConfirmDate4:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckHBT4:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckHBT4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitFutansyaNo4:
                    Result.Message = ResponseMessage.InvalidPatternKohiMstCheckDigitFutansyaNo4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckDigitJyukyusyaNo4:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiMstCheckAge4:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstCheckAge4;
                    break;
                case ValidateInsuranceStatus.InvalidFutanJyoTokuNull4:
                    Result.Message = ResponseMessage.InvalidPatternKohiHokenMstFutanJyoTokuNull4;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty21:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmpty21;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty31:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmpty31;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty32:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmpty32;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty41:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmpty41;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty42:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmpty42;
                    break;
                case ValidateInsuranceStatus.InvalidKohiEmpty43:
                    Result.Message = ResponseMessage.InvalidPatternKohiEmpty43;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi1:
                    Result.Message = ResponseMessage.InvalidPatternDuplicateKohi1;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi2:
                    Result.Message = ResponseMessage.InvalidPatternDuplicateKohi2;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi3:
                    Result.Message = ResponseMessage.InvalidPatternDuplicateKohi3;
                    break;
                case ValidateInsuranceStatus.InvalidDuplicateKohi4:
                    Result.Message = ResponseMessage.InvalidPatternDuplicateKohi4;
                    break;
                case ValidateInsuranceStatus.InvalidWarningDuplicatePattern:
                    Result.Message = ResponseMessage.InvalidPatternWarningDuplicatePattern;
                    break;
                case ValidateInsuranceStatus.InvalidWarningAge75:
                    Result.Message = ResponseMessage.InvalidPatternWarningAge75;
                    break;
                case ValidateInsuranceStatus.InvalidWarningAge65:
                    Result.Message = ResponseMessage.InvalidPatternWarningAge65;
                    break;
                case ValidateInsuranceStatus.InvalidMaruchoOnly:
                    Result.Message = ResponseMessage.InvalidPatternMaruchoOnly;
                    break;
            }
        }
    }
}