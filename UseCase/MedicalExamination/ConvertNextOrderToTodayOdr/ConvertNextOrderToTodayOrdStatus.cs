namespace UseCase.MedicalExamination.ConvertNextOrderToTodayOdr
{
    public enum ConvertNextOrderToTodayOrdStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidRaiinNo = 3,
        InvalidSinDate = 4,
        InvalidUserId = 5,
        InvalidOrderInfs = 6,
        InvalidPtId = 7
    }
}
