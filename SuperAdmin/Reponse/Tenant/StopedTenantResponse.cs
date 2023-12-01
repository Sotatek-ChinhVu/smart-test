namespace SuperAdminAPI.Reponse.Tenant
{
    public class StopedTenantResponse
    {
        public StopedTenantResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
