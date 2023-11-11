namespace Domain.Models.KensaIrai;

public class KensaInfDetailModel
{
    public KensaInfDetailModel(long ptId, int iraiDate, long raiinNo, long iraiCd, long seqNo, string kensaItemCd, string resultVal, string resultType, string abnormalKbn, int isDeleted, string cmtCd1, string cmtCd2, KensaMstModel kensaMstModel)
    {
        PtId = ptId;
        IraiDate = iraiDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        SeqNo = seqNo;
        KensaItemCd = kensaItemCd;
        ResultVal = resultVal;
        ResultType = resultType;
        AbnormalKbn = abnormalKbn;
        IsDeleted = isDeleted;
        CmtCd1 = cmtCd1;
        CmtCd2 = cmtCd2;
        KensaMstModel = kensaMstModel;
        CenterCd = KensaMstModel.CenterCd;
        Nyubi = string.Empty;
        Yoketu = string.Empty;
        Bilirubin = string.Empty;
        Type = string.Empty;
    }

    public KensaInfDetailModel(long seqNo, long ptId, long iraiCd)
    {
        IraiCd = iraiCd;
        SeqNo = seqNo;
        PtId = ptId;
        KensaItemCd = string.Empty;
        ResultVal = string.Empty;
        ResultType = string.Empty;
        AbnormalKbn = string.Empty;
        CmtCd1 = string.Empty;
        CmtCd2 = string.Empty;
        KensaMstModel = new();
        CenterCd = KensaMstModel.CenterCd;
        Nyubi = string.Empty;
        Yoketu = string.Empty;
        Bilirubin = string.Empty;
        Type = string.Empty;
    }

    public KensaInfDetailModel(int index, string type, string centerCd, long iraiCd, string nyubi, string yoketu, string bilirubin, string kensaItemCd, string abnormalKbn, string resultVal, string resultType, string cmtCd1, string cmtCd2)
    {
        Index = index;
        Type = type;
        IraiCd = iraiCd;
        KensaItemCd = kensaItemCd;
        ResultVal = resultVal;
        ResultType = resultType;
        AbnormalKbn = abnormalKbn;
        CmtCd1 = cmtCd1;
        CmtCd2 = cmtCd2;
        CenterCd = centerCd;
        Nyubi = nyubi;
        Yoketu = yoketu;
        Bilirubin = bilirubin;
        KensaMstModel = new();
    }

    public string Type { get; private set; }

    public long PtId { get; private set; }

    public int IraiDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public long SeqNo { get; private set; }

    public string KensaItemCd { get; private set; }

    public string ResultVal { get; private set; }

    public string ResultType { get; private set; }

    public string AbnormalKbn { get; private set; }

    public int IsDeleted { get; private set; }

    public string CmtCd1 { get; private set; }

    public string CmtCd2 { get; private set; }

    public string CenterCd { get; private set; }

    public string Nyubi { get; private set; }

    public string Yoketu { get; private set; }

    public string Bilirubin { get; private set; }

    public KensaMstModel KensaMstModel { get; private set; }

    public int Index { get; private set; }

    public long KeyNo { get; set; } = 0;

    public bool IsAddNew { get; set; } = false;

    public KensaInfDetailModel ChangeIsDeleted(int isDeleted)
    {
        IsDeleted = isDeleted;
        return this;
    }

    public KensaInfDetailModel ChangeIraiCd(long iraiCd)
    {
        IraiCd = iraiCd;
        return this;
    }
}
