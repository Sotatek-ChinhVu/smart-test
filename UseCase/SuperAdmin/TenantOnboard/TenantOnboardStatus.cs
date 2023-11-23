namespace UseCase.SuperAdmin.TenantOnboard
{
    public enum TenantOnboardStatus
    {
        Successed = 1,
        InvalidSize = 2,
        InvalidSizeType = 3,
        InvalidClusterMode = 4,
        Failed = 5,
        SubDomainExists = 6
    }
}
