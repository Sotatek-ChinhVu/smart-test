namespace UseCase.MedicalExamination.SaveMedical;

public enum SaveMedicalStatus : byte
{
    Successed = 1,
    Failed = 2,
    MedicalScreenLocked = 3,
    NoPermissionSaveSummary = 4,
}
