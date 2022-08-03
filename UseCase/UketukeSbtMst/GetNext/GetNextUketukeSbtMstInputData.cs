using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetNext;

public class GetNextUketukeSbtMstInputData : IInputData<GetNextUketukeSbtMstOutputData>
{
    public GetNextUketukeSbtMstInputData(int sinDate, int kbnId)
    {
        SinDate = sinDate;
        KbnId = kbnId;
    }

    public int SinDate { get; private set; }
    public int KbnId { get; private set; }
}
