namespace UseCase.SwapHoken.Save
{
    public enum SaveSwapHokenStatus
    {
        Successful,
        SourceInsuranceHasNotSelected,
        DesInsuranceHasNotSelected,
        StartDateGreaterThanEndDate,
        InvalidHpId,
        InvalidPtId,
        Failed,
        CantExecCauseNotValidDate,
        InvalidIsShowConversionCondition,
        ConfirmSwapHoken
    }
}
