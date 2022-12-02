namespace EmrCloudApi.Responses.OrdInfs
{
    public class GetMaxRpNoResponse
    {
        public GetMaxRpNoResponse(long maxRpNo)
        {
            MaxRpNo = maxRpNo;
        }

        public long MaxRpNo { get; private set; }
    }
}
