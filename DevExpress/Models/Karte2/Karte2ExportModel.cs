namespace DevExpress.Models.Karte2;

public class Karte2ExportModel
{
    public Karte2ExportModel(int hpId, long ptId, long raiinNo, int sinDate, string kanaName, string name, string sex, string birthday, string currentTime, string startDate, string endDate, string fileName, List<RichTextKarteOrder> richTextKarte2Models)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        KanaName = kanaName;
        Name = name;
        Sex = sex;
        Birthday = birthday;
        CurrentTime = currentTime;
        StartDate = startDate;
        EndDate = endDate;
        FileName = fileName;
        RichTextKarte2Models = richTextKarte2Models;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public string Sex { get; private set; } 

    public string Birthday { get; private set; }

    public string CurrentTime { get; private set; } 

    public string StartDate { get; private set; } 

    public string EndDate { get; private set; } 

    public string FileName { get; private set; } 

    public List<RichTextKarteOrder> RichTextKarte2Models { get; private set; }
}
