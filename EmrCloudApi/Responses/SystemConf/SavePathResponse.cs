using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf;

public class GetAllPathResponse
{
    public GetAllPathResponse(List<SystemConfListXmlPathModel> systemConfListXmlPathModels)
    {
        SystemConfListXmlPathModels = systemConfListXmlPathModels;
    }

    public List<SystemConfListXmlPathModel> SystemConfListXmlPathModels { get; private set; }
}
