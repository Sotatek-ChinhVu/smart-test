namespace UseCase.Receipt.SaveListReceCmt;

public enum SaveListReceCmtStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidPtId = 5,
    InvalidSinYm = 6,
}
