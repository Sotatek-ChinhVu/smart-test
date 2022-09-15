using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SuperSetDetail;

public class GetSuperSetDetailInputData : IInputData<GetSuperSetDetailOutputData>
{
    public GetSuperSetDetailInputData(int hpId, int setCd)
    {
        HpId = hpId;
        SetCd = setCd;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
}
