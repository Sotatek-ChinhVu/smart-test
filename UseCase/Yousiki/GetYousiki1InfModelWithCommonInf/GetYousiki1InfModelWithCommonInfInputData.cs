using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;

public class GetYousiki1InfModelWithCommonInfInputData : IInputData<GetYousiki1InfModelWithCommonInfOutputData>
{
    public GetYousiki1InfModelWithCommonInfInputData(int hpId, int sinYm, long ptNum, int dataTypes, int status)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtNum = ptNum;
        DataTypes = dataTypes;
        Status = status;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtNum { get; private set; }

    public int DataTypes { get; private set; }

    public int Status { get; private set; }
}
