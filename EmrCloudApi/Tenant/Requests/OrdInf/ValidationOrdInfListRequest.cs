using Domain.Models.OrdInfs;

namespace EmrCloudApi.Tenant.Requests.OrdInfs
{
    public class ValidationOrdInfListRequest
    {
        public List<OdrInfItem> OrdInfs { get; set; } = new();
    }
}
