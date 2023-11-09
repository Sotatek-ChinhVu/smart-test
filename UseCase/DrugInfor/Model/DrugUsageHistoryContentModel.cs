namespace UseCase.DrugInfor.Model;

public class DrugUsageHistoryContentModel
{
    public DrugUsageHistoryContentModel(string itemName, string quantity, string unitName, int odrKouiKbn, List<ActionGraph> listActionGraph)
    {
        ItemName = itemName;
        Quantity = quantity;
        UnitName = unitName;
        OdrKouiKbn = odrKouiKbn;
        ListActionGraph = listActionGraph;
    }

    public string ItemName { get; private set; }

    public string Quantity { get; private set; }

    public string UnitName { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public List<ActionGraph> ListActionGraph { get; private set; }

    public string Tooltip => string.Format($"{ItemName}    {Quantity} {UnitName}");
}
