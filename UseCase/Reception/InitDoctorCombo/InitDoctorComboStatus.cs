namespace UseCase.Reception.InitDoctorCombo;

public enum InitDoctorComboStatus : byte
{
    Successed = 0,
    InvalidHpId = 1,
    InvalidPtId = 2,
    InvalidSinDate = 3,
    InvalidUserId = 4,
    Failed = 5
}