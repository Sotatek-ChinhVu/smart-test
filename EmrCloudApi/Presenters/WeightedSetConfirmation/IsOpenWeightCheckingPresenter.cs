using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.WeightedSetConfirmation;
using UseCase.WeightedSetConfirmation.CheckOpen;

namespace EmrCloudApi.Presenters.WeightedSetConfirmation
{
    public class IsOpenWeightCheckingPresenter : IIsOpenWeightCheckingOutputPort
    {
        public Response<IsOpenWeightCheckingResponse> Result { get; private set; } = new Response<IsOpenWeightCheckingResponse>();

        public void Complete(IsOpenWeightCheckingOutputData outputData)
        {
            Result.Data = new IsOpenWeightCheckingResponse(outputData.IsAllow);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(IsOpenWeightCheckingStatus status) => status switch
        {
            IsOpenWeightCheckingStatus.Successful => ResponseMessage.Success,
            IsOpenWeightCheckingStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}
