namespace EmrCloudApi.Responses.WeightedSetConfirmation
{
    public class IsOpenWeightCheckingResponse
    {
        public IsOpenWeightCheckingResponse(bool allow)
        {
            Allow = allow;
        }

        public bool Allow { get; private set; }
    }
}
