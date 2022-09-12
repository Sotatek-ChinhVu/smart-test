using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.SupperSetDetail;

public class GetSupperSetDetailInputData : IInputData<GetSupperSetDetailOutputData>
{
    public GetSupperSetDetailInputData(int hpId, int setCd)
    {
        HpId = hpId;
        SetCd = setCd;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
}
