using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfDetails;

public class GetYousiki1InfDetailsInputData : IInputData<GetYousiki1InfDetailsOutputData>
{
    public GetYousiki1InfDetailsInputData(int hpId, int sinYm, long ptId, int dataType, int seqNo)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtId = ptId;
        DataType = dataType;
        SeqNo = seqNo;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int DataType { get; private set; }

    public int SeqNo { get; private set; }
}
