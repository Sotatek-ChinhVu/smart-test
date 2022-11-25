using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocCategory;
public class GetListDocCategoryInputData : IInputData<GetListDocCategoryOutputData>
{
    public GetListDocCategoryInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
