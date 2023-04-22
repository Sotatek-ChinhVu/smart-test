namespace Domain.Models.Lock
{
    public class LockModel
    {
        public LockModel(int userId, string userName, DateTime lockDateTime, string functionName, string functionCode, int lockLevel, int lockRange)
        {
            UserId = userId;
            UserName = userName;
            LockDateTime = lockDateTime;
            FunctionName = functionName;
            FunctionCode = functionCode;
            LockLevel = lockLevel;
            LockRange = lockRange;
        }

        public LockModel()
        {
            UserName = string.Empty;
            LockDateTime = DateTime.MinValue;
            FunctionName = string.Empty;
            FunctionCode = string.Empty;
        }

        public int UserId { get; private set; }

        public string UserName { get; private set; }

        public DateTime LockDateTime { get; private set; } 

        public string FunctionName { get; private set; }

        public string FunctionCode { get; private set; }

        public int LockLevel { get; private set; }

        public int LockRange { get; private set; }
    }
}
