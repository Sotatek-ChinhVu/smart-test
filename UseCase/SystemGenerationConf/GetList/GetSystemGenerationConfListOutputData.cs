using Domain.Models.SystemGenerationConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemGenerationConf.GetList
{
    public class GetSystemGenerationConfListOutputData : IOutputData
    {
        public GetSystemGenerationConfListOutputData(List<SystemGenerationConfModel> systemGenerationConfModels, GetSystemGenerationConfListStatus status)
        {
            SystemGenerationConfModels = systemGenerationConfModels;
            Status = status;
        }

        public List<SystemGenerationConfModel> SystemGenerationConfModels { get; private set; }

        public GetSystemGenerationConfListStatus Status { get; private set; }
    }
}
