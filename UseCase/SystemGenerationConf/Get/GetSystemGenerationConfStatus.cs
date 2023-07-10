namespace UseCase.SystemGenerationConf.Get
{
    public enum GetSystemGenerationConfStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidGrpCd = 3,
        InvalidGrpEdaNo = 4,
        InvalidPresentDate = 5,
        InvalidDefaultValue = 6,
        Failed = 7
    }
}
