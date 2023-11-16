namespace SuperAdminAPI.Request.Tennant
{
    public class TenantOnboardRequest
    {
        public string? Hospital { get; set; }
        public string? AdminId { get; set; }
        public string? Password { get; set; }
        public string? SubDomain { get; set; }
        public int Size { get; set; }
        public int SizeType { get; set; }
        public int ClusterMode { get; set; }
    }
}
