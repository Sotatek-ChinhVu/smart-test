namespace UseCase.Document.UploadTemplateToCategory;

public enum UploadTemplateToCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidCategoryCd = 4,
    InvalidFileInput = 5,
    ExistFileTemplateName = 6,
    InvalidExtentionFile = 7,
}
