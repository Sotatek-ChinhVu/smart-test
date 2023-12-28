namespace EmrCloudApi.Responses.Cache
{
    public class RemoveCacheResponse
    {
        public RemoveCacheResponse(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}
