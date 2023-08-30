using Domain.Models.SystemConf;

namespace EmrCloudApi.Requests.SystemConf
{
    public class SavePathRequest
    {
        public List<SystemConfListXmlPathModel> SystemConfListXmlPathModels { get; set; } = new();
    }
}
