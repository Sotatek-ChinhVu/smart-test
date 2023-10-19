namespace Domain.Models.PatientInfor;

public class VisitTimesManagementModel
{
    public VisitTimesManagementModel(long ptId, int sinDate, int hokenPid, int kohiId, int seqNo, string sortKey, bool isDeleted)
    {
        SinDate = sinDate;
        HokenPid = hokenPid;
        SeqNo = seqNo;
        IsDeleted = isDeleted;
        PtId = ptId;
        KohiId = kohiId;
        SortKey = sortKey;
    }

    public VisitTimesManagementModel(long ptId, int sinDate, int hokenPid, int kohiId, int seqNo, string sortKey)
    {
        PtId = ptId;
        SinDate = sinDate;
        HokenPid = hokenPid;
        KohiId = kohiId;
        SeqNo = seqNo;
        SortKey = sortKey;
        IsDeleted = false;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int HokenPid { get; private set; }

    public int KohiId { get; private set; }

    public int SeqNo { get; private set; }

    public bool IsDeleted { get; private set; }

    public string SortKey { get; private set; }

    public bool IsOutHospital => HokenPid == 0;
}
