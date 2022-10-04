namespace Domain.Models.SuperSetDetail;

public class SetKarteInfModel
{
    public SetKarteInfModel()
    {
        HpId = 0;
        SetCd = 0;
        RichText = string.Empty;
    }

    public SetKarteInfModel(int hpId, int setCd, string richText)
    {
        HpId = hpId;
        SetCd = setCd;
        RichText = richText;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public string RichText { get; private set; }
}
