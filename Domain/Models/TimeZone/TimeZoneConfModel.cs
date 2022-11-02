namespace Domain.Models.TimeZone;

public class TimeZoneConfModel
{
    public TimeZoneConfModel(int hpId, int youbiKbn, long seqNo, int startTime, int endTime, int timeKbn)
    {
        HpId = hpId;
        YoubiKbn = youbiKbn;
        SeqNo = seqNo;
        StartTime = startTime;
        EndTime = endTime;
        TimeKbn = timeKbn;
    }
    
    public int HpId { get; private set; }
    
    public int YoubiKbn { get; private set; }
    
    public long SeqNo { get; private set; }
    
    public int StartTime { get; private set; }
    
    public int EndTime { get; private set; }
    
    public int TimeKbn { get; private set; }
}
