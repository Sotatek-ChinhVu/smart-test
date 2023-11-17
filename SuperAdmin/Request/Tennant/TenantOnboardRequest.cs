namespace SuperAdminAPI.Request.Tennant
{
    public class TenantOnboardRequest
    {
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
        /// 1: Sharing , 2: Dedicated
        /// </summary>
        public byte ClusterMode { get; set; }
    }
}
