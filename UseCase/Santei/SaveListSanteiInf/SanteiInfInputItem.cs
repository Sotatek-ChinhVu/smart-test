namespace UseCase.Santei.SaveListSanteiInf;

public class SanteiInfInputItem
{
    public SanteiInfInputItem(long id, long ptId, string itemCd, int alertDays, int alertTerm, bool isDeleted, int sortNo, List<SanteiInfDetailInputItem> listSanteInfDetails)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        AlertDays = alertDays;
        AlertTerm = alertTerm;
        IsDeleted = isDeleted;
        SortNo = sortNo;
        ListSanteInfDetails = listSanteInfDetails;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public int AlertDays { get; private set; }

    public int AlertTerm { get; private set; }

    public bool IsDeleted { get; private set; }

    public int SortNo { get; private set; }

    public List<SanteiInfDetailInputItem> ListSanteInfDetails { get; private set; }
}
