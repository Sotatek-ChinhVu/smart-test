namespace UseCase.SetMst.CopyPasteSetMst;

public enum CopyPasteSetMstStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidLevel = 3,
    InvalidHpId = 4,
    InvalidUserId = 5,
    InvalidCopySetCd = 6,
    InvalidPasteSetCd = 7,
    InvalidPasteSetKbn = 8,
    InvalidPasteSetKbnEdaNo = 9,
}
