using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.SupperSetDetail;

public class GetSupperSetDetailInputData : IInputData<GetSupperSetDetailOutputData>
{
    public GetSupperSetDetailInputData(int hpId, int setCd, int sindate)
    {
        HpId = hpId;
        SetCd = setCd;
        Sindate = sindate;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public int Sindate { get; private set; }
}
