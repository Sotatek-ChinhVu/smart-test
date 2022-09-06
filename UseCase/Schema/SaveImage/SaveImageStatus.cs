namespace UseCase.Schema.SaveImage;

public enum SaveImageStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidOldImage = 3,
    InvalidFileImage = 4,
    InvalidPtId = 5,
}