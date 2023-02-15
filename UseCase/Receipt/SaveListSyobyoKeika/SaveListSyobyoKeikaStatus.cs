namespace UseCase.Receipt.SaveListSyobyoKeika;

public enum SaveListSyobyoKeikaStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidSinYm = 4,
    InvalidSeqNo = 5,
    InvalidSinDay = 6,
}
