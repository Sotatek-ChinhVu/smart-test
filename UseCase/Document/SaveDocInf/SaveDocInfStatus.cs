namespace UseCase.Document.SaveDocInf;

public enum SaveDocInfStatus:byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidCategoryCd = 5,
    ValidateSuccess = 6,
    InvalidPtId = 7,
    InvalidSindate = 8,
    InvalidDisplayFileName = 9,
    InvalidRaiinNo = 10,
    InvalidFileInput = 11,
}
