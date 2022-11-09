namespace UseCase.SystemConf
{
    public enum GetSystemConfStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidGrpCd = 3,
        InvalidGrpEdaNo = 4,
        Failed = 5
    }
}
