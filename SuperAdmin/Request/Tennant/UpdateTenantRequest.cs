namespace SuperAdminAPI.Request.Tennant
{
    public class UpdateTenantRequest
    {
        public int TenantId { get; set; }
        public IFormFile FileUpdate { get; set; }
    }
}
    