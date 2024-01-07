
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace SuperAdminAPI.Request.Tennant
{
    public class RestoreObjectS3TenantRequest
    {
        public string ObjectName { get; set; } = string.Empty;

        public List<RestoreObjectS3TenantTypeEnum> Type { get; set; } = new();

        public bool IsPrefixDelete { get; set; }


    }
}
