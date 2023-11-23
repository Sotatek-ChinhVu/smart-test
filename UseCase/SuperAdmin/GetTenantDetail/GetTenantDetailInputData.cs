﻿using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetTenantDetail;

public class GetTenantDetailInputData : IInputData<GetTenantDetailOutputData>
{
    public GetTenantDetailInputData(int tenantId)
    {
        TenantId = tenantId;
    }

    public int TenantId { get; private set; }
}