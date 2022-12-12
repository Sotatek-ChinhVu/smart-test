namespace UseCase.Document.MoveTemplateToOtherCategory;

public enum MoveTemplateToOtherCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidOldCategoryCd = 4,
    InvalidNewCategoryCd = 5,
    FileTemplateNotFould = 6,
    FileTemplateIsExistInNewFolder = 7,
    VaidateSuccess = 8
}
