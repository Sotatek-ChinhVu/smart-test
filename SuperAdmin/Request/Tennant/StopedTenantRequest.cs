namespace SuperAdminAPI.Request.Tennant
{
    public class StopedTenantRequest
    {
        public int TenantId { get; set; }
        
        /// <summary>
        /// 0: stoped
        /// 1: Start
        /// </summary>
        public int Type { get; set; }
    }
}
