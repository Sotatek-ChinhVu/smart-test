namespace UseCase.FlowSheet.Upsert;

public enum UpsertFlowSheetStatus
{
    Success = 1,
    UpdateNoSuccess,
    InputDataNoValid,
    RainNoNoValid,
    PtIdNoValid,
    SinDateNoValid,
    TagNoNoValid,
    CmtKbnNoValid,
    RainListCmtSeqNoNoValid,
    RainListTagSeqNoNoValid,
    PtIdNoExist,
    RaiinNoExist
}