namespace Domain.Models.KarteInfs;

public class KarteImgInfModel
{
    public KarteImgInfModel(int hpId, long ptId, long raiinNo, string fileName)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        FileName = fileName;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public string FileName { get; private set; }

}
