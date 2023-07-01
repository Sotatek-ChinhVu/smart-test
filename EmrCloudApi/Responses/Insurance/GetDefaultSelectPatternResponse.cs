namespace EmrCloudApi.Responses.InsuranceList
{
    public class GetDefaultSelectPatternResponse
    {
        public GetDefaultSelectPatternResponse(List<int> hokenPids)
        {
            HokenPids = hokenPids;
        }

        public List<int> HokenPids { get; private set; }
    }
}