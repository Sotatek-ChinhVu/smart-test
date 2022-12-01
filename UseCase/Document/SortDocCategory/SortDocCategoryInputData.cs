using UseCase.Core.Sync.Core;

namespace UseCase.Document.SortDocCategory;

public class SortDocCategoryInputData : IInputData<SortDocCategoryOutputData>
{
    public SortDocCategoryInputData(int hpId, int userId, int moveInCd, int moveOutCd)
    {
        HpId = hpId;
        UserId = userId;
        MoveInCd = moveInCd;
        MoveOutCd = moveOutCd;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int MoveInCd { get; private set; }

    public int MoveOutCd { get; private set; }
}
