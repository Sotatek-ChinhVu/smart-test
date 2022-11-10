using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

public class GetSuperSetDetailToDoTodayOrderInputData : IInputData<GetSuperSetDetailToDoTodayOrderOutputData>
{
    public GetSuperSetDetailToDoTodayOrderInputData(int hpId, int setCd, int sindate)
    {
        HpId = hpId;
        SetCd = setCd;
        Sindate = sindate;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public int Sindate { get; private set; }
}
