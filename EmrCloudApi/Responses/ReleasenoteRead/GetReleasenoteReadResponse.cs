namespace EmrCloudApi.Responses.ReleasenoteRead
{
    public class GetReleasenoteReadResponse
    {
        public GetReleasenoteReadResponse(List<string> version)
        {
            Version = version;
        }

        public List<string> Version { get; private set; }
    }
}
