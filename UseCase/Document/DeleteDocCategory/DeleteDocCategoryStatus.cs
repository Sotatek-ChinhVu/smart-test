namespace UseCase.Document.DeleteDocCategory;

public enum DeleteDocCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidDocCategoryCd = 5,
    ValidateSuccess = 6,
}
