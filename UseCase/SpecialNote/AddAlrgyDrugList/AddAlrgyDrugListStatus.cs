namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public enum AddAlrgyDrugListStatus : byte
    {
        InvalidHpId = 0,
        Successed = 1,
        InvalidPtId = 2,
        HpIdNoExist = 3,
        PtIdNoExist = 4,
        InvalidSeqNo = 5,
        InvalidSortNo = 6,
        InvalidStartDate = 7,
        InvalidEndDate = 8,
        InvalidIsDeleted = 8,
        InvalidDuplicate = 9,
        InputNoData = 10,
    }
}
