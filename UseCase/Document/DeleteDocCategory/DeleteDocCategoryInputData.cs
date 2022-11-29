using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocCategory;

public class DeleteDocCategoryInputData : IInputData<DeleteDocCategoryOutputData>
{
    public DeleteDocCategoryInputData(int hpId, int userId, int categoryCd)
    {
        HpId = hpId;
        UserId = userId;
        CategoryCd = categoryCd;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int CategoryCd { get; private set; }
}
