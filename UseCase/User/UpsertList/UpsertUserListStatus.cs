namespace UseCase.User.UpsertList;

public enum UpsertUserListStatus
{
    Success = 1,
    DuplicateId = 2,
    ExistedId = 3,
    Failed = 4,
    UserListInputNoData,
    UserListUpdateNoSuccess,
    UserListIdNoExist,
    UserListInvalidHpId,
    UserListInvalidId,
    UserListInvalidUserId,
    UserListInvalidJobCd,
    UserListInvalidManagerKbn,
    UserListInvalidkaId,
    UserListInvalidKanaName,
    UserListInvalidName,
    UserListInvalidSname,
    UserListInvalidLoginId,
    UserListInvalidLoginPass,
    UserListInvalidMayakuLicenseNo,
    UserListInvalidStartDate,
    UserListInvalidEndDate,
    UserListInvalidSortNo,
    UserListInvalidRenkeiCd1,
    UserListInvalidDrName,
    UserListInvalidIsDeleted
}
