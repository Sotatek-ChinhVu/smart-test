﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SuperAdminModels.MigrationTenantHistory
{
    public interface IMigrationTenantHistoryRepository
    {
        List<string> GetMigration(int tenantId);
        bool AddMigrationHistory(int tenantId, string migrationId);
    }
}
