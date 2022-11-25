using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocCategoryMst;
public class GetListDocCategoryMstInputData : IInputData<GetListDocCategoryMstOutputData>
{
    public GetListDocCategoryMstInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
