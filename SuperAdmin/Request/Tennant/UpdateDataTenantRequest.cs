namespace SuperAdminAPI.Request.Tennant
{
    public class UpdateDataTenantRequest
    {
        public int TenantId { get; set; }
        public IFormFile FileUpdate { get; set; }
    }
}
    