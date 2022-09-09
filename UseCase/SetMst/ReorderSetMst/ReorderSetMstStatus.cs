namespace UseCase.SetMst.ReorderSetMst;

public enum ReorderSetMstStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidLevel = 3,
    InvalidHpId = 4,
    InvalidDragSetCd = 5,
    InvalidDropSetCd = 6,
}
