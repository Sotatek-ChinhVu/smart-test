using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetBySinDate;

public class GetUketukeSbtMstBySinDateInputData : IInputData<GetUketukeSbtMstBySinDateOutputData>
{
    public GetUketukeSbtMstBySinDateInputData(int hpId, int sinDate)
    {
        HpId = hpId;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
}
