namespace SuperAdminAPI.Request.Tennant
{
    public class TenantOnboardRequest
    {
        public int TenantId { get; set; }
        public string Hospital { get; set; } = string.Empty;
        public int AdminId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string SubDomain { get; set; } = string.Empty;
    }
}
