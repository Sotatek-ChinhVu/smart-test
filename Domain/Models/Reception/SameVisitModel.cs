namespace Domain.Models.Reception;

public class SameVisitModel
{
    public SameVisitModel(int sinDate, long ptId, long raiinNo, long oyaRaiinNo, string sameVisit)
    {
        SinDate = sinDate;
        PtId = ptId;
        RaiinNo = raiinNo;
        OyaRaiinNo = oyaRaiinNo;
        SameVisit = sameVisit;
    }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public long OyaRaiinNo { get; private set; }

    public string SameVisit { get; private set; }
}
