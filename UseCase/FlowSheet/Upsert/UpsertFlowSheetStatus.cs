namespace UseCase.FlowSheet.Upsert;

public enum UpsertFlowSheetStatus
{
    Success = 1,
    InvalidSinDate,
    InvalidRaiinNo,
    InvalidPtId,
    InvalidTagNo,
    InvalidCmtKbn,
    UpdateNoSuccess
}
