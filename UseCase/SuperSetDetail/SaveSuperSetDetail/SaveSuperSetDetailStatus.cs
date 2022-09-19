namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public enum SaveSuperSetDetailStatus : byte
{
    Successed = 1,
    Failed = 2,
    SaveSetByomeiFailed = 3,
    SaveSetKarteInfFailed = 4,
    SaveSetOrderInfFailed = 5,
    ValidateSuccess = 6,
    InvalidSetByomeiId = 7,
    InvalidHpId = 8,
    InvalidSetCd = 9,
    InvalidUserId = 10,
    InvalidSikkanKbn = 11,
    InvalidNanByoCd = 12,
    InvalidByomeiCdOrSyusyokuCd = 13,
    FullByomeiMaxlength160 = 14,
    ByomeiCmtMaxlength80 = 15,
    SetCdNotExist = 16,
}
