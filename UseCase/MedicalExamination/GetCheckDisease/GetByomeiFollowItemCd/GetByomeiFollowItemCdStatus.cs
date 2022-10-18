namespace UseCase.MedicalExamination.GetByomeiFollowItemCd
{
    public enum GetByomeiFollowItemCdStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidSinDate = 3,
        InvalidItemCd = 4,
        InvalidByomeis = 5,
        NoData = 6,
        Failed = 7
    }
}
