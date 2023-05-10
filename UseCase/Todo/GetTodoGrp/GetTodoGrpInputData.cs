using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoGrp;

public class GetTodoGrpInputData : IInputData<GetTodoGrpOutputData>
{
    public GetTodoGrpInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
