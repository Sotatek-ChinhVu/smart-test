namespace Domain.Models.DrugInfor;

public class SinrekiFilterMstKouiModel
{
    public SinrekiFilterMstKouiModel(int grpCd, long seqNo, int kouiKbnId, bool isChecked)
    {
        GrpCd = grpCd;
        SeqNo = seqNo;
        KouiKbnId = kouiKbnId;
        IsChecked = isChecked;
    }

    public int GrpCd { get; private set; }

    public long SeqNo { get; private set; }

    public int KouiKbnId { get; private set; }

    public bool IsChecked { get; private set; }
}
