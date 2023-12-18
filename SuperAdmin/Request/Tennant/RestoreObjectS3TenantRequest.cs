
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace SuperAdminAPI.Request.Tennant
{
    public class RestoreObjectS3TenantRequest
    {
        public string ObjectName { get; set; } = string.Empty;

        public RestoreObjectS3TenantTypeEnum Type { get; set; }

        public bool IsPrefixDelete {  get; set; }


    }
}
