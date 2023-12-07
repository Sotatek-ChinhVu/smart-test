namespace SuperAdminAPI.Reponse.Tenant
{
    public class RestoreObjectS3TenantResponse
    {
        public RestoreObjectS3TenantResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
