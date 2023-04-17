using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoInfFinder;

public class GetTodoInfFinderInputData : IInputData<GetTodoInfFinderOutputData>
{
    public GetTodoInfFinderInputData(int hpId, int todoNo, int todoEdaNo, bool incDone)
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
