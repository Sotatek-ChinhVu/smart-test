using UseCase.Core.Sync.Core;

namespace UseCase.WeightedSetConfirmation.Save
{
    public class SaveWeightedSetConfirmationOutputData : IOutputData
    {
        public SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus status)
        {
            Status = status;
        }

        public SaveWeightedSetConfirmationStatus Status { get; private set; }
    }
}
