namespace Domain.Models.RaiinFilterMst;

public class RaiinFilterSortModel
{
    public RaiinFilterSortModel(long id, int filterId, long seqNo,
        int priority, string columnName, int kbnCd, int sortKbn)
    {
        Id = id;
        FilterId = filterId;
        SeqNo = seqNo;
        Priority = priority;
        ColumnName = columnName;
        KbnCd = kbnCd;
        SortKbn = sortKbn;
    }

    public long Id { get; private set; }
    public int FilterId { get; private set; }
    public long SeqNo { get; private set; }
    public int Priority { get; private set; }
    public string ColumnName { get; private set; }
    public int KbnCd { get; private set; }
    public int SortKbn { get; private set; }
}
