using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertList
{
    public interface IUpsertUserListInputPort : IInputPort<UpsertUserListInputData, UpsertUserListOutputData>
    {
        new UpsertUserListOutputData Handle(UpsertUserListInputData inputData);
    }
}
