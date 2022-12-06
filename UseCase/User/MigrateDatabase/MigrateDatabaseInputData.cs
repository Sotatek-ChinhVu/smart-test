using UseCase.Core.Sync.Core;

namespace UseCase.User.MigrateDatabase
{
    public class MigrateDatabaseInputData : IInputData<MigrateDatabaseOutputData>
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public MigrateDatabaseInputData(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
