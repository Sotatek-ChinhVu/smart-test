namespace EmrReporting.DataSources;

public class Karte2
{
    public Karte2Header Header { get; set; }
    // Table
    public List<MedicalInfo> MedicalInfos { get; set; }
}

public class Karte2Header
{
    public string PrintDate { get; set; }
    public long PtNum { get; set; }
    public string KanjiName { get; set; }
    public string KanaName { get; set; }
    public string BirthDay { get; set; }
    public string Sex { get; set; }
    public string BirthDayAndSex => $"{BirthDay} ({Sex})";
    public string PrintStartDate { get; set; }
    public string PrintEndDate { get; set; }
}

public class MedicalInfo
{
    public string SinDate { get; set; }
    public string GenernalInfo { get; set; }
    public List<KarteFrame> KarteFrames { get; set; }
    public List<OrderLine> OrderLines { get; set; }
}

public class KarteFrame
{
    public string EmfFilePath { get; set; }
    public int Top { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Caption { get; set; }
}

public class OrderLine
{
    public string GroupName { get; set; }
    public string Star { get; set; }
    public string DeletedLine { get; set; }
}
