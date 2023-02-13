namespace UseCase.UserConf.GetListMedicalExaminationConfig;

public enum GetListMedicalExaminationConfigStatus : byte
{
    Successed = 1,
    Failed = 2,
    InvalidHpId = 3,
    InvalidUserId = 4,
}
