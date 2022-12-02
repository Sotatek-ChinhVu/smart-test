namespace UseCase.Document.GetDocCategoryDetail;

public enum GetDocCategoryDetailStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidCategoryCd = 4,
    InvalidPtId = 5,
}
