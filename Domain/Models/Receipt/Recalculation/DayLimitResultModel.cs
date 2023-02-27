namespace Domain.Models.Receipt.Recalculation;

public class DayLimitResultModel
{
    public DayLimitResultModel(string yjCd, double usingDay, double limitDay, string itemName, string itemCd)
    {
        YjCd = yjCd;
        UsingDay = usingDay;
        LimitDay = limitDay;
        ItemName = itemName;
        ItemCd = itemCd;
    }

    public string YjCd { get; private set; }

    public double UsingDay { get; private set; }

    public double LimitDay { get; private set; }

    public string ItemName { get; private set; }

    public string ItemCd { get; private set; }
}
