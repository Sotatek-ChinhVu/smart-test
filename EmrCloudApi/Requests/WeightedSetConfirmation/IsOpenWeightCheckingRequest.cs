namespace EmrCloudApi.Requests.WeightedSetConfirmation
{
    public class IsOpenWeightCheckingRequest
    {
        public IsOpenWeightCheckingRequest(int sinDate, int lastDate, double lastWeight)
        {
            SinDate = sinDate;
            LastDate = lastDate;
            LastWeight = lastWeight;
        }

        public int SinDate { get; private set; }

        public int LastDate { get; private set; }

        public double LastWeight { get; private set; }
    }
}
