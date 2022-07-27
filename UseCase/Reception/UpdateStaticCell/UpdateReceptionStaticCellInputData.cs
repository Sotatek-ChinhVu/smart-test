using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateStaticCell;

public class UpdateReceptionStaticCellInputData : IInputData<UpdateReceptionStaticCellOutputData>
{
    public int HpId { get; set; }
    public int SinDate { get; set; }
    public long RaiinNo { get; set; }
    public long PtId { get; set; }
    public string CellName { get; set; } = string.Empty;
    public string CellValue { get; set; } = string.Empty;
}
