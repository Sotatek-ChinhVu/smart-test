namespace SuperAdminAPI.Request.Tennant
{
    public class TenantOnboardRequest
    {
        public int TenantId { get; set; }
        public string Hospital { get; set; } = string.Empty;
        public int AdminId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string SubDomain { get; set; } = string.Empty;
        public int Size { get; set; }

        /// <summary>
        /// 1: MB , 2: GB
        /// </summary>
        public int SizeType { get; set; }

        /// <summary>
        /// 0: Sharing , 1: Dedicated
        /// </summary>
        public byte ClusterMode { get; set; }
    }
}
