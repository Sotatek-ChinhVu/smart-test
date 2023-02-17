namespace UseCase.Receipt.SaveReceCheckCmtList;

public enum SaveReceCheckCmtListStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidSinYm = 4,
    InvalidHokenId = 5,
}
