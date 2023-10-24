namespace UseCase.MstItem.UploadImageDrugInf;

public enum UploadImageDrugInfStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidTypeImage = 3,
    InvalidFileInput = 4,
    InvalidSizeFile = 5,
}
