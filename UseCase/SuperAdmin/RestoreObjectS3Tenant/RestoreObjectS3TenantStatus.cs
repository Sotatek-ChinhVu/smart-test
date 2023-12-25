namespace UseCase.SuperAdmin.RestoreObjectS3Tenant
{
    public enum RestoreObjectS3TenantStatus
    {
        Success = 1,
        Failed = 2,
        SubdomainDoesNotExist = 3,
        TenantIsProcessOfRestoreS3 = 4
    }
}
