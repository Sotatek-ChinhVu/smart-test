using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetPathAll
{
    public class GetPathAllOutputData : IOutputData
    {
        public GetPathAllOutputData(List<SystemConfListXmlPathModel> systemConfListXmlPathModels, GetPathAllStatus status)
        {
            SystemConfListXmlPathModels = systemConfListXmlPathModels;
            Status = status;
        }

        public List<SystemConfListXmlPathModel> SystemConfListXmlPathModels { get; private set; }

        public GetPathAllStatus Status { get; private set; }
    }
}
