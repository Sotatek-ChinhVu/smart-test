namespace UseCase.MedicalExamination.GetOrderSheetGroup
{
    public enum GetOrderSheetGroupStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidPtId,
        InvalidUserId
    }
}
