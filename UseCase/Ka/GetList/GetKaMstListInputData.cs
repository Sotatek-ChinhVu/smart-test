using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetList;

public class GetKaMstListInputData : IInputData<GetKaMstListOutputData>
{
    public GetKaMstListInputData(int isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public int IsDeleted { get; private set; }
}
