namespace UseCase.Receipt.SaveReceStatus;

public enum SaveReceStatusStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed,
    InvalidPtId,
    InvalidSinYm,
    InvalidSeikyuYm,
    InvalidHokenId,
    InvalidFusenKbn,
    InvalidStatusKbn,
}
