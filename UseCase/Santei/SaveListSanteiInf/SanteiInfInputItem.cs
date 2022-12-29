namespace UseCase.Santei.SaveListSanteiInf;

public class SanteiInfInputItem
{
    public SanteiInfInputItem(long id, string itemCd, int alertDays, int alertTerm, bool isDeleted, List<SanteiInfDetailInputItem> listSanteInfDetails)
    {
        Id = id;
        ItemCd = itemCd;
        AlertDays = alertDays;
        AlertTerm = alertTerm;
        IsDeleted = isDeleted;
        ListSanteInfDetails = listSanteInfDetails;
    }

    public long Id { get; private set; }

    public string ItemCd { get; private set; }

    public int AlertDays { get; private set; }

    public int AlertTerm { get; private set; }

    public bool IsDeleted { get; private set; }

    public List<SanteiInfDetailInputItem> ListSanteInfDetails { get; private set; }
}
