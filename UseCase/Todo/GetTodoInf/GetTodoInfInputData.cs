using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoInf;

public class GetTodoInfInputData : IInputData<GetTodoInfOutputData>
{
    public GetTodoInfInputData(int hpId, int todoNo, int todoEdaNo, int ptId, int isDone)
    {
        HpId = hpId;
        TodoNo = todoNo;
        TodoEdaNo = todoEdaNo;
        PtId = ptId;
        IsDone = isDone;
    }

    public int HpId { get; private set;}

    public int TodoNo { get; private set;}

    public int TodoEdaNo { get; private set;}

    public int PtId { get; private set;}

    public int IsDone { get; private set;}
}
