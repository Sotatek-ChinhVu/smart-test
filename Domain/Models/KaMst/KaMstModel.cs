namespace Domain.Models.KaMst;

public class KaMstModel
{
    public KaMstModel(long id, int kaId, int sortNo, string receKaCd, string kaSname, string kaName, int isDeleted)
    {
        Id = id;
        KaId = kaId;
        SortNo = sortNo;
        ReceKaCd = receKaCd;
        KaSname = kaSname;
        KaName = kaName;
        IsDeleted = isDeleted;
    }

    public long Id { get; private set; }
    public int KaId { get; private set; }
    public int SortNo { get; private set; }
    public string ReceKaCd { get; private set; }
    public string KaSname { get; private set; }
    public string KaName { get; private set; }
    public int IsDeleted { get; private set; }
}
