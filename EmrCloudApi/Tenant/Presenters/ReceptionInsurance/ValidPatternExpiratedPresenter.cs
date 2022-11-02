using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ReceptionInsurance;
using UseCase.Insurance.ValidPatternExpirated;

namespace EmrCloudApi.Tenant.Presenters.ReceptionInsurance
{
    public class ValidPatternExpiratedPresenter : IValidPatternExpiratedOutputPort
    {
        public Response<ValidPatternExpiratedResponse> Result { get; private set; } = default!;

        public void Complete(ValidPatternExpiratedOutputData outputData)
        {
            Result = new Response<ValidPatternExpiratedResponse>
            {
                Data = new ValidPatternExpiratedResponse(outputData.Result, outputData.Message),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ValidPatternExpiratedStatus.ValidPatternExpiratedSuccess:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ValidPatternExpiratedStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ValidPatternExpiratedStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case ValidPatternExpiratedStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidPatternExpiratedStatus.InvalidPatternHokenPid:
                    Result.Message = ResponseMessage.InvalidPatternHokenPid;
                    break;
                case ValidPatternExpiratedStatus.InvalidPatternConfirmDate:
                    Result.Message = ResponseMessage.InvalidPatternConfirmDate;
                    break;
                case ValidPatternExpiratedStatus.InvalidHokenInfStartDate:
                    Result.Message = ResponseMessage.InvalidHokenInfStartDate;
                    break;
                case ValidPatternExpiratedStatus.InvalidHokenInfEndDate:
                    Result.Message = ResponseMessage.InvalidHokenInfEndDate;
                    break;
                case ValidPatternExpiratedStatus.InvalidKohiConfirmDate1:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate1;
                    break;
                case ValidPatternExpiratedStatus.InvalidKohiConfirmDate2:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate2;
                    break;
                case ValidPatternExpiratedStatus.InvalidKohiConfirmDate3:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate3;
                    break;
                case ValidPatternExpiratedStatus.InvalidKohiConfirmDate4:
                    Result.Message = ResponseMessage.InvalidPatternKohiConfirmDate4;
                    break;
                case ValidPatternExpiratedStatus.InvalidPatientInfBirthday:
                    Result.Message = ResponseMessage.InvalidPtBirthday;
                    break;
                case ValidPatternExpiratedStatus.InvalidPatternHokenKbn:
                    Result.Message = ResponseMessage.InvalidHokenKbn;
                    break;
                case ValidPatternExpiratedStatus.InvalidConfirmDateAgeCheck:
                    Result.Message = ResponseMessage.InvalidConfirmDateAgeCheck;
                    break;
                case ValidPatternExpiratedStatus.InvalidConfirmDateHoken:
                    Result.Message = ResponseMessage.InvalidConfirmDateHoken;
                    break;
                case ValidPatternExpiratedStatus.InvalidHokenMstDate:
                    Result.Message = ResponseMessage.InvalidHokenMstDate;
                    break;
                case ValidPatternExpiratedStatus.InvalidConfirmDateKohi1:
                    Result.Message = ResponseMessage.InvalidConfirmDateKohi1;
                    break;
                case ValidPatternExpiratedStatus.InvalidMasterDateKohi1:
                    Result.Message = ResponseMessage.InvalidMasterDateKohi1;
                    break;
                case ValidPatternExpiratedStatus.InvalidConfirmDateKohi2:
                    Result.Message = ResponseMessage.InvalidConfirmDateKohi2;
                    break;
                case ValidPatternExpiratedStatus.InvalidMasterDateKohi2:
                    Result.Message = ResponseMessage.InvalidMasterDateKohi2;
                    break;
                case ValidPatternExpiratedStatus.InvalidConfirmDateKohi3:
                    Result.Message = ResponseMessage.InvalidConfirmDateKohi3;
                    break;
                case ValidPatternExpiratedStatus.InvalidMasterDateKohi3:
                    Result.Message = ResponseMessage.InvalidMasterDateKohi3;
                    break;
                case ValidPatternExpiratedStatus.InvalidConfirmDateKohi4:
                    Result.Message = ResponseMessage.InvalidConfirmDateKohi4;
                    break;
                case ValidPatternExpiratedStatus.InvalidMasterDateKohi4:
                    Result.Message = ResponseMessage.InvalidMasterDateKohi4;
                    break;
                case ValidPatternExpiratedStatus.InvalidPatternIsExpirated:
                    Result.Message = ResponseMessage.InvalidPatternIsExpirated;
                    break;
                case ValidPatternExpiratedStatus.InvalidHasElderHoken:
                    Result.Message = ResponseMessage.InvalidHasElderHoken;
                    break;
                default:
                    break;
            }
        }
    }
}
