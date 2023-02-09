using UseCase.Core.Sync.Core;

namespace UseCase.User.CheckedLockMedicalExamination;

public class CheckedLockMedicalExaminationOutputData : IOutputData
{
    public CheckedLockMedicalExaminationOutputData(CheckedLockMedicalExaminationStatus status, bool isLocked)
    {
        Status = status;
        IsLocked = isLocked;
    }

    public CheckedLockMedicalExaminationStatus Status { get; private set; }
    public bool IsLocked { get; private set; }
}
