namespace UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;

public class SaveSetKarteInputItem
{
    public SaveSetKarteInputItem()
    {
        HpId = 0;
        SetCd = 0;
        RichText = string.Empty;
        Text = string.Empty;
    }

    public SaveSetKarteInputItem(int hpId, int setCd, string richText, string text)
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
