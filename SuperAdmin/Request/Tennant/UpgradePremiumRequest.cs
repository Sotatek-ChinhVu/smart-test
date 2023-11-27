namespace SuperAdminAPI.Request.Tennant
{
    public class UpgradePremiumRequest
    {
        public int TenantId { get; set; }
        public string Domain { get; set; }
        public int Size { get; set; }
        public int SizeType { get; set; }
    }
}
