namespace UseCase.User.CheckedLockMedicalExamination;

public enum CheckedLockMedicalExaminationStatus
{
    Successed = 1,
    InvalidSinDate,
    InvalidHpId,
    InvalidPtId,
    InvalidUserId,
    InvalidRaiinNo,
    InvalidToken,
    Failed
}
