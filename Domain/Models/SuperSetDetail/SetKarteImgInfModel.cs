namespace Domain.Models.SuperSetDetail;

public class SetKarteImgInfModel
{
    public SetKarteImgInfModel(int hpId, int setCd, long position, string fileName, string oldFileName)
    {
        HpId = hpId;
        SetCd = setCd;
        Position = position;
        FileName = fileName;
        OldFileName = oldFileName;
    }

    public int HpId { get; private set; }

    public int SetCd { get; private set; }

    public long Position { get; private set; }

    public string FileName { get; private set; }

    public string OldFileName { get; private set; }

}
