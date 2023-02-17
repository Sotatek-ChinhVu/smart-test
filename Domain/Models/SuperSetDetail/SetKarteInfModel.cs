namespace Domain.Models.SuperSetDetail;

public class SetKarteInfModel
{
    public SetKarteInfModel()
    {
        HpId = 0;
        SetCd = 0;
        RichText = string.Empty;
        Text = string.Empty;
    }

    public SetKarteInfModel(int hpId, int setCd, string richText, string text)
    {
        HpId = hpId;
        SetCd = setCd;
        RichText = richText;
        Text = text;
    }

    public int HpId { get; private set; }

    public int SetCd { get; private set; }

    public string RichText { get; private set; }

    public string Text { get; private set; }
}
