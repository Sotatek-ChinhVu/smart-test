namespace EmrCloudApi.Responses.Lock
{
    public class LockResponse
    {
        public string UserName { get; set; } = string.Empty;

        public int LockLevel { get; set; }

        public string ScreenName { get; set; } = string.Empty;
    }
}
