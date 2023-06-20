
using Domain.Models.SystemGenerationConf;

namespace EmrCloudApi.Responses.SystemGenerationConf
{
    public class GetSystemGenerationConfListResponse
    {
        public GetSystemGenerationConfListResponse(List<SystemGenerationConfModel> systemGenerationConfModels)
        {
            SystemGenerationConfModels = systemGenerationConfModels;
        }

        public List<SystemGenerationConfModel> SystemGenerationConfModels { get; private set; }
    }
}
