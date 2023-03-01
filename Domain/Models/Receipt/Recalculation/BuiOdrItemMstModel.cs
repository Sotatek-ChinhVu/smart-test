namespace Domain.Models.Receipt.Recalculation;

public class BuiOdrItemMstModel
{
    public BuiOdrItemMstModel(string itemCd)
    {
        ItemCd = itemCd;
    }

    public string ItemCd { get; private set; }
}
