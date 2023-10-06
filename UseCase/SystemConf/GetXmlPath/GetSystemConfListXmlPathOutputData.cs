using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetXmlPath
{
    public class GetSystemConfListXmlPathOutputData : IOutputData
    {
        public List<SystemConfListXmlPathModel> SystemConfListXmlPath { get; private set; }

        public GetSystemConfListXmlPathStatus Status { get; private set; }

        public GetSystemConfListXmlPathOutputData(List<SystemConfListXmlPathModel> systemConfListXmlPath, GetSystemConfListXmlPathStatus status)
        {
            SystemConfListXmlPath = systemConfListXmlPath;
            Status = status;
        }

        public GetSystemConfListXmlPathOutputData(GetSystemConfListXmlPathStatus status)
        {
            SystemConfListXmlPath = new List<SystemConfListXmlPathModel>();
            Status = status;
        }
    }
}