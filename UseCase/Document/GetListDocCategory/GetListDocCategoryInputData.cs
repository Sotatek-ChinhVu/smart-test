using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocCategory;
public class GetListDocCategoryInputData : IInputData<GetListDocCategoryOutputData>
{
    public GetListDocCategoryInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
