using UseCase.Core.Sync.Core;

namespace UseCase.User.UpdateUserConf;

public class UpdateUserConfInputData : IInputData<UpdateUserConfOutputData>
{
    public UpdateUserConfInputData(int hpId, int userId, int grpCd, int value)
    {
        HpId = hpId;
        UserId = userId;
        GrpCd = grpCd;
        Value = value;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public int GrpCd { get; private set; }
    public int Value { get; private set; }
}
