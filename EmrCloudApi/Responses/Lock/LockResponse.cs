namespace EmrCloudApi.Responses.Lock
{
    public class LockResponse
    {
        public LockResponse(string userName, int lockLevel, string screenName)
        {
            UserName = userName;
            LockLevel = lockLevel;
            ScreenName = screenName;
        }

        public string UserName { get; private set; }

        public int LockLevel { get; private set; }

        public string ScreenName { get; private set; }
    }
}
