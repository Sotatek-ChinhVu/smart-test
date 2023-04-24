namespace UseCase.SetKbnMst.Upsert
{
    public enum UpsertSetKbnMstStatus : byte
    {
        Successed = 1,
        InvalidInputData = 2,
        InvalidHpId = 3,
        InvalidSetKbn = 4,
        InvalidSetKbnEdaNo = 5,
        InvalidSetKbnName = 6,
        InvalidKaCd = 7,
        InvalidDocCd = 8,
        InvalidIsDelete = 9,
        InvalidGenerationId = 10,
        InvalidSinDate = 11,
        Failed = 12,
    }
}