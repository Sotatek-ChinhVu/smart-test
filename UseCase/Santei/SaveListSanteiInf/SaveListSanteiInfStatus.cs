namespace UseCase.Santei.SaveListSanteiInf;

public enum SaveListSanteiInfStatus : byte
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidPtId = 3,
    InvalidItemCd = 4,
    InvalidAlertDays = 5,
    InvalidAlertTerm = 6,
    InvalidEndDate = 7,
    InvalidKisanSbt = 8,
    InvalidKisanDate = 9,
    InvalidByomei = 10,
    InvalidHosokuComment = 11,
    InvalidHpId = 12,
    InvalidUserId = 13,
}
