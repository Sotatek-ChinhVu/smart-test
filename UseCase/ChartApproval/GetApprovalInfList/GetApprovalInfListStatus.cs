namespace UseCase.ChartApproval.GetApprovalInfList
{
    public enum GetApprovalInfListStatus
    {
        Success,
        NoData,
        ApprovalInfListNotExisted,
        InvalidStartDate,
        InvalidEndDate,
        InvalidKaId,
        InvalidTantoId,
        InvalidStartDateMoreThanEndDate,
        ConfirmStartDateEndDate
    }
}
