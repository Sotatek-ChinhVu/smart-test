using UseCase.Core.Sync.Core;
using static Helper.Constants.UserConfConst;

namespace UseCase.User.UpsertUserConfList;

public class UpsertUserConfListOutputData : IOutputData
{
    public UpsertUserConfListOutputData(UpsertUserConfListStatus status, List<UserConfItemValidation> userConfItemValidations)
    {
        Status = status;
        UserConfItemValidations = userConfItemValidations;
    }

    public UpsertUserConfListStatus Status { get; private set; }

    public List<UserConfItemValidation> UserConfItemValidations { get; private set; }
}

public class UserConfItemValidation
{
    public UserConfItemValidation(int position, UserConfStatus status)
    {
        Position = position;
        Status = status;
    }

    public int Position { get; private set; }

    public UserConfStatus Status { get; private set; }
}
