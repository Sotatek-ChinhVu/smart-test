using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetDocCategoryDetail;

public class GetDocCategoryDetailInputData : IInputData<GetDocCategoryDetailOutputData>
{
    public GetDocCategoryDetailInputData(int hpId, int categoryCd)
    {
        HpId = hpId;
        CategoryCd = categoryCd;
    }

    public int HpId { get; private set; }

    public int CategoryCd { get; private set; }
}
