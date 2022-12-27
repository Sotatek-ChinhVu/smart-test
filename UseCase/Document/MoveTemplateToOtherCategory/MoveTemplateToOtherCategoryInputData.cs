using UseCase.Core.Sync.Core;

namespace UseCase.Document.MoveTemplateToOtherCategory;

public class MoveTemplateToOtherCategoryInputData : IInputData<MoveTemplateToOtherCategoryOutputData>
{
    public MoveTemplateToOtherCategoryInputData(int hpId, int oldCategoryCd, int newCategoryCd, string fileName)
    {
        HpId = hpId;
        OldCategoryCd = oldCategoryCd;
        NewCategoryCd = newCategoryCd;
        FileName = fileName;
    }

    public int HpId { get; private set; }

    public int OldCategoryCd { get; private set; }

    public int NewCategoryCd { get; private set; }

    public string FileName { get; private set; }
}
