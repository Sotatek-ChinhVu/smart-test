using UseCase.Core.Sync.Core;

namespace UseCase.Document.SaveListDocCategory;

public class SaveListDocCategoryInputData : IInputData<SaveListDocCategoryOutputData>
{
    public SaveListDocCategoryInputData(int hpId, int userId, List<SaveListDocCategoryInputItem> listDocCategoryItems)
    {
        HpId = hpId;
        UserId = userId;
        ListDocCategoryItems = listDocCategoryItems;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public List<SaveListDocCategoryInputItem> ListDocCategoryItems { get; private set; }
}
