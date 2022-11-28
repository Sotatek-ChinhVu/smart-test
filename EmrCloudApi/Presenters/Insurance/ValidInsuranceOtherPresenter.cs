using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.ValidPatternOther;

namespace EmrCloudApi.Presenters.Insurance
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
                    Result.Message = ResponseMessage.InvalidPatternOtherAge75;
                    break;
                case ValidInsuranceOtherStatus.InvalidAge65:
                    Result.Message = ResponseMessage.InvalidPatternOtherAge65;
                    break;
                case ValidInsuranceOtherStatus.InvalidDuplicatePattern:
                    Result.Message = ResponseMessage.InvalidCheckDuplicatePattern;
                    break;
            }
        }
    }
}
