using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SuperSetDetail;

public class GetSuperSetDetailInputData : IInputData<GetSuperSetDetailOutputData>
{
    public GetSuperSetDetailInputData(int hpId, int userId, int setCd, int sindate)
    {
        HpId = hpId;
        UserId = userId;
        SetCd = setCd;
        Sindate = sindate;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SetCd { get; private set; }

    public int Sindate { get; private set; }
}
