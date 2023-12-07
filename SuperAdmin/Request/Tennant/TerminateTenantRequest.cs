namespace SuperAdminAPI.Request.Tennant
{
    public class TerminateTenantRequest
    {
        public int TenantId { get; set; }

        /// <summary>
        /// 0: Hard Terminate
        /// 1: Soft Terminate
        /// </summary>
        public int Type { get; set; }
    }
}
