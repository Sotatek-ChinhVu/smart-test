namespace UseCase.Santei.GetListSanteiInf;

public class SanteiInfDetailOutputItem
{
    public SanteiInfDetailOutputItem(long ptId, string itemCd, int seqNo, int endDate, int kisanSbt, int kisanDate, string byomei, string hosokuComment, string comment)
    {
        PtId = ptId;
        ItemCd = itemCd;
        SeqNo = seqNo;
        EndDate = endDate;
        KisanSbt = kisanSbt;
        KisanDate = kisanDate;
        Byomei = byomei;
        HosokuComment = hosokuComment;
        Comment = comment;
    }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public int SeqNo { get; private set; }

    public int EndDate { get; private set; }

    public int KisanSbt { get; private set; }

    public int KisanDate { get; private set; }

    public string Byomei { get; private set; }

    public string HosokuComment { get; private set; }

    public string Comment { get; private set; }
}
