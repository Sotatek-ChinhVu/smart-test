using Domain.Models.Todo;
using UseCase.Todo;
using UseCase.Todo.TodoGrpMst;
using UseCase.Todo.UpsertTodoGrpMst;

namespace Interactor.Todo;

public class UpsertTodoGrpMstInteractor : IUpsertTodoGrpMstInputPort
{
    private readonly ITodoGrpMstRepository _todoGrpMstRepository;

    public UpsertTodoGrpMstInteractor(ITodoGrpMstRepository todoGrpMstRepository)
    {
        _todoGrpMstRepository = todoGrpMstRepository;
    }

    public UpsertTodoGrpMstOutputData Handle(UpsertTodoGrpMstInputData input)
    {
        try
        {
            var validateResult = ValidateDataInput(input.TodoGrpMsts);
            if(validateResult != UpsertTodoGrpMstStatus.Success)
            {
                return new UpsertTodoGrpMstOutputData(validateResult);
            }

            if (input.ToList() == null || input.ToList().Count == 0)
            {
                return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.InputNoData);
            }

            if (_todoGrpMstRepository.CheckExistedTodoGrpNo(input.ToList().Where(x => x.TodoGrpNo > 0).Select(x => x.TodoGrpNo).ToList()))
            {
                return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.InvalidExistedTodoGrpNo);
            }

            var checkInputTodoGrpNo = input.ToList().Where(x => x.TodoGrpNo > 0).Select(x => x.TodoGrpNo);
            if (checkInputTodoGrpNo.Count() != checkInputTodoGrpNo.Distinct().Count())
            {
                return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.InvalidTodoGrpMst);
            }

            var todoGrpMstList = ConvertToInsertTodoGrpMstDto(input.TodoGrpMsts);

            _todoGrpMstRepository.Upsert(todoGrpMstList, input.UserId, input.HpId);

            return new UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus.Success);
        }
        finally
        {
            _todoGrpMstRepository.ReleaseResource();
        }
    }

    private List<TodoGrpMstModel> ConvertToInsertTodoGrpMstDto(List<InsertTodoGrpMstDto> insertTodoGrpMstDtos)
    {
        List<TodoGrpMstModel> result = new();
        foreach (var todoGrpMst in insertTodoGrpMstDtos)
        {
            result.Add(new TodoGrpMstModel(
                todoGrpMst.TodoGrpNo,
                todoGrpMst.TodoGrpName,
                todoGrpMst.GrpColor,
                todoGrpMst.SortNo,
                todoGrpMst.IsDeleted));
        }

        return result;
    }

    private UpsertTodoGrpMstStatus ValidateDataInput(List<InsertTodoGrpMstDto> insertTodoGrpMstDtos)
    {
        foreach (var insertTodoGrpMst in insertTodoGrpMstDtos)
        {
            if(insertTodoGrpMst.TodoGrpNo < 0)
            {
                return UpsertTodoGrpMstStatus.InvalidTodoGrpNo;
            } 
            else if (insertTodoGrpMst.TodoGrpName.Length > 20)
            {
                return UpsertTodoGrpMstStatus.InvalidTodoGrpName;
            }
            else if (insertTodoGrpMst.GrpColor.Length > 8)
            {
                return UpsertTodoGrpMstStatus.InvalidGrpColor;
            }
            else if (insertTodoGrpMst.SortNo < 0) 
            {
                return UpsertTodoGrpMstStatus.InvalidSortNo;
            }
            else if (insertTodoGrpMst.IsDeleted < 0)
            {
                return UpsertTodoGrpMstStatus.InvalidIsDeleted;
            }
        }
        return UpsertTodoGrpMstStatus.Success;
    }
}