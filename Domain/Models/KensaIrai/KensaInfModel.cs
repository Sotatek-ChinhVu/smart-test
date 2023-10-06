namespace Domain.Models.KensaIrai;

public class KensaInfModel
{
    public KensaInfModel(long ptId, int iraiDate, long raiinNo, long iraiCd, int inoutKbn, int status, int tosekiKbn, int sikyuKbn, int resultCheck, string centerCd, string nyubi, string yoketu, string bilirubin, bool isDeleted, int createId)
    {
        PtId = ptId;
        IraiDate = iraiDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        InoutKbn = inoutKbn;
        Status = status;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        ResultCheck = resultCheck;
        CenterCd = centerCd;
        Nyubi = nyubi;
        Yoketu = yoketu;
        Bilirubin = bilirubin;
        IsDeleted = isDeleted;
        CreateId = createId;
        PtName = string.Empty;
        KensaCenterName = string.Empty;
        KensaInfDetailModelList = new();
    }

    public KensaInfModel(long ptId, long raiinNo, string centerCd, int primaryKbn, long iraiCd)
    {
        PtId = ptId;
        RaiinNo = raiinNo;
        CenterCd = centerCd;
        PrimaryKbn = primaryKbn;
        IraiCd = iraiCd;
        Nyubi = string.Empty;
        Yoketu = string.Empty;
        Bilirubin = string.Empty;
        PtName = string.Empty;
        KensaCenterName = string.Empty;
        KensaInfDetailModelList = new();
    }

    public KensaInfModel(long ptId, long raiinNo, long iraiCd, List<KensaInfDetailModel> kensaInfDetailModelList)
    {
        PtId = ptId;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        CenterCd = string.Empty;
        Nyubi = string.Empty;
        Yoketu = string.Empty;
        Bilirubin = string.Empty;
        PtName = string.Empty;
        KensaCenterName = string.Empty;
        KensaInfDetailModelList = kensaInfDetailModelList;
    }

    public KensaInfModel(long ptId, int iraiDate, long raiinNo, long iraiCd, int inoutKbn, int status, int tosekiKbn, int sikyuKbn, int resultCheck, string centerCd, string nyubi, string yoketu, string bilirubin, bool isDeleted, int createId, int primaryKbn, long ptNum, string ptName, string kensaCenterName, DateTime updateDate, DateTime createDate, List<KensaInfDetailModel> kensaInfDetailModelList)
    {
        PtId = ptId;
        IraiDate = iraiDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        InoutKbn = inoutKbn;
        Status = status;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        ResultCheck = resultCheck;
        CenterCd = centerCd;
        Nyubi = nyubi;
        Yoketu = yoketu;
        Bilirubin = bilirubin;
        IsDeleted = isDeleted;
        CreateId = createId;
        PrimaryKbn = primaryKbn;
        PtNum = ptNum;
        PtName = ptName;
        KensaCenterName = kensaCenterName;
        UpdateDate = updateDate;
        CreateDate = createDate;
        KensaInfDetailModelList = kensaInfDetailModelList;
    }

    public long PtId { get; private set; }

    public int IraiDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public int InoutKbn { get; private set; }

    public int Status { get; private set; }

    public int TosekiKbn { get; private set; }

    public int SikyuKbn { get; private set; }

    public int ResultCheck { get; private set; }

    public string CenterCd { get; private set; }

    public string Nyubi { get; private set; }

    public string Yoketu { get; private set; }

    public string Bilirubin { get; private set; }

    public bool IsDeleted { get; private set; }

    public int CreateId { get; private set; }

    public int PrimaryKbn { get; private set; }

    public long PtNum { get; private set; }

    public string PtName { get; private set; }

    public string KensaCenterName { get; private set; }

    public List<KensaInfDetailModel> KensaInfDetailModelList { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public DateTime CreateDate { get; private set; }

    public long KeyNo { get; set; } = 0;
    public bool IsAddNew { get; set; } = false;
    public bool IsUpdate { get; set; } = false;

    public KensaInfModel UpdateKensaInfModel(int createId, int tosekiKbn, int sikyuKbn)
    {
        CreateId = createId;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        return this;
    }

    public KensaInfModel ChangeIraiCd(long iraiCd)
    {
        IraiCd = iraiCd;
        return this;
    }
}
