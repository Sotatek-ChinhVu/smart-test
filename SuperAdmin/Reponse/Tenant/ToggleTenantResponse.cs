namespace SuperAdminAPI.Reponse.Tenant
{
    public class ToggleTenantResponse
    {
        public ToggleTenantResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
