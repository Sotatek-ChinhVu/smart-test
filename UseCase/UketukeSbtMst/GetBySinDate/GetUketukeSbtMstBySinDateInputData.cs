using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetBySinDate;

public class GetUketukeSbtMstBySinDateInputData : IInputData<GetUketukeSbtMstBySinDateOutputData>
{
    public GetUketukeSbtMstBySinDateInputData(int sinDate)
    {
        SinDate = sinDate;
    }

    public int SinDate { get; private set; }
}
