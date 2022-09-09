using UseCase.Core.Sync.Core;

namespace UseCase.User.GetByLoginId;

public class GetUserByLoginIdInputData : IInputData<GetUserByLoginIdOutputData>
{
    public GetUserByLoginIdInputData(string loginId)
    {
        LoginId = loginId;
    }

    public string LoginId { get; private set; }
}
