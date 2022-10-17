namespace UseCase.User.UpsertList;

public enum UpsertUserListStatus
{
    Success = 1,
    DuplicateId = 2,
    ExistedId = 3,
    Failed = 4,
    UserListInputNoData,
    UserListUpdateNoSuccess,
    UserListKaIdNoExist,
    UserListIdNoExist,
    UserListInvalidExistedLoginId,
    UserListInvalidExistedUserId,
    InvalidHpId,
    InvalidId,
    InvalidUserId,
    InvalidJobCd,
    InvalidManagerKbn,
    InvalidKaId,
    InvalidKanaName,
    InvalidName,
    InvalidSname,
    InvalidLoginId,
    InvalidLoginPass,
    InvalidMayakuLicenseNo,
    InvalidStartDate,
    InvalidEndDate,
    InvalidSortNo,
    InvalidRenkeiCd1,
    InvalidDrName,
    InvalidIsDeleted

}
