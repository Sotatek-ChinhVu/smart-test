using EmrCloudApi.Constants;
using EmrCloudApi.Responses.SwapHoken;
using EmrCloudApi.Responses;
using UseCase.SwapHoken.Validate;

namespace EmrCloudApi.Presenters.SwapHoken
{
    public class ValidateSwapHokenPresenter : IValidateSwapHokenOutputPort
    {
        public Response<ValidateSwapHokenResponse> Result { get; private set; } = new Response<ValidateSwapHokenResponse>();

        public void Complete(ValidateSwapHokenOutputData outputData)
        {
            Result.Data = new ValidateSwapHokenResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = outputData.Message ?? GetMessage(outputData.Status);
        }

        private string GetMessage(ValidateSwapHokenStatus status) => status switch
        {
            ValidateSwapHokenStatus.Successful => ResponseMessage.Success,
            ValidateSwapHokenStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            ValidateSwapHokenStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            ValidateSwapHokenStatus.InvalidStartDate => ResponseMessage.InvalidStarDate,
            ValidateSwapHokenStatus.InvalidEndDate => ResponseMessage.InvalidEndDate,
            ValidateSwapHokenStatus.InvalidHokenPid => ResponseMessage.InvalidHokenId,
            _ => string.Empty
        };
    }
}
