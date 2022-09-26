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
                Data = new ValidateMainInsuranceReponse(outputData.Result, outputData.Message),
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
                default:
                    break;
            }
        }
    }
}
