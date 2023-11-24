﻿using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpgradePremiumInputData : IInputData<UpgradePremiumOutputData>
    {
        public UpgradePremiumInputData(int tenantId, int size, int sizeType, string subDomain)
        {
            TenantId = tenantId;
            Size = size;
            SizeType = sizeType;
            SubDomain = subDomain;
        }
        public int TenantId { get; private set; }

        public int Size { get; private set; }

        public int SizeType { get; private set; }

        public string SubDomain { get; private set; } 
    }
}
