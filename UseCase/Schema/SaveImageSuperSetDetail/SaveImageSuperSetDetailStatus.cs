namespace UseCase.Schema.SaveImageSuperSetDetail;

public enum SaveImageSuperSetDetailStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidOldImage = 3,
    InvalidFileImage = 4,
    InvalidSetCd = 5,
    DeleteSuccessed = 6,
}
