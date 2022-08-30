namespace EmrCloudApi.Tenant.Messages.Reception;

public class UpdateReceptionStaticCellMessage
{
    public UpdateReceptionStaticCellMessage(long raiinNo, string cellName, string cellValue)
    {
        RaiinNo = raiinNo;
        CellName = cellName;
        CellValue = cellValue;
    }

    public long RaiinNo { get; private set; }
    public string CellName { get; private set; }
    public string CellValue { get; private set; }
}
