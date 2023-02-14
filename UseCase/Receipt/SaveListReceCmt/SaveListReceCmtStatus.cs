namespace UseCase.Receipt.SaveListReceCmt;

public enum SaveListReceCmtStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidPtId = 5,
    InvalidSinYm = 6,
    InvalidItemCd = 7,
    InvalidReceCmtId = 8,
    InvalidCmtKbn = 9,
    InvalidCmtSbt = 10,
}
