namespace Domain.Models.Yousiki;

public class ForeignKFileModel
{
    public ForeignKFileModel(int sinDate, long ptNum, string kanaName, int sex, int birthday, bool isTester)
    {
        SinDate = sinDate;
        PtNum = ptNum;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        IsTester = isTester;
    }

    public int SinDate { get; private set; }

    public long PtNum { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public bool IsTester { get; private set; }
}
