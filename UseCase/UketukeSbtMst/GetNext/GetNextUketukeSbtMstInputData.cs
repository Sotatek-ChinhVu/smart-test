using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtMst.GetNext;

public class GetNextUketukeSbtMstInputData : IInputData<GetNextUketukeSbtMstOutputData>
{
    public GetNextUketukeSbtMstInputData(int hpId, int sinDate, int currentKbnId, int userId)
    {
        HpId = hpId;
        SinDate = sinDate;
        CurrentKbnId = currentKbnId;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public int CurrentKbnId { get; private set; }
    public int UserId { get; private set; }
}
