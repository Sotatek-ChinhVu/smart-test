namespace UseCase.Todo.UpsertTodoGrpMst;

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
    InvalidExistedTodoGrpNoIsDeleted,
}
