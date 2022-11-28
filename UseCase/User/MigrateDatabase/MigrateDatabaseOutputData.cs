using UseCase.Core.Sync.Core;

namespace UseCase.User.MigrateDatabase
{
    public class MigrateDatabaseOutputData : IOutputData
    {
        public MigrateDatabaseStatus Status { get; private set; }

        public MigrateDatabaseOutputData(MigrateDatabaseStatus status)
        {
            Status = status;
        }
    }
}
