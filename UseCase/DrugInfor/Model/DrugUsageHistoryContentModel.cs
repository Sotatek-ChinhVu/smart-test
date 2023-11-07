namespace UseCase.DrugInfor.Model;

public class DrugUsageHistoryContentModel
{
    public DrugUsageHistoryContentModel(string itemName, string quantity, string unitName, int kouiKbn, int fromDate, int toDate, List<ActionGraph> listActionGraph)
    {
        ItemName = itemName;
        Quantity = quantity;
        UnitName = unitName;
        KouiKbn = kouiKbn;
        FromDate = fromDate;
        ToDate = toDate;
        ListActionGraph = listActionGraph;
    }

    public string ItemName { get; private set; }

    public string Quantity { get; private set; }

    public string UnitName { get; private set; }

    public int KouiKbn { get; private set; }

    public int FromDate { get; private set; }

    public int ToDate { get; private set; }

    public List<ActionGraph> ListActionGraph { get; private set; }

    public string Tooltip => string.Format($"{ItemName}    {Quantity} {UnitName}");
}
