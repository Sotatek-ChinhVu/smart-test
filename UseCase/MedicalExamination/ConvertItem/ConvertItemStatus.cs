namespace UseCase.MedicalExamination.ConvertItem
{
    public enum ConvertItemStatus : byte
    {
        Successed = 1,
        InValidHpId,
        InValidUserId,
        InValidRaiinNo,
        InValidPtId,
        InValidSinDate,
        InputNotData,
        Failed,
    }
}
