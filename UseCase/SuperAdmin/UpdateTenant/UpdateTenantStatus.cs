namespace UseCase.SuperAdmin.UpgradePremium
{
    public enum UpdateTenantStatus : byte
    {
        Successed = 1,
        InvalidTenantId = 2,
        Failed = 4,
        TenantDoesNotExist = 5,
        RdsDoesNotExist = 6,
        NewDomainAleadyExist = 7,
        InvalidSize = 8,
        InvalidSizeType = 9,
        InvalidDomain = 10,
        InvalidHospital = 11,
        InvalidAdminId = 12,
        InvalidPassword = 13,
        NotAllowUpdateTenantDedicateToSharing = 14,
        TenantNotReadyToUpdate = 15
    }
}
