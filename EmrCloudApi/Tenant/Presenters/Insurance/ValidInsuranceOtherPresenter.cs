using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidPatternOther;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class ValidInsuranceOtherPresenter : IValidInsuranceOtherOutputPort
    {
        public Response<ValidInsuranceOtherResponse> Result { get; private set; } = default!;
        public void Complete(ValidInsuranceOtherOutputData output)
        {
            Result = new Response<ValidInsuranceOtherResponse>()
            {

                Data = new ValidInsuranceOtherResponse(output.Result, output.Message),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                case ValidInsuranceOtherStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case ValidInsuranceOtherStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ValidInsuranceOtherStatus.InvalidPtBirthday:
                    Result.Message = ResponseMessage.InvalidPtBirthday;
                    break;
                case ValidInsuranceOtherStatus.InvalidAge75:
                    Result.Message = ResponseMessage.InvalidAge75;
                    break;
                case ValidInsuranceOtherStatus.InvalidAge65:
                    Result.Message = ResponseMessage.InvalidAge65;
                    break;
            }
        }
    }
}
