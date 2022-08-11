using UseCase.Core.Sync.Core;

namespace UseCase.User.GetList;

public class GetUserListInputData : IInputData<GetUserListOutputData>
{
    public GetUserListInputData(int sinDate, bool isDoctorOnly)
    {
        SinDate = sinDate;
        IsDoctorOnly = isDoctorOnly;
    }

    public int SinDate { get; private set; }
    public bool IsDoctorOnly { get; private set; }
}
