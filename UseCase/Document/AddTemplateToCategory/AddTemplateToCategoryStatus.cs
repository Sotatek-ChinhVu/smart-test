namespace UseCase.Document.AddTemplateToCategory;

public enum AddTemplateToCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidCategoryCd = 4,
    InvalidFileInput = 5,
}
