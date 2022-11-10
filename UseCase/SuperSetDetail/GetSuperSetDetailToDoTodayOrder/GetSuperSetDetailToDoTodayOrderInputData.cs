using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

public class GetSuperSetDetailToDoTodayOrderInputData : IInputData<GetSuperSetDetailToDoTodayOrderOutputData>
{
    public GetSuperSetDetailToDoTodayOrderInputData(int hpId, int setCd)
    {
        HpId = hpId;
        SetCd = setCd;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
}
