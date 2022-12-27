namespace UseCase.Schema.SaveListFileTodayOrder;

public enum SaveListFileTodayOrderStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidFileImage = 3,
    InvalidPtId = 4,
    InvalidTypeUpload = 5,
    InvalidSetCd = 6,
    ValidateSuccess = 7,
}
