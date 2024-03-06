using UseCase.Core.Sync.Core;

namespace UseCase.User.GetList;

public class GetUserListInputData : IInputData<GetUserListOutputData>
{
    public GetUserListInputData(int hpId, int sinDate, bool isDoctorOnly, bool isAll)
    {
        HpId = hpId;
        SinDate = sinDate;
        IsDoctorOnly = isDoctorOnly;
        IsAll = isAll;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public bool IsDoctorOnly { get; private set; }
    public bool IsAll { get; private set; }
}
