namespace UseCase.User.UpsertUserConfList;

public enum UpsertUserConfListStatus
{
    Successed = 1,
    InvalidHpId = 2,
    InvalidUserId = 3,
    InvalidUserConfs = 4,
    DuplicateUserConf = 5,
    Failed = 6
}
