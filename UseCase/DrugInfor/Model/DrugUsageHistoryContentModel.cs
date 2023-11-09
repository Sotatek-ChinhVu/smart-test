namespace UseCase.DrugInfor.Model;

public class DrugUsageHistoryContentModel
{
    public DrugUsageHistoryContentModel(string itemCd, string itemName, string quantity, string unitName, int odrKouiKbn, double suryo, int unitSbt, List<ActionGraph> listActionGraph)
    {
        ItemCd = itemCd;
        ItemName = itemName;
        Quantity = quantity;
        UnitName = unitName;
        OdrKouiKbn = odrKouiKbn;
        Suryo = suryo;
        UnitSbt = unitSbt;
        ListActionGraph = listActionGraph;
    }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public string Quantity { get; private set; }

    public string UnitName { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public double Suryo { get; private set; }

    public int UnitSbt { get; private set; }

    public List<ActionGraph> ListActionGraph { get; private set; }

    public string Tooltip => string.Format($"{ItemName}    {Quantity} {UnitName}");
}
