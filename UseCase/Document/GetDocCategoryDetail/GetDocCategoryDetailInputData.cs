using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetDocCategoryDetail;

public class GetDocCategoryDetailInputData : IInputData<GetDocCategoryDetailOutputData>
{
    public GetDocCategoryDetailInputData(int hpId, long ptId, int categoryCd)
    {
        HpId = hpId;
        PtId = ptId;
        CategoryCd = categoryCd;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int CategoryCd { get; private set; }
}
