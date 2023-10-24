namespace Domain.Models.KensaIrai;

public class KensaIraiLogModel
{
    public KensaIraiLogModel(int iraiDate, string centerCd, string centerName, int fromDate, int toDate, string iraiFile, byte[] iraiList, DateTime createDate)
    {
        IraiDate = iraiDate;
        CenterCd = centerCd;
        CenterName = centerName;
        FromDate = fromDate;
        ToDate = toDate;
        IraiFile = iraiFile;
        IraiList = iraiList;
        CreateDate = createDate;
    }

    public int IraiDate { get; private set; }

    public string CenterCd { get; private set; }

    public string CenterName { get; private set; }

    public int FromDate { get; private set; }

    public int ToDate { get; private set; }

    public string IraiFile { get; private set; }

    public byte[] IraiList { get; private set; }

    public DateTime CreateDate { get; private set; }
}
