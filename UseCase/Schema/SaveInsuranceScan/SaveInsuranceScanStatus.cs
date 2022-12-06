namespace Schema.Insurance.SaveInsuranceScan
{
    public enum SaveInsuranceScanStatus
    {
        Successful,
        Failed,
        FailedSaveToDb,
        Exception,
        InvalidImageScan,
        InvalidHpId,
        InvalidPtId,
        RemoveOldImageFailed,
        RemoveOldImageSuccessful,
        OldImageNotFound
    }
}
