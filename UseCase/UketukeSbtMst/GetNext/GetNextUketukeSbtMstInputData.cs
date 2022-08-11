using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetNext;

public class GetNextUketukeSbtMstInputData : IInputData<GetNextUketukeSbtMstOutputData>
{
    public GetNextUketukeSbtMstInputData(int sinDate, int currentKbnId)
    {
        SinDate = sinDate;
        CurrentKbnId = currentKbnId;
    }

    public int SinDate { get; private set; }
    public int CurrentKbnId { get; private set; }
}
