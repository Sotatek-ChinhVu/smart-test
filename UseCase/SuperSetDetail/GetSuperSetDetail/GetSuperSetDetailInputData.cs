using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SuperSetDetail;

public class GetSuperSetDetailInputData : IInputData<GetSuperSetDetailOutputData>
{
    public GetSuperSetDetailInputData(int hpId, int setCd, int sindate)
    {
        HpId = hpId;
        SetCd = setCd;
        Sindate = sindate;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public int Sindate { get; private set; }
}
