using Domain.Models.Todo;
using UseCase.Todo.GetListTodoKbn;

namespace Interactor.Todo;

public class GetTodoKbnInteractor : IGetTodoKbnInputPort
{
    private readonly ITodoKbnMstRepository _todoKbnMstRepository;
    public GetTodoKbnInteractor(ITodoKbnMstRepository todoKbnMstRepository)
    {
        _todoKbnMstRepository = todoKbnMstRepository;
    }
    public GetTodoKbnOutputData Handle(GetTodoKbnInputData input)
    {
        try
        {
            var todoKbnMsts = _todoKbnMstRepository.GetTodoKbnMsts(input.HpId);
            return new GetTodoKbnOutputData(GetTodoKbnStatus.Success, todoKbnMsts);
        }
        finally
        {
            _todoKbnMstRepository.ReleaseResource();
        }
    }
}
