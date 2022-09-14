namespace UseCase.MstItem.DiseaseSearch;

public enum DiseaseSearchStatus : byte
{
    Successed = 1,
    InvalidPageIndex = 2,
    InvalidPageCount = 3,
    Failed = 4
}
