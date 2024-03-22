namespace UseCase.SuperAdmin.TerminateTenant
{
    public enum TerminateTenantStatus : byte
    {
        Successed = 1,
        InvalidTenantId = 2,
        Failed = 3,
        TenantDoesNotExist = 4,
        TenantDbDoesNotExistInRDS = 5,
        PathFileDumpRestoreNotAvailable = 6,
        TenantIsTerminating = 7
    }
}
