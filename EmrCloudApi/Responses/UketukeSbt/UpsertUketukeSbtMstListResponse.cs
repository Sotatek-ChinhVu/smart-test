namespace EmrCloudApi.Responses.UketukeSbt
{
    public class UpsertUketukeSbtMstListResponse
    {
        public UpsertUketukeSbtMstListResponse(bool success)
        {
            Success = success;
        }
        public bool Success { get; private set; }
    }
}