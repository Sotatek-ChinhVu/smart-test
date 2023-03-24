namespace Domain.Models.Receipt.Recalculation;

public class HasErrorWithSanteiModel
{
    public HasErrorWithSanteiModel(long ptId, string itemCd, int sindate)
    {
        PtId = ptId;
        ItemCd = itemCd;
        Sindate = sindate;
    }

    public HasErrorWithSanteiModel(long ptId, string itemCd, int sindate, bool isHasError)
    {
        PtId = ptId;
        ItemCd = itemCd;
        Sindate = sindate;
        IsHasError = isHasError;
    }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public int Sindate { get; private set; }

    public bool IsHasError { get; private set; }
}
