namespace UseCase.Schema.SaveImageTodayOrder;

public enum SaveImageTodayOrderStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidOldImage = 3,
    InvalidFileImage = 4,
    InvalidPtId = 5,
}