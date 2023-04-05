namespace Domain.Models.Todo;

public class TodoInfModel
{
    public TodoInfModel(int todoNo, int todoEdaNo, long ptId, int sinDate, long raiinNo, int todoKbnNo, int todoGrpNo, int tanto, int term, string cmt1, string cmt2, int isDone, int isDeleted)
    {
        TodoNo = todoNo;
        TodoEdaNo = todoEdaNo;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        TodoKbnNo = todoKbnNo;
        TodoGrpNo = todoGrpNo;
        Tanto = tanto;
        Term = term;
        Cmt1 = cmt1;
        Cmt2 = cmt2;
        IsDone = isDone;
        IsDeleted = isDeleted;
    }
    public int TodoNo { get; private set; }

    public int TodoEdaNo { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int TodoKbnNo { get; private set; }

    public int TodoGrpNo { get; private set; }

    public int Tanto { get; private set; }

    public int Term { get; private set; }

    public string Cmt1 { get; private set; }

    public string Cmt2 { get; private set; }

    public int IsDone { get; private set; }

    public int IsDeleted { get; private set; }
}