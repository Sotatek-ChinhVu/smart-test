using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.SuperAdmin.TenantOnboard
{
    public class TenantOnboardItem
    {
        public TenantOnboardItem() { }
        public TenantOnboardItem(string message, string rdsEndpoint, string tenantUrl)
        {
            Message = message;
            RdsEndpoint = rdsEndpoint;
            TenantUrl = tenantUrl;
        }

        public string Message { get; private set; } = string.Empty;
        public string RdsEndpoint { get; private set; } = string.Empty;
        public string TenantUrl { get; private set; } = string.Empty;
    }
}
