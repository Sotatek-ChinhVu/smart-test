namespace Domain.Models.KensaIrai;

public class KensaInfMessageModel
{
    public KensaInfMessageModel(long ptId, long iraiCd, long ptNum, string ptName, List<KensaInfDetailMessageModel> kensaInfDetailList)
    {
        PtId = ptId;
        IraiCd = iraiCd;
        PtNum = ptNum;
        PtName = ptName;
        KensaInfDetailList = kensaInfDetailList;
    }
    public long PtId { get; private set; }

    public long IraiCd { get; private set; }

    public long PtNum { get; private set; }

    public string PtName { get; private set; }

    public List<KensaInfDetailMessageModel> KensaInfDetailList { get; private set; }
}

public class KensaInfDetailMessageModel
{
    public KensaInfDetailMessageModel(string kensaItemCd, string kensaMstName)
    {
        KensaItemCd = kensaItemCd;
        KensaMstName = kensaMstName;
    }

    public string KensaItemCd { get; private set; }

    public string KensaMstName { get; private set; }
}
