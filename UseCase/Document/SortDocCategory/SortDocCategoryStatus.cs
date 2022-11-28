namespace UseCase.Document.SortDocCategory;

public enum SortDocCategoryStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
    InvalidMoveInCd = 5,
    InvalidMoveOutCd = 6,
    ValidateSuccess = 7,
}
