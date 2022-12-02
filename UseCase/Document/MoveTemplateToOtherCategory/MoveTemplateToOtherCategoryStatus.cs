namespace UseCase.Document.MoveTemplateToOtherCategory;

public enum MoveTemplateToOtherCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidOldCategoryCd = 4,
    InvalidNewCategoryCd = 5,
    InvalidFileName = 6,
    FileTemplateNotFould = 7,
}
