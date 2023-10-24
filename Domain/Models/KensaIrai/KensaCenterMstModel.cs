namespace Domain.Models.KensaIrai;

public class KensaCenterMstModel
{
    public KensaCenterMstModel()
    {
        CenterCd = string.Empty;
        CenterName = string.Empty;
    }

    public KensaCenterMstModel(string centerCd, string centerName, int primaryKbn)
    {
        CenterCd = centerCd;
        CenterName = centerName;
        PrimaryKbn = primaryKbn;
    }

    public string CenterCd { get; private set; }
    
    public string CenterName { get; private set; }
    
    public int PrimaryKbn { get; private set; }
}
