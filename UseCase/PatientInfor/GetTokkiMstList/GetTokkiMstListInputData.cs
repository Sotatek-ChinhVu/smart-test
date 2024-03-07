using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetTokiMstList;

public class GetTokkiMstListInputData : IInputData<GetTokkiMstListOutputData>
{
    public GetTokkiMstListInputData(int seikyuYm)
    {
        SeikyuYm = seikyuYm;
    }

    public int SeikyuYm { get; private set; }
}
