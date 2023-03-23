using UseCase.Core.Sync.Core;

namespace UseCase.WeightedSetConfirmation.CheckOpen
{
    public class IsOpenWeightCheckingInputData : IInputData<IsOpenWeightCheckingOutputData>
    {
        public IsOpenWeightCheckingInputData(int hpId, int sinDate, int lastDate, double lastWeight)
        {
            HpId = hpId;
            SinDate = sinDate;
            LastDate = lastDate;
            LastWeight = lastWeight;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int LastDate { get; private set; }

        public double LastWeight { get; private set; }
    }
}
