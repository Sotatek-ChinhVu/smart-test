using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.ValidMainInsurance;

namespace EmrCloudApi.Presenters.Insurance
{
    public class ValidateMainInsurancePresenter : IValidMainInsuranceOutputPort
    {
        public Response<ValidateMainInsuranceReponse> Result { get; private set; } = default!;

        public void Complete(ValidMainInsuranceOutputData outputData)
        {
            outputData.ValidateDetails.ForEach(x =>
            {
                if(string.IsNullOrEmpty(x.Message))
                {
                    switch (x.Status)
                    {
                        case ValidMainInsuranceStatus.ValidSuccess:
                            x.Message = ResponseMessage.Success;
                            break;
                        case ValidMainInsuranceStatus.InvalidHpId:
                            x.Message = ResponseMessage.InvalidHpId;
                            break;
                        case ValidMainInsuranceStatus.InvalidSinDate:
                            x.Message = ResponseMessage.InvalidSinDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidPtBirthday:
                            x.Message = ResponseMessage.InvalidPtBirthday;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenNo:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfHokenNo;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfStartDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfStartDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfEndDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfEndDate;
                            break;
                        case ValidMainInsuranceStatus.InvaliSelectedHokenInfHokensyaMstIsKigoNa:
                            x.Message = ResponseMessage.InvaliSelectedHokenInfHokensyaMstIsKigoNa;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfHonkeKbn:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfHonkeKbn;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm1:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfTokureiYm1;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfTokureiYm2:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfTokureiYm2;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfConfirmDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenInfConfirmDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenMstHokenNo:
                            x.Message = ResponseMessage.InvalidSelectedHokenMstHokenNo;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenMstCheckDegit:
                            x.Message = ResponseMessage.InvalidSelectedHokenMstCheckDegit;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenMstAgeStart:
                            x.Message = ResponseMessage.InvalidSelectedHokenMstAgeStart;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenMstAgeEnd:
                            x.Message = ResponseMessage.InvalidSelectedHokenMstAgeEnd;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenMstStartDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenMstStartDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenMstEndDate:
                            x.Message = ResponseMessage.InvalidSelectedHokenMstEndDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidFaild:
                            x.Message = ResponseMessage.Failed;
                            break;
                        case ValidMainInsuranceStatus.InvalidHpIdNotExist:
                            x.Message = ResponseMessage.InvalidHpIdNotExist;
                            break;
                        case ValidMainInsuranceStatus.InvalidJihiSelectedHokenInfHokenNoEquals0:
                            x.Message = ResponseMessage.InvalidPatternJihiSelectedHokenInfHokenNoEquals0;
                            break;
                        case ValidMainInsuranceStatus.InvalidEmptyHoken:
                            x.Message = ResponseMessage.InvalidPatternEmptyHoken;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenNashiOnly:
                            x.Message = ResponseMessage.InvalidPatternHokenNashiOnly;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue1:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue1;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue21:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue21;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue31:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue31;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue41:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue41;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue51:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue51;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue2:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue2;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue23:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue23;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue24:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue24;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue25:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue25;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue3:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue3;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue34:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue34;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue35:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue35;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue4:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue4;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue45:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue45;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkiValue5:
                            x.Message = ResponseMessage.InvalidPatternTokkiValue5;
                            break;
                        case ValidMainInsuranceStatus.InvalidYukoKigen:
                            x.Message = ResponseMessage.InvalidPatternYukoKigen;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenSyaNoNullAndHokenNoNotEquals0:
                            x.Message = ResponseMessage.InvalidPatternHokenSyaNoNullAndHokenNoNotEquals0;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenNoEquals0:
                            x.Message = ResponseMessage.InvalidPatternHokenNoEquals0;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenNoHaveHokenMst:
                            x.Message = ResponseMessage.InvalidPatternHokenNoHaveHokenMst;
                            break;
                        case ValidMainInsuranceStatus.InvalidHoubetu:
                            x.Message = ResponseMessage.InvalidPatternHoubetu;
                            break;
                        case ValidMainInsuranceStatus.InvalidCheckDigitEquals1:
                            x.Message = ResponseMessage.InvalidPatternCheckDigitEquals1;
                            break;
                        case ValidMainInsuranceStatus.InvalidCheckAgeHokenMst:
                            x.Message = ResponseMessage.InvalidPatternCheckAgeHokenMst;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokensyaNoNull:
                            x.Message = ResponseMessage.InvalidPatternHokensyaNoNull;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokensyaNoEquals0:
                            x.Message = ResponseMessage.InvalidPatternHokensyaNoEquals0;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokensyaNoLength8StartWith39:
                            x.Message = ResponseMessage.InvalidPatternHokensyaNoLength8StartWith39;
                            break;
                        case ValidMainInsuranceStatus.InvalidKigoNull:
                            x.Message = ResponseMessage.InvalidPatternKigoNull;
                            break;
                        case ValidMainInsuranceStatus.InvalidBangoNull:
                            x.Message = ResponseMessage.InvalidPatternBangoNull;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenKbnEquals0:
                            x.Message = ResponseMessage.InvalidPatternHokenKbnEquals0;
                            break;
                        case ValidMainInsuranceStatus.InvalidTokkurei:
                            x.Message = ResponseMessage.InvalidPatternTokkurei;
                            break;
                        case ValidMainInsuranceStatus.InvalidConfirmDateAgeCheck:
                            x.Message = ResponseMessage.InvalidPatternConfirmDateAgeCheck;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenMstStartDate:
                            x.Message = ResponseMessage.InvalidPatternHokenMstStartDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidHokenMstEndDate:
                            x.Message = ResponseMessage.InvalidPatternHokenMstEndDate;
                            break;
                        case ValidMainInsuranceStatus.InvalidSelectedHokenInfHokenEdraNo:
                            x.Message = ResponseMessage.InvalidHokenEdraNo;
                            break;
                    }
                }
            });

            Result = new Response<ValidateMainInsuranceReponse>()
            {
                Data = new ValidateMainInsuranceReponse(outputData.Result, outputData.ValidateDetails)
            };

        }
    }
}
