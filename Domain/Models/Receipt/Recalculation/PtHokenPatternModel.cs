namespace Domain.Models.Receipt.Recalculation;

public class PtHokenPatternModel
{
    public PtHokenPatternModel(long ptId, int hokenPid, long seqNo, int hokenKbn, int hokenSbtCd, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string hokenMemo, int startDate, int endDate)
    {
        PtId = ptId;
        HokenPid = hokenPid;
        SeqNo = seqNo;
        HokenKbn = hokenKbn;
        HokenSbtCd = hokenSbtCd;
        HokenId = hokenId;
        Kohi1Id = kohi1Id;
        Kohi2Id = kohi2Id;
        Kohi3Id = kohi3Id;
        Kohi4Id = kohi4Id;
        HokenMemo = hokenMemo;
        StartDate = startDate;
        EndDate = endDate;
    }

    public PtHokenPatternModel()
    {
        HokenMemo = string.Empty;
    }

    public long PtId { get; set; }

    public int HokenPid { get; set; }

    public long SeqNo { get; set; }

    public int HokenKbn { get; set; }

    public int HokenSbtCd { get; set; }

    public int HokenId { get; set; }

    public int Kohi1Id { get; set; }

    public int Kohi2Id { get; set; }

    public int Kohi3Id { get; set; }

    public int Kohi4Id { get; set; }

    public string HokenMemo { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }
}
