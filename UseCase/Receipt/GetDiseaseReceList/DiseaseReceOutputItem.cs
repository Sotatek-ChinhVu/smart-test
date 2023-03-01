namespace UseCase.Receipt.GetDiseaseReceList;

public class DiseaseReceOutputItem
{
    public DiseaseReceOutputItem(string byomei, string startDate, string tenkiKbn, string tenkiDate)
    {
        Byomei = byomei;
        StartDate = startDate;
        TenkiKbn = tenkiKbn;
        TenkiDate = tenkiDate;
    }

    public string Byomei { get; private set; }

    public string StartDate { get; private set; }

    public string TenkiKbn { get; private set; }

    public string TenkiDate { get; private set; }
}
