namespace UseCase.Receipt.Recalculation;

public enum RecalculationStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidSinYm = 4,
}
