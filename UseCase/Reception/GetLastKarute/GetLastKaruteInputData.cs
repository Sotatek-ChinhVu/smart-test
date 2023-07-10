using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetLastKarute;

public class GetLastKaruteInputData : IInputData<GetLastKaruteOutputData>
{
    public GetLastKaruteInputData(int hpId, long ptNum)
    {
        HpId = hpId;
        PtNum = ptNum;
    }

    public int HpId { get; private set; }

    public long PtNum { get; private set; }
}
