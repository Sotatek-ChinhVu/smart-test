namespace UseCase.Schema.SaveListImageTodayOrder;

public enum SaveListFileTodayOrderStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidFileImage = 3,
    InvalidPtId = 4,
    InvalidHpId = 5,
    InvalidRaiinNo = 6,
    InvalidListFileIdDeletes = 7,
    ValidateSuccess = 8,
}
