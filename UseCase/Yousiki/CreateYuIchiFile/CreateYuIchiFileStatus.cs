namespace UseCase.Yousiki.CreateYuIchiFile;

public enum CreateYuIchiFileStatus : byte
{
    ValidateSuccessed = 0,
    Successed = 1,
    Failed = 2,
    InvalidCreateYuIchiFileSinYm = 3,
    InvalidCreateYuIchiFileCheckedOption = 4,
    CreateYuIchiFileSuccessed = 5,
    CreateYuIchiFileFailed = 6,
}
