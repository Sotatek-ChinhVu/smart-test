namespace Reporting.Accounting.Model;

public class CoAccountDueListModel
{
    public CoAccountDueListModel(int sinDate, int nyukinKbn, long raiinNo, long oyaRaiinNo)
    {
        SinDate = sinDate;
        NyukinKbn = nyukinKbn;
        RaiinNo = raiinNo;
        OyaRaiinNo = oyaRaiinNo;
    }

    public CoAccountDueListModel()
    {
    }

    public int SinDate { get; private set; }

    public int NyukinKbn { get; private set; }

    public long RaiinNo { get; private set; }

    public long OyaRaiinNo { get; private set; }

    public int Month
    {
        get => GetMonth(SinDate);
    }

    private int GetMonth(int date)
    {
        return (date / 100);
    }
}
