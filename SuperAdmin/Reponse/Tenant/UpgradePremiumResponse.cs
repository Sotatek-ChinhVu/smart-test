namespace SuperAdminAPI.Reponse.Tenant
{
    public class UpgradePremiumResponse
    {
       public UpgradePremiumResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
