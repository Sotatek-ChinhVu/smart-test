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
                default:
                    break;
            }
        }
    }
}
