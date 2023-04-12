using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoInf;

public class GetTodoInfInputData : IInputData<GetTodoInfOutputData>
{
    public GetTodoInfInputData(int hpId, int todoNo, int todoEdaNo, bool incDone)
    {
        HpId = hpId;
        TodoNo = todoNo;
        TodoEdaNo = todoEdaNo;
        IncDone = incDone;
    }

    public int HpId { get; private set;}

    public int TodoNo { get; private set;}

    public int TodoEdaNo { get; private set;}

    public bool IncDone { get; private set;}
}
