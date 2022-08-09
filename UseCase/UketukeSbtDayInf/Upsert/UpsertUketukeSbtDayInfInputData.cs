using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtDayInf.Upsert;

public class UpsertUketukeSbtDayInfInputData : IInputData<UpsertUketukeSbtDayInfOutputData>
{
    public UpsertUketukeSbtDayInfInputData(int sinDate, int uketukeSbt, int seqNo)
    {
        SinDate = sinDate;
        UketukeSbt = uketukeSbt;
        SeqNo = seqNo;
    }

    public int SinDate { get; private set; }
    public int UketukeSbt { get; private set; }
    public int SeqNo { get; private set; }
}
