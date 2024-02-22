using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetList;

public class GetKaMstListInputData : IInputData<GetKaMstListOutputData>
{
    public GetKaMstListInputData(int hpId, int isDeleted)
    {
        HpId = hpId;
        IsDeleted = isDeleted;
    }

    public int HpId { get; private set; }
    public int IsDeleted { get; private set; }
}
