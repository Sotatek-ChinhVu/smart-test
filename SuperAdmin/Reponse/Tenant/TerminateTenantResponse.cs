using UseCase.SuperAdmin.TenantOnboard;

namespace SuperAdminAPI.Reponse.Tenant
{
    public class TerminateTenantResponse
    {
        public TerminateTenantResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
