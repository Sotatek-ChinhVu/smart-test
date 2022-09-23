using Domain.Models.OrdInfs;

namespace EmrCloudApi.Tenant.Responses.OrdInfs
{
    public class GetHeaderInfResponse
    {
        public GetHeaderInfResponse(OrdInfModel odrInfs)
        {
            OdrInfs = odrInfs;
        }

        public OrdInfModel OdrInfs { get; private set; }
    }
}
