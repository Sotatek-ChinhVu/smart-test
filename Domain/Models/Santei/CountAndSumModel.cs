namespace Domain.Models.Santei;

public class CountAndSumModel
{
    public CountAndSumModel(string itemCd, int count, double sum)
    {
        ItemCd = itemCd;
        Count = count;
        Sum = sum;
    }

    public string ItemCd { get; private set; }

    public int Count { get; private set; }

    public double Sum { get; private set; }
}
