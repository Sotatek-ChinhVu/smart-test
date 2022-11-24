using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidMainInsurance;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidateMainInsurancePresenter : IValidMainInsuranceOutputPort
    {
        public Response<ValidateMainInsuranceReponse> Result { get; private set; } = default!;

        public void Complete(ValidMainInsuranceOutputData outputData)
        {
            Result = new Response<ValidateMainInsuranceReponse>
            {
                Data = new ValidateMainInsuranceReponse(outputData.Result, outputData.Message, outputData.TypeMessage),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ValidMainInsuranceStatus.ValidSuccess:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ValidMainInsuranceStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ValidMainInsuranceStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidMainInsuranceStatus.InvalidPtBirthday:
                    Result.Message = ResponseMessage.InvalidPtBirthday;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenNo:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfHokenNo;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfStartDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfStartDate;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfEndDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfEndDate;
                    break;
                case ValidMainInsuranceStatus.InvaliSelectedHokenInfHokensyaMstIsKigoNa:
                    Result.Message = ResponseMessage.InvaliSelectedHokenInfHokensyaMstIsKigoNa;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfHonkeKbn;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm1:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfTokureiYm1;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm2:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfTokureiYm2;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfConfirmDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfConfirmDate;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenMstHokenNo:
                    Result.Message = ResponseMessage.InvalidSelectedHokenMstHokenNo;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenMstCheckDegit:
                    Result.Message = ResponseMessage.InvalidSelectedHokenMstCheckDegit;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenMstAgeStart:
                    Result.Message = ResponseMessage.InvalidSelectedHokenMstAgeStart;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenMstAgeEnd:
                    Result.Message = ResponseMessage.InvalidSelectedHokenMstAgeEnd;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenMstStartDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenMstStartDate;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenMstEndDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenMstEndDate;
                    break;
                case ValidMainInsuranceStatus.InvalidFaild:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case ValidMainInsuranceStatus.InvalidHpIdNotExist:
                    Result.Message = ResponseMessage.InvalidHpIdNotExist;
                    break;
                case ValidMainInsuranceStatus.InvalidJihiSelectedHokenInfHokenNoEquals0:
                    Result.Message = ResponseMessage.InvalidPatternJihiSelectedHokenInfHokenNoEquals0;
                    break;
                case ValidMainInsuranceStatus.InvalidEmptyHoken:
                    Result.Message = ResponseMessage.InvalidPatternEmptyHoken;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenNashiOnly:
                    Result.Message = ResponseMessage.InvalidPatternHokenNashiOnly;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue1:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue1;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue21:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue21;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue31:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue31;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue41:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue41;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue51:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue51;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue2:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue2;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue23:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue23;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue24:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue24;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue25:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue25;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue3:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue3;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue34:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue34;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue35:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue35;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue4:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue4;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue45:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue45;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkiValue5:
                    Result.Message = ResponseMessage.InvalidPatternTokkiValue5;
                    break;
                case ValidMainInsuranceStatus.InvalidYukoKigen:
                    Result.Message = ResponseMessage.InvalidPatternYukoKigen;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokenSyaNoNullAndHokenNoNotEquals0;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenNoEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokenNoEquals0;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenNoHaveHokenMst:
                    Result.Message = ResponseMessage.InvalidPatternHokenNoHaveHokenMst;
                    break;
                case ValidMainInsuranceStatus.InvalidHoubetu:
                    Result.Message = ResponseMessage.InvalidPatternHoubetu;
                    break;
                case ValidMainInsuranceStatus.InvalidCheckDigitEquals1:
                    Result.Message = ResponseMessage.InvalidPatternCheckDigitEquals1;
                    break;
                case ValidMainInsuranceStatus.InvalidCheckAgeHokenMst:
                    Result.Message = ResponseMessage.InvalidPatternCheckAgeHokenMst;
                    break;
                case ValidMainInsuranceStatus.InvalidHokensyaNoNull:
                    Result.Message = ResponseMessage.InvalidPatternHokensyaNoNull;
                    break;
                case ValidMainInsuranceStatus.InvalidHokensyaNoEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokensyaNoEquals0;
                    break;
                case ValidMainInsuranceStatus.InvalidHokensyaNoLength8StartWith39:
                    Result.Message = ResponseMessage.InvalidPatternHokensyaNoLength8StartWith39;
                    break;
                case ValidMainInsuranceStatus.InvalidKigoNull:
                    Result.Message = ResponseMessage.InvalidPatternKigoNull;
                    break;
                case ValidMainInsuranceStatus.InvalidBangoNull:
                    Result.Message = ResponseMessage.InvalidPatternBangoNull;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenKbnEquals0:
                    Result.Message = ResponseMessage.InvalidPatternHokenKbnEquals0;
                    break;
                case ValidMainInsuranceStatus.InvalidTokkurei:
                    Result.Message = ResponseMessage.InvalidPatternTokkurei;
                    break;
                case ValidMainInsuranceStatus.InvalidConfirmDateAgeCheck:
                    Result.Message = ResponseMessage.InvalidPatternConfirmDateAgeCheck;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenMstStartDate:
                    Result.Message = ResponseMessage.InvalidPatternHokenMstStartDate;
                    break;
                case ValidMainInsuranceStatus.InvalidHokenMstEndDate:
                    Result.Message = ResponseMessage.InvalidPatternHokenMstEndDate;
                    break;
                case ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenEdraNo:
                    Result.Message = ResponseMessage.InvalidHokenEdraNo;
                    break;
            }
        }
    }
}
