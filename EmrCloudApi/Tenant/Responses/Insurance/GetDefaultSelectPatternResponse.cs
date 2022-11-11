namespace EmrCloudApi.Tenant.Responses.InsuranceList
{
    public class GetDefaultSelectPatternResponse
    {
        public GetDefaultSelectPatternResponse(int hokenPid)
        {
            HokenPid = hokenPid;
        }

        public int HokenPid { get; private set; }
    }
}