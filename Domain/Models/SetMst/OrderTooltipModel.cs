namespace Domain.Models.SetMst;

public class OrderTooltipModel
{
    public OrderTooltipModel(string itemName, double suryo, string unitName)
    {
        ItemName = itemName;
        Suryo = suryo;
        UnitName = unitName;
    }

    public string ItemName { get; private set; }
    public double Suryo { get; private set; }
    public string UnitName { get; private set; }
}
