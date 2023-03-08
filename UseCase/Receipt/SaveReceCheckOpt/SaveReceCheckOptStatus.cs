namespace UseCase.Receipt.SaveReceCheckOpt;

public enum SaveReceCheckOptStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidErrCd = 3,
    InvalidCheckOpt = 4,
}
