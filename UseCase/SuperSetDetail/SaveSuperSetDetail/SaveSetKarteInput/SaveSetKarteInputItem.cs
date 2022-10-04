namespace UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;

public class SaveSetKarteInputItem
{
    public SaveSetKarteInputItem()
    {
        HpId = 0;
        SetCd = 0;
        RichText = string.Empty;
    }

    public SaveSetKarteInputItem(int hpId, int setCd, string richText)
    {
        HpId = hpId;
        SetCd = setCd;
        RichText = richText;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public string RichText { get; private set; }
}
