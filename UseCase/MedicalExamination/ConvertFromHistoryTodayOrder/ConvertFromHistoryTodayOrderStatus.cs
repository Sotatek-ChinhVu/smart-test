namespace UseCase.MedicalExamination.ConvertFromHistoryTodayOrder
{
    public enum ConvertFromHistoryTodayOrderStatus : byte
    {
        Successed = 1,
        InvalidHpId,
        InvalidSinDate,
        InvalidRaiinNo,
        InvalidUserId,
        InvalidPtId,
        InputNoData
    }
}
