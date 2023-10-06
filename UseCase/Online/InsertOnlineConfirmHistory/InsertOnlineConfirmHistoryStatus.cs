namespace UseCase.Online.InsertOnlineConfirmHistory;

public enum InsertOnlineConfirmHistoryStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidConfirmationResult = 3,
    InvalidOnlineConfirmationDate = 4,
}
