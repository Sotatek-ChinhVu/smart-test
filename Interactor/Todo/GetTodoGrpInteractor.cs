using Domain.Models.Todo;
using UseCase.Todo;
using UseCase.Todo.GetTodoGrp;

namespace Interactor.Todo;

public class GetTodoGrpInteractor : IGetTodoGrpInputPort
{
    private readonly ITodoGrpMstRepository _todoGrpMstRepository;
    public GetTodoGrpInteractor(ITodoGrpMstRepository todoGrpMstRepository)
    {
        _todoGrpMstRepository = todoGrpMstRepository;
    }

    public GetTodoGrpOutputData Handle(GetTodoGrpInputData input)
    {
        try
        {
            if (input.HpId <= 0)
            {
                return new GetTodoGrpOutputData(GetTodoGrpStatus.InvalidHpId, new());
            }

            var todoGrps = _todoGrpMstRepository.GetTodoGrpMsts(input.HpId);

            return new GetTodoGrpOutputData(GetTodoGrpStatus.Success, todoGrps.Select(t => new TodoGrpMstDto(t.TodoGrpNo, t.TodoGrpName, t.GrpColor, t.SortNo, t.IsDeleted)).ToList());
        }
        finally
        {
            _todoGrpMstRepository.ReleaseResource();
        }
    }
}
