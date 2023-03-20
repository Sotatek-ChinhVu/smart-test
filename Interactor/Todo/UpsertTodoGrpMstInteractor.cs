using Domain.Models.Todo;
using UseCase.Todo.TodoGrpMst;
using static Helper.Constants.TodoGrpMstConstant;
using UseCase.Todo.UpsertTodoGrpMst;
using UseCase.UketukeSbtMst.Upsert;
using UseCase.ApprovalInfo.UpdateApprovalInfList;

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
            if (input.ToList() == null || input.ToList().Count == 0)
            {
                return new UpsertTodoGrpMstOutputData(TodoGrpMstConstant.InputNoData);
            }

            var checkInputTodoGrpNo = input.ToList().Where(x => x.TodoGrpNo > 0).Select(x => x.TodoGrpNo);
            if (checkInputTodoGrpNo.Count() != checkInputTodoGrpNo.Distinct().Count())
            {
                return new UpsertTodoGrpMstOutputData(TodoGrpMstConstant.InvalidTodoGrpMst);
            }

            foreach (var data in input.ToList())
            {
                var status = data.Validation();
                if (status != ValidationStatus.Valid)
                {
                    return new UpsertTodoGrpMstOutputData(ConvertStatus(status));
                }
            }

            if (!_todoGrpMstRepository.CheckExistedTodoGrpNo(input.ToList().Where(x => x.TodoGrpNo > 0).Select(x => x.TodoGrpNo).ToList()))
            {
                return new UpsertTodoGrpMstOutputData(TodoGrpMstConstant.InvalidExistedTodoGrpNo);
            }

            _todoGrpMstRepository.Upsert(input.ToList(), input.UserId, input.HpId);

            return new UpsertTodoGrpMstOutputData(TodoGrpMstConstant.Success);
        }
        finally 
        {
            _todoGrpMstRepository.ReleaseResource();
        }
    }

    private static TodoGrpMstConstant ConvertStatus(ValidationStatus status)
    {
        if (status == ValidationStatus.InvalidTodoGrpNo)
            return TodoGrpMstConstant.InvalidTodoGrpNo;
        if (status == ValidationStatus.InvalidTodoGrpName)
            return TodoGrpMstConstant.InvalidTodoGrpName;
        if (status == ValidationStatus.InvalidGrpColor)
            return TodoGrpMstConstant.InvalidGrpColor;
        if (status == ValidationStatus.InvalidSortNo)
            return TodoGrpMstConstant.InvalidSortNo;
        if (status == ValidationStatus.InvalidIsDeleted)
            return TodoGrpMstConstant.InvalidIsDeleted;
        if (status == ValidationStatus.InvalidTodoGrpMst)
            return TodoGrpMstConstant.InvalidTodoGrpMst;
        if (status == ValidationStatus.InvalidExistedTodoGrpNo)
            return TodoGrpMstConstant.InvalidExistedTodoGrpNo;

        return TodoGrpMstConstant.Success;
    }
}
