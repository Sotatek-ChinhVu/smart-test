namespace Domain.Models.Lock;

public class ResponseLockModel
{
    public ResponseLockModel(int sinDate, long ptId, long raiinNo, int status)
    {
        SinDate = sinDate;
        PtId = ptId;
        RaiinNo = raiinNo;
        Status = status;
    }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int Status { get; private set; }
}
