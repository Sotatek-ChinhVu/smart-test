namespace UseCase.Receipt.ValidateCreateUKEFile
{
    public enum ValidateCreateUKEFileStatus
    {
        Successful,
        InvalidHpId,
        InvaliSeikyuYm,
        ErrorValidateRosai,
        ErrorValidateAftercare
    }
}
