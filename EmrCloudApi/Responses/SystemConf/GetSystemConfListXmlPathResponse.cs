using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf
{
    public class GetSystemConfListXmlPathResponse
    {
        public List<SystemConfListXmlPathModel> SystemConfListXmlPath { get; private set; }

        public GetSystemConfListXmlPathResponse(List<SystemConfListXmlPathModel> systemConfListXmlPath)
        {
            SystemConfListXmlPath = systemConfListXmlPath;
        }
    }
}