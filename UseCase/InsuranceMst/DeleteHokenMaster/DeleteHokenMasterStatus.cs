namespace UseCase.InsuranceMst.DeleteHokenMaster
{
    public enum DeleteHokenMasterStatus
    {
        Successful,
        Failed,
        InvalidHpId,
        InvalidHokenNo,
        InvalidHokenEdaNo,
        InvalidPrefNo,
        InvalidStartDate,
        InvalidHokenNoLessThan900,
        Exception
    }
}
