﻿namespace UseCase.SuperAdmin.TenantOnboard
{
    public enum TenantOnboardStatus
    {
        InvalidRequest = 0,
        Successed = 1,
        InvalidSize = 2,
        InvalidSizeType = 3,
        InvalidClusterMode = 4,
        Failed = 5,
        SubDomainExists = 6,
        InvalidSubDomain = 7,
        HopitalExists = 8,
    }
}
