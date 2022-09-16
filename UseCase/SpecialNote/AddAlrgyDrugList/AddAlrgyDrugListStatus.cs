namespace UseCase.SpecialNote.AddAlrgyDrugList
{
    public enum AddAlrgyDrugListStatus : byte
    {
        Successed = 1,
        InvalidPtId = 2,
        HpIdNoExist = 3,
        PtIdNoExist = 4,
        InvalidSortNo = 5,
        InvalidStartDate = 6,
        InvalidEndDate = 7,
        ItemCdNoExist = 8,
        InvalidDuplicate = 9,
        InvalidItemCd = 10,
        InvalidDrugName = 11,
        InvalidCmt = 12,
        InputNoData = 13,
        Failed = 14,
    }
}
