namespace UseCase.Online.SaveOQConfirmation;

public enum SaveOQConfirmationStatus : byte
{
    ValidateSuccessed = 0,
    Successed = 1,
    Failed = 2,
    InvalidId = 3,
    InvalidPtId = 4,
    InvalidConfirmationResult = 5,
}
