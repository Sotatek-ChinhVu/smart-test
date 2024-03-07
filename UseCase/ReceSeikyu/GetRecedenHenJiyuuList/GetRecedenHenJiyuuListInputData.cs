using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.GetRecedenHenJiyuuList;

public class GetRecedenHenJiyuuListInputData : IInputData<GetRecedenHenJiyuuListOutputData>
{
    public GetRecedenHenJiyuuListInputData(int hpId, long ptId, int sinYm)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }
}
