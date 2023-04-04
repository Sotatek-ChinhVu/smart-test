namespace UseCase.Todo.TodoGrpMst;

public enum UpsertTodoGrpMstStatus : byte
{
    Success = 1,
    Failed,
    InputNoData, 
    InvalidTodoGrpNo,
    InvalidTodoGrpName,
    InvalidGrpColor,
    InvalidSortNo,
    InvalidIsDeleted,
    InvalidTodoGrpMst,
    InvalidExistedTodoGrpNo,
}
