namespace Reporting.Accounting.Model;

public class CoAccountDueListModel
{
    public CoAccountDueListModel(int sinDate, long raiinNo, long oyaRaiinNo)
    {
        SinDate = sinDate;
        RaiinNo = raiinNo;
        OyaRaiinNo = oyaRaiinNo;
    }

    public CoAccountDueListModel()
    {
        SinDate = 0;
        RaiinNo = 0;
        OyaRaiinNo = 0;
    }

    public int SinDate { get; private set; }

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
