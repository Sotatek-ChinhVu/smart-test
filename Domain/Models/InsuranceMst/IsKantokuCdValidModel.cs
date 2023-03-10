namespace Domain.Models.InsuranceMst;

public class IsKantokuCdValidModel
{
    public IsKantokuCdValidModel(long ptId, int hokenId)
    {
        PtId = ptId;
        HokenId = hokenId;
    }

    public IsKantokuCdValidModel(long ptId, int hokenId, bool isKantokuCdValid)
    {
        PtId = ptId;
        HokenId = hokenId;
        IsKantokuCdValid = isKantokuCdValid;
    }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }

    public bool IsKantokuCdValid { get; private set; }
}
