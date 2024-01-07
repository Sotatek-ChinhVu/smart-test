namespace UseCase.SuperAdmin.TenantOnboard
{
    public enum TenantOnboardStatus
    {
        InvalidRequest = 0,
        TenantOnboardSuccessed = 1,
        InvalidSize = 2,
        InvalidSizeType = 3,
        InvalidClusterMode = 4,
        TenantOnboardFailed = 5,
        SubDomainExists = 6,
        InvalidSubDomain = 7,
        HopitalExists = 8,
    }
}
