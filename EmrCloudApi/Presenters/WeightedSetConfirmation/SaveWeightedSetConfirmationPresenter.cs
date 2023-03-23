using EmrCloudApi.Constants;
using EmrCloudApi.Responses.WeightedSetConfirmation;
using EmrCloudApi.Responses;
using UseCase.WeightedSetConfirmation.Save;

namespace EmrCloudApi.Presenters.WeightedSetConfirmation
{
    public class SaveWeightedSetConfirmationPresenter : ISaveWeightedSetConfirmationOutputPort
    {
        public Response<SaveWeightedSetConfirmationResponse> Result { get; private set; } = new Response<SaveWeightedSetConfirmationResponse>();

        public void Complete(SaveWeightedSetConfirmationOutputData outputData)
        {
            Result.Data = new SaveWeightedSetConfirmationResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveWeightedSetConfirmationStatus status) => status switch
        {
            SaveWeightedSetConfirmationStatus.Successful => ResponseMessage.Success,
            SaveWeightedSetConfirmationStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveWeightedSetConfirmationStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            SaveWeightedSetConfirmationStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            SaveWeightedSetConfirmationStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            SaveWeightedSetConfirmationStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
