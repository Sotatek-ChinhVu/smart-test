using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidateRousaiJibai;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidateRousaiJibaiPresenter : IValidateRousaiJibaiOutputPort
    {
        public Response<ValidateRousaiJibaiResponse> Result { get; private set; } = default!;
        public void Complete(ValidateRousaiJibaiOutputData output)
        {
            Result = new Response<ValidateRousaiJibaiResponse>()
            {
                Data = new ValidateRousaiJibaiResponse(output.Result, output.Message),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                case ValidateRousaiJibaiStatus.InvalidSuccess:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ValidateRousaiJibaiStatus.InvalidHokenKbn:
                    Result.Message = ResponseMessage.InvalidHokenKbn;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSaigaiKbn:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfRousaiSaigaiKbn;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRousaiSyobyoDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfRousaiSyobyoDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoStartDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfRyoyoStartDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfRyoyoEndDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfRyoyoEndDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfStartDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfStartDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfEndDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfEndDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidSelectedHokenInfConfirmDate:
                    Result.Message = ResponseMessage.InvalidSelectedHokenInfConfirmDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ValidateRousaiJibaiStatus.InvalidFaild:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case ValidateRousaiJibaiStatus.InvalidRodoBangoNull:
                    Result.Message = ResponseMessage.InvalidRodoBangoNull;
                    break;
                case ValidateRousaiJibaiStatus.InvalidRodoBangoLengthNotEquals14:
                    Result.Message = ResponseMessage.InvalidRodoBangoLengthNotEquals14;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckItemFirstListRousaiTenki:
                    Result.Message = ResponseMessage.InvalidCheckItemFirstListRousaiTenki;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiSinkei:
                    Result.Message = ResponseMessage.InvalidCheckRousaiTenkiSinkei;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiTenki:
                    Result.Message = ResponseMessage.InvalidCheckRousaiTenkiTenki;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckRousaiTenkiEndDate:
                    Result.Message = ResponseMessage.InvalidCheckRousaiTenkiEndDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckRousaiSaigaiKbnNotEquals1And2:
                    Result.Message = ResponseMessage.InvalidCheckRousaiSaigaiKbnNotEquals1And2;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckRousaiSyobyoDateEquals0:
                    Result.Message = ResponseMessage.InvalidCheckRousaiSyobyoDateEquals0;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull:
                    Result.Message = ResponseMessage.InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckRousaiRyoyoDate:
                    Result.Message = ResponseMessage.InvalidCheckRousaiRyoyoDate;
                    break;
                case ValidateRousaiJibaiStatus.InvalidCheckDateExpirated:
                    Result.Message = ResponseMessage.InvalidCheckDateExpirated;
                    break;
                case ValidateRousaiJibaiStatus.InvalidNenkinBangoIsNull:
                    Result.Message = ResponseMessage.InvalidNenkinBangoIsNull;
                    break;
                case ValidateRousaiJibaiStatus.InvalidNenkinBangoLengthNotEquals9:
                    Result.Message = ResponseMessage.InvalidNenkinBangoLengthNotEquals9;
                    break;
                case ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoIsNull:
                    Result.Message = ResponseMessage.InvalidKenkoKanriBangoIsNull;
                    break;
                case ValidateRousaiJibaiStatus.InvalidKenkoKanriBangoLengthNotEquals13:
                    Result.Message = ResponseMessage.InvalidKenkoKanriBangoLengthNotEquals13;
                    break;
            }
        }
    }
}
