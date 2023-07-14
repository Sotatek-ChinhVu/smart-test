namespace UseCase.Reception.RevertDeleteNoRecept
{
    public enum RevertDeleteNoReceptStatus : byte
    {
        Success = 1,
        InvalidRaiinNo = 2,
        InvalidHpId = 3,
        Failed = 4
    }
}
