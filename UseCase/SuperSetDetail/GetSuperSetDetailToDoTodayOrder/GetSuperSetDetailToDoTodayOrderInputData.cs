using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

public class GetSuperSetDetailToDoTodayOrderInputData : IInputData<GetSuperSetDetailToDoTodayOrderOutputData>
{
    public GetSuperSetDetailToDoTodayOrderInputData(int hpId, int userId, int setCd, int sinDate)
    {
        HpId = hpId;
        UserId = userId;
        SetCd = setCd;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SetCd { get; private set; }

    public int SinDate { get; private set; }
}
