namespace UseCase.SetMst.SaveSetMst;

public enum SaveSetMstStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidSindate = 3,
    InvalidSetCd = 4,
    InvalidSetKbn = 5,
    InvalidSetKbnEdaNo = 6,
    InvalidGenarationId = 7,
    InvalidLevel1 = 8,
    InvalidLevel2 = 9,
    InvalidLevel3 = 10,
    InvalidSetName = 11,
    InvalidWeightKbn = 12,
    InvalidColor = 13,
    InvalidIsDeleted = 14,
}
