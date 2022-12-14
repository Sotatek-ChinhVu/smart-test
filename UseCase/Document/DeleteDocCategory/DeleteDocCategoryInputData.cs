using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocCategory;

public class DeleteDocCategoryInputData : IInputData<DeleteDocCategoryOutputData>
{
    public DeleteDocCategoryInputData(int hpId, int userId, int categoryCd, long ptId, int moveToCategoryCd)
    {
        HpId = hpId;
        UserId = userId;
        CategoryCd = categoryCd;
        PtId = ptId;
        MoveToCategoryCd = moveToCategoryCd;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int CategoryCd { get; private set; }

    public long PtId { get; private set; }

    public int MoveToCategoryCd { get; private set; }
}
