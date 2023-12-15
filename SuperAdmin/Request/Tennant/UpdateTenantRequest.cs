namespace SuperAdminAPI.Request.Tennant
{
    public class UpdateTenantRequest
    {
        public int TenantId { get; set; }

        public string SubDomain { get; set; } = string.Empty;

        public double Size { get; set; }

        public int SizeType { get; set; }

        public byte Type { get; set; }

        public string Hospital { get; set; } = string.Empty;

        public int AdminId { get; set; }

        public string Password { get; set; } = string.Empty;

    }
}
