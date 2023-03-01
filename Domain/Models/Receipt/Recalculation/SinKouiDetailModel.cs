namespace Domain.Models.Receipt.Recalculation;

public class SinKouiDetailModel
{
    public SinKouiDetailModel(long ptId, long sinYm, int sinDate, long ptNum, string maxAge, string minAge, string itemCd, string cmtOpt, string itemName, string receName, double suryo, int isNodspRece, string masterSbt)
    {
        PtId = ptId;
        SinYm = sinYm;
        SinDate = sinDate;
        PtNum = ptNum;
        MaxAge = maxAge;
        MinAge = minAge;
        ItemCd = itemCd;
        CmtOpt = cmtOpt;
        ItemName = itemName;
        ReceName = receName;
        Suryo = suryo;
        IsNodspRece = isNodspRece;
        MasterSbt = masterSbt;
    }

    public long PtId { get; private set; }

    public long SinYm { get; private set; }

    public int SinDate { get; private set; }

    public long PtNum { get; private set; }

    public string MaxAge { get; private set; }

    public string MinAge { get; private set; }

    public string ItemCd { get; private set; }

    public string CmtOpt { get; private set; }

    public string ItemName { get; private set; }

    public string ReceName { get; private set; }

    public double Suryo { get; private set; }

    public int IsNodspRece { get; private set; }

    public string MasterSbt { get; private set; }
}
