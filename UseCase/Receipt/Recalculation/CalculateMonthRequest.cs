namespace UseCase.Receipt.Recalculation;

public class CalculateMonthRequest
{
    public int HpId { get; set; }

    public int SeikyuYm { get; set; }

    public List<long> PtIds { get; set; } = new List<long>();

    public string PreFix { get; set; } = string.Empty;
}
