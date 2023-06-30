using UseCase.Core.Sync.Core;

namespace UseCase.SinKoui.GetSinKoui;

public class GetSinKouiInputData : IInputData<GetSinKouiListOutputData>
{
    public GetSinKouiInputData(int hpId, long ptId)
    {
        HpId = hpId; 
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
