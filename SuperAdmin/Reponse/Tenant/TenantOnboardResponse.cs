using UseCase.SuperAdmin.TenantOnboard;

namespace SuperAdminAPI.Reponse.Tenant
{
    public class TenantOnboardResponse
    {
        public TenantOnboardResponse(TenantOnboardItem data, TenantOnboardStatus status)
        {
            Data = data;
            Status = status;
        }
        public TenantOnboardItem Data { get; private set; }
        public TenantOnboardStatus Status { get; private set; }
    }
}
