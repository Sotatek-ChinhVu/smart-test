using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.GetReceSeikyModelByPtNum;

public class GetReceSeikyModelByPtNumInputData : IInputData<GetReceSeikyModelByPtNumOutputData>
{
    public GetReceSeikyModelByPtNumInputData(int hpId, int sinDate, int sinYm, long ptNum)
    {
        HpId = hpId;
        SinDate = sinDate;
        SinYm = sinYm;
        PtNum = ptNum;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public int SinYm { get; private set; }

    public long PtNum { get; private set; }
}
