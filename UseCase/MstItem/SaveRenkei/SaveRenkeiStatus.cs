namespace UseCase.MstItem.SaveRenkei;

public enum SaveRenkeiStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidRenkeiId = 3,
    InvalidParam = 4,
    InvalidPtNumLength = 5,
    InvalidTemplateId = 6,
    InvalidIsInvalid = 7,
    InvalidBiko = 8,
    InvalidPath = 9,
    InvalidMachine = 10,
    InvalidWorkPath = 11,
    InvalidUser = 12,
    InvalidPassWord = 13,
    InvalidEventCd = 14,
    InvalidRenkeiSbt = 15,
    InvalidRenkeiTimingModelList = 16,
}
