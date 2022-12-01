namespace UseCase.Document.CheckExistFileName;

public enum CheckExistFileNameStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidCategoryCd = 4,
    InvalidFileName = 5,
    InvalidPtId = 6,
    ValidateSuccess = 7,
}
