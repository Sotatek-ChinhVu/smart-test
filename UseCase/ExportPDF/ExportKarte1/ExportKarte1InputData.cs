using UseCase.Core.Sync.Core;

namespace UseCase.ExportPDF.ExportKarte1;

public class ExportKarte1InputData : IInputData<ExportKarte1OutputData>
{
    public ExportKarte1InputData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        HokenPid = hokenPid;
        TenkiByomei = tenkiByomei;
    }

    public int HpId { get; private set; }
    public long PtId { get; private set; }
    public int SinDate { get; private set; }
    public int HokenPid { get; private set; }
    public bool TenkiByomei { get; private set; }
}
