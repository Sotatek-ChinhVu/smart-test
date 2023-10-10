using UseCase.Lock.Add;

namespace EmrCloudApi.Responses.Lock
{
    public class LockResponse
    {
        public LockResponse(int userId, string userName, int lockLevel, string screenName, AddLockInputData addLockInputData)
        {
            UserId = userId;
            UserName = userName;
            LockLevel = lockLevel;
            ScreenName = screenName;
            AddLockInputData = addLockInputData;
        }

        public LockResponse(int userId, string userName, int lockLevel, string screenName)
        {
            UserId = userId;
            UserName = userName;
            LockLevel = lockLevel;
            ScreenName = screenName;
            AddLockInputData = new();
        }

        public int UserId { get; private set; }

        public string UserName { get; private set; }

        public int LockLevel { get; private set; }

        public string ScreenName { get; private set; }

        public AddLockInputData AddLockInputData { get; private set; }
    }
}
