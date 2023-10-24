namespace UseCase.Insurance.FindPtHokenList
{
    public enum FindPtHokenListStatus : byte
    {
        Failed = 5,
        InvalidSinDate = 4,
        InvalidHpId = 3,
        InvalidPtId = 2,
        Successed = 1
    }
}