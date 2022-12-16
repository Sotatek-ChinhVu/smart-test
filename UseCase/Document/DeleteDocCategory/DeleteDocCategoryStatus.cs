namespace UseCase.Document.DeleteDocCategory;

public enum DeleteDocCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    DocCategoryNotFound = 3,
    InvalidPtId = 4,
    MoveDocCategoryNotFound = 5,
    InvalidUserId = 6,
    ValidateSuccess = 7,
}
