using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetByLoginId;

public class GetUserByLoginIdOutputData : IOutputData
{
    public GetUserByLoginIdOutputData(GetUserByLoginIdStatus status)
    {
        Status = status;
    }

    public GetUserByLoginIdOutputData(GetUserByLoginIdStatus status, UserMstModel? user)
    {
        Status = status;
        User = user;
    }

    public GetUserByLoginIdStatus Status { get; private set; }
    public UserMstModel? User { get; private set; }
}
