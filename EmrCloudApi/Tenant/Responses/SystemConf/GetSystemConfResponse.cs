using Domain.Models.SystemConf;

namespace EmrCloudApi.Tenant.Responses.SystemConf
{
    public class GetSystemConfResponse
    {
        public GetSystemConfResponse(SystemConfModel data)
        {
            Data = data;
        }

        public SystemConfModel Data { get; private set; }
    }
}
