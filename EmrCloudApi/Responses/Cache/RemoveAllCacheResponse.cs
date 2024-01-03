namespace EmrCloudApi.Responses.Cache
{
    public class RemoveAllCacheResponse
    {
        public RemoveAllCacheResponse(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}
