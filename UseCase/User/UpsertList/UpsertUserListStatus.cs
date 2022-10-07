namespace UseCase.User.UpsertList;

public enum UpsertUserListStatus
{
    Success = 1,
    DuplicateId = 2,
    ExistedId = 3,
    Failed = 4,
}
