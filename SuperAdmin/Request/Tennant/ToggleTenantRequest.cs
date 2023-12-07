namespace SuperAdminAPI.Request.Tennant
{
    public class ToggleTenantRequest
    {
        public int TenantId { get; set; }
        
        /// <summary>
        /// 0: stoped
        /// 1: Start
        /// </summary>
        public int Type { get; set; }
    }
}
