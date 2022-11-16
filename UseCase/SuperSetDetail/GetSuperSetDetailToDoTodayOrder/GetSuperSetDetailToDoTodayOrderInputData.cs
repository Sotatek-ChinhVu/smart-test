using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

public class GetSuperSetDetailToDoTodayOrderInputData : IInputData<GetSuperSetDetailToDoTodayOrderOutputData>
{
    public GetSuperSetDetailToDoTodayOrderInputData(int hpId, int setCd, int sinDate)
    {
        HpId = hpId;
        SetCd = setCd;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public int SetCd { get; private set; }

    public int SinDate { get; private set; }
}
