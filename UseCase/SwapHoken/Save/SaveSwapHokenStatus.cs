namespace UseCase.SwapHoken.Save
{
    public enum SaveSwapHokenStatus
    {
        Successful = 1,
        SourceInsuranceHasNotSelected = 2,
        DesInsuranceHasNotSelected = 3,
        StartDateGreaterThanEndDate = 4,
        InvalidHpId = 5,
        InvalidPtId = 6,
        Failed = 7,
        CantExecCauseNotValidDate = 8
    }
}
