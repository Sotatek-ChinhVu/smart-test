using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetListTodoKbn;

public class GetTodoKbnInputData : IInputData<GetTodoKbnOutputData>
{
    public GetTodoKbnInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set;}
}
