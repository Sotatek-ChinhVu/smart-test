namespace UseCase.MedicalExamination.Karte2Print
{
    public enum Karte2PrintStatus : byte
    {
        Successed = 0,
        InvalidHpId = 1,
        InvalidPtId = 2,
        InvalidSinDate = 3,
        InvalidUser = 4,
        InvalidDeleteCondition = 5,
        NoData = 6,
        Failed = 7,
        InvalidUrl = 8,
        InvalidStartDate = 9,
        InvalidEndDate = 10,

    }
}
