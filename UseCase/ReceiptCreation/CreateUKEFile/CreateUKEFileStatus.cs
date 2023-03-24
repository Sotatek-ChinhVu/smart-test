namespace UseCase.ReceiptCreation.CreateUKEFile
{
    public enum CreateUKEFileStatus
    {
        Successful,
        Failed,
        NoData,
        InvalidHpId,
        InvaliSeikyuYm,
        ErrorValidateRosai,
        ErrorValidateAftercare,
        ErrorInputData,
        WarningInputData,
        WarningIncludeOutDrug,
        ConfirmCreateUKEFile
    }

    public enum ModeTypeCreateUKE
    {
        Shaho,
        Kokuho,
        Rosai,
        Aftercare
    }
}
