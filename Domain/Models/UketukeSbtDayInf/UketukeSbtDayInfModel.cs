namespace Domain.Models.UketukeSbtDayInf;

public class UketukeSbtDayInfModel
{
    public UketukeSbtDayInfModel(int sinDate, int seqNo, int uketukeSbt)
    {
        SinDate = sinDate;
        SeqNo = seqNo;
        UketukeSbt = uketukeSbt;
    }

    public int SinDate { get; private set; }
    public int SeqNo { get; private set; }
    public int UketukeSbt { get; private set; }
}
