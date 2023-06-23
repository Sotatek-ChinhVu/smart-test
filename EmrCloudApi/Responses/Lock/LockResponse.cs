namespace EmrCloudApi.Responses.Lock
{
    public class LockResponse
    {
        public LockResponse(int userId, string userName, int lockLevel, string screenName)
        {
            UserId = userId;
            UserName = userName;
            LockLevel = lockLevel;
            ScreenName = screenName;
        }

        public int UserId { get; private set; }

        public string UserName { get; private set; }

        public int LockLevel { get; private set; }

        public string ScreenName { get; private set; }
    }
}
