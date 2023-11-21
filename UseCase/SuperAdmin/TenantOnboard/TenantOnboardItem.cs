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
        public TenantOnboardItem(string message)
        {
            Message = message;
        }

        public string Message { get; private set; } = string.Empty;
    }
}
