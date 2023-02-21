namespace UseCase.InsuranceMst.SaveHokenMaster
{
    public enum SaveHokenMasterStatus
    {
        Successful,
        Exception,
        Failed,
        InvalidHpId,
        InvalidStartDate,
        InvalidEndDate,
        InvalidStartDateMoreThanEndDate,
        InvalidPrefNo,
        InvalidHokenNo,
        InvalidDuplicateKey,
        InvalidAgeStartMoreThanAgeEnd,
    }
}
