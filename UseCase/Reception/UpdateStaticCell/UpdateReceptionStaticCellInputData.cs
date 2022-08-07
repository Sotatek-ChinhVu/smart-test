using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateStaticCell;

public class UpdateReceptionStaticCellInputData : IInputData<UpdateReceptionStaticCellOutputData>
{
    public UpdateReceptionStaticCellInputData(int hpId, int sinDate,
        long raiinNo, long ptId, string cellName, string cellValue)
    {
        HpId = hpId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
        CellName = cellName;
        CellValue = cellValue;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public long RaiinNo { get; private set; }
    public long PtId { get; private set; }
    public string CellName { get; private set; }
    public string CellValue { get; private set; }
}
