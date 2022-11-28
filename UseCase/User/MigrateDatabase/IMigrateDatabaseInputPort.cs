using UseCase.Core.Sync.Core;

namespace UseCase.User.MigrateDatabase
{
    public interface IMigrateDatabaseInputPort : IInputPort<MigrateDatabaseInputData, MigrateDatabaseOutputData>
    {
    }
}
