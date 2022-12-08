namespace UseCase.Document.SaveListDocCategory;

public enum SaveListDocCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidCategoryCd = 5,
    InvalidCategoryName = 6,
    InvalidSortNo = 7,
    ValidateSuccess = 8,
}
