using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCmt;

public class GetListReceCmtInputData : IInputData<GetListReceCmtOutputData>
{
    public GetListReceCmtInputData(int hpId, int sinDate, long ptId, int hokenId)
    {
        HpId = hpId;
        SinDate = sinDate;
        PtId = ptId;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }
}
