using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf
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
