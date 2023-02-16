namespace UseCase.Receipt.SaveListReceCmt;

public enum SaveReceCmtListStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidSinYm = 4,
    InvalidItemCd = 5,
    InvalidReceCmtId = 6,
    InvalidCmtKbn = 7,
    InvalidCmtSbt = 8,
    InvalidCmt = 9,
    InvalidCmtData = 10,
}
