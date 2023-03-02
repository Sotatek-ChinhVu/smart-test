namespace UseCase.MedicalExamination.CheckedExpired
{
    public enum CheckedExpiredStatus : byte
    {
        Successed = 1,
        InValidHpId,
        InValidSinDate,
        InputNotData,
    }
}
