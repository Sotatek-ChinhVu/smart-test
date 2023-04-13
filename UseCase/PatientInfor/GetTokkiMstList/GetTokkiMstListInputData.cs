using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetTokiMstList;

public class GetTokkiMstListInputData : IInputData<GetTokkiMstListOutputData>
{
    public GetTokkiMstListInputData(int hpId, int seikyuYm)
    {
        HpId = hpId;
        SeikyuYm = seikyuYm;
    }

    public int HpId { get; private set; }

    public int SeikyuYm { get; private set; }
}
