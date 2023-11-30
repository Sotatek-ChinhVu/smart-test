namespace SuperAdminAPI.Reponse.Tenant
{
    public class RestoreTenantResponse
    {
        public RestoreTenantResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
