namespace UseCase.Receipt.SaveReceiptEdit;

public enum SaveReceiptEditStatus : byte
{
    ValidateSuccess,
    Successed,
    Failed,
    InvalidPtId,
    InvalidSinYm,
    InvalidSeikyuYm,
    InvalidHokenId,
    InvalidSeqNo,
    InvalidNissuItem,
    InvalidTokkiItem,
}
