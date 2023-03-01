using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetSystemConf
{
    public class GetSystemConfOutputData : IOutputData
    {
        public GetSystemConfOutputData(SystemConfModel model, GetSystemConfStatus status)
        {
            Model = model;
            Status = status;
        }

        public GetSystemConfOutputData(GetSystemConfStatus status)
        {
            Model = new SystemConfModel();
            Status = status;
        }

        public SystemConfModel Model { get; private set; }

        public GetSystemConfStatus Status { get; private set; }
    }
}
