namespace Domain.Models.KarteInfs;

public class KarteImgInfModel
{
    public KarteImgInfModel(long id, int hpId, long ptId, long raiinNo, string fileName)
    {
        Id = id;
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public string FileName { get; private set; }

}
