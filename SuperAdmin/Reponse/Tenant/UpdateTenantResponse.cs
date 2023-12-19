namespace SuperAdminAPI.Reponse.Tenant
{
    public class UpdateTenantResponse
    {
       public UpdateTenantResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
