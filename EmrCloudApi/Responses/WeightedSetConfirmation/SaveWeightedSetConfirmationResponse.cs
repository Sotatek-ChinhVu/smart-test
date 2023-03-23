using UseCase.WeightedSetConfirmation.Save;

namespace EmrCloudApi.Responses.WeightedSetConfirmation
{
    public class SaveWeightedSetConfirmationResponse
    {
        public SaveWeightedSetConfirmationResponse(SaveWeightedSetConfirmationStatus status)
        {
            Status = status;
        }

        public SaveWeightedSetConfirmationStatus Status { get; private set; }
    }
}
